using Application.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Validators;
using Domain.DTO;
using Microsoft.AspNetCore.WebUtilities;
using EmailService;
using MimeKit;

namespace Services.Services;
public partial class AuthenticationService
{
    private async Task<IdentityResult> RegisterUser(UserRegistrationDTO userForRegistration)
    {
        _logger.Information("Checking user's default role");
        if (!await _roleManager.RoleExistsAsync(userForRegistration.Roles?.First()!))
            throw new RoleNotFoundException();

        var user = _mapper.Map<UserModel>(userForRegistration);
        //The create async method and add to roles async method are the built in method provided by the microsoft identity class
        _logger.Information("Creating new user", userForRegistration.UserName);
        var result = await _userManager.CreateAsync(user, userForRegistration.Password!);
        if (result.Succeeded)
        {
            _logger.Information("New user created");
            _logger.Information("Setting up role for user");
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles!);
            _logger.Information("New user and its role successfully created");
        }

        return result;
    }

    private async Task SendConfirmationLink(UserRegistrationDTO userForRegistration)
    {
        //This method is used to generate a confirmation link for the user to confirm their email.
        //We use the GenerateEmailConfirmationTokenAsync method from the UserManager class to generate a token for the user.
        //The token is then used to generate a confirmation link that is sent to the user's email.
        var user = _mapper.Map<UserModel>(userForRegistration);
        _logger.Information("Generating email confirmation token");
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var param = new Dictionary<string, string>()
        {
            { "token", token },
            { "email", userForRegistration.Email!}
        };
        userForRegistration.ClientUri = $"https://localhost:7229/api/emailconfirmation";
        var callBack = QueryHelpers.AddQueryString(userForRegistration.ClientUri!, param!);

        //Send the confirmation link to the user's email
        var message = new MessageModel([userForRegistration.Email!], "Email Confirmation Token", $"Please confirm your email by clicking this link: {callBack}");

        _logger.Information("Sending verification email to the user");
        await _emailSender.SendEmailAsync(message);
        _logger.Information("Verification email has been sent!");
    }

    private async Task ValidateUserModel(UserRegistrationDTO userForRegistration)
    {
        _logger.Information("Validating user for registration");
        var validator = new UserValidator();
        await validator.ValidateInput(userForRegistration);
        _logger.Information("User validated");
    }
}
