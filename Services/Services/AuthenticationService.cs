using Application.Exceptions;
using Application.IRepositories;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Services;
public partial class AuthenticationService : IAuthenticationService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    //This class is use to provide the APIs for managing users and roles in a persistence store
    private readonly UserManager<UserModel> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailSender _emailSender;

    private UserModel? _user;
    private readonly JwtConfiguration _jwtConfiguration;

    public AuthenticationService(IRepositoryManager repositoryManager,
                                 IMapper mapper,
                                 ILogger logger,
                                 UserManager<UserModel> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IConfiguration configuration,
                                 IEmailSender emailSender)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _emailSender = emailSender;

        //We want to use JWT configuration model instead of taking it from the appsettings.json.
        //Please take a look at the JwtInstaller class for more details.
        _jwtConfiguration = new JwtConfiguration();
        _configuration.Bind(_jwtConfiguration.Section, _jwtConfiguration);
    }

    public async Task<ApiResponseDto<List<string>>> RegisterAndSendConfirmationLink(UserRegistrationDTO userForRegistration)
    {
        //Validate user and default role first before registering the user. If it throws an exception, then cancel the registration process.
        await ValidateUserModel(userForRegistration);
        await CheckingUserDefaultRole(userForRegistration);

        var userModel = _mapper.Map<UserModel>(userForRegistration);
        var identityResult = await RegisterUser(userForRegistration, userModel);
        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.Errors)
            {
                return new ApiResponseDto<List<string>>
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid Validation",
                    Data = identityResult.Errors.Select(e => e.Description).ToList()
                };
            }
        }
        await SendConfirmationLink(userForRegistration, userModel);
        await CreateDefaultRole(userForRegistration, userModel);

        return new ApiResponseDto<List<string>> { IsSuccess = true };
    }

    public async Task<ApiResponseDto<string>> ValidateUser(UserAuthenticationDTO userForAuth)
    {
        //Don't log anything related to the sensitive information
        _user = await _userManager.FindByNameAsync(userForAuth.UserName!);
        if (_user == null || !await _userManager.CheckPasswordAsync(_user, userForAuth.Password!))
        {
            _logger.Warning("Authentication failed. Invalid username or password.");
            return new ApiResponseDto<string>
            {
                IsSuccess = false,
                ErrorMessage = "Authentication failed. Invalid username or password."
            };
        }

        if (!await _userManager.IsEmailConfirmedAsync(_user))
        {
            _logger.Warning("Email hasn't been confirmed.");
            return new ApiResponseDto<string>
            {
                IsSuccess = false,
                ErrorMessage = "Your email hasn't been confirmed yet. Please confirm it."
            };
        }

        return new ApiResponseDto<string> { IsSuccess = true };
    }


    public async Task EmailConfirmation(string email, string token)
    {
        _logger.Information("Confirm an email");
        _logger.Debug("Confirm an email: {email}", email);
        var user = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException();
        var identityResult = await _userManager.ConfirmEmailAsync(user, token);
        if (!identityResult.Succeeded)
        {
            _logger.Debug("Error {@error}", identityResult.Errors);
            throw new EmailConfirmationFailedException();
        }
        _logger.Information("Email confirmed");
        _logger.Debug("Email confirmed: {email}", email);
    }

    public async Task<TokenDTO> CreateToken(bool populateExp)
    {
        _logger.Information("Preparing the token");
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var refreshToken = GenerateRefreshToken();
        _user!.RefreshToken = refreshToken;

        if (populateExp)
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        //Update user data in the database
        _logger.Information("Updating refresh token");
        await _userManager.UpdateAsync(_user);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        _logger.Information("New token created");

        return new TokenDTO(accessToken, refreshToken);
    }

    public async Task<TokenDTO> RefreshToken(TokenDTO tokenDto)
    {
        _logger.Information("Refresh JWT token");
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        var user = await _userManager.FindByNameAsync(principal.Identity!.Name!);
        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new RefreshTokenBadRequest();

        _user = user;

        return await CreateToken(populateExp: false);
    }

    private SigningCredentials GetSigningCredentials()
    {
        string? secretKey = Environment.GetEnvironmentVariable("SECRET");
        var key = Encoding.UTF8.GetBytes(secretKey!);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var lstClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user!.UserName!)
        };

        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            lstClaims.Add(new Claim(ClaimTypes.Role, role));
        }
        return lstClaims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtConfiguration.ValidIssuer,
            audience: _jwtConfiguration.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
            signingCredentials: signingCredentials
            );
        return tokenOptions;
    }

    private string GenerateRefreshToken()
    {
        //We use the RandomNumberGenerator class to generate a cryptographic random number for this purpose.
        var randomNumber = new byte[32]; using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber); return Convert.ToBase64String(randomNumber);
        }
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string AccessToken)
    {
        //GetPrincipalFromExpiredToken is used to get the user principal from the expired access token.
        //We make use of the ValidateToken method from the JwtSecurityTokenHandler class for this purpose

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!)),
            ValidateLifetime = false,
            ValidIssuer = _jwtConfiguration.ValidIssuer,
            ValidAudience = _jwtConfiguration.ValidAudience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(AccessToken, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        return principal;
    }
}
