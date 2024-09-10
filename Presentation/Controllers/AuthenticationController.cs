using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;

namespace Presentation.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/authentication")]
public class AuthenticationController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;

    [HttpPost("registration"), AllowAnonymous]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO userForRegistration)
    {
        //Look into identityresult class from asp.net core identity for more details.
        var result = await _serviceManager.AuthenticationService.RegisterUser(userForRegistration);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                //ModelState.TryAddModelError(error.Code, error.Description);
                ApiResponseDto<List<string>> apiResponseDto = new()
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid Validation",
                    Data = result.Errors.Select(e => e.Description).ToList()
                };
                return BadRequest(apiResponseDto);
            }
        }

        //If there is no error, then new user and its role is created sucessfully.
        return Created();
    }

    [HttpPost("login"), AllowAnonymous]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationDTO user)
    {
        var responseDto = await _serviceManager.AuthenticationService.ValidateUser(user);
        if (!responseDto.IsSuccess)
            return Unauthorized(responseDto);

        //Create a JWT token after a successfull login
        var tokenDTO = await _serviceManager.AuthenticationService.CreateToken(populateExp: true);
        return Ok(new ApiResponseDto<TokenDTO> { IsSuccess = true, Data = tokenDTO });
    }

    [HttpPost("refresh"), AllowAnonymous]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Refresh([FromBody] TokenDTO tokenDto)
    {
        var tokenDtoToReturn = await _serviceManager.AuthenticationService.RefreshToken(tokenDto);
        return Ok(tokenDtoToReturn);
    }
}
