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
            var response = new ResponseDto
            {
                IsSuccess = false,
                Error = result.Errors.Select(e => e.Description).ToList()
            };
            return BadRequest(response);

            //This is another way to return the error messages, I got this from Code Maze, but need to check it again since it is not working.
            //foreach (var error in result.Errors)
            //    ModelState.TryAddModelError(error.Code, error.Description);

            //return BadRequest(ModelState);
        }

        //If there is no error, then new user and its role is created sucessfully.
        return Created();
    }

    [HttpPost("login"), AllowAnonymous]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationDTO user)
    {
        if (!await _serviceManager.AuthenticationService.ValidateUser(user))
            return Unauthorized();

        //Create a JWT token after a successfull login
        var tokenDTO = await _serviceManager.AuthenticationService.CreateToken(populateExp: true);
        return Ok(tokenDTO);

    }

    [HttpPost("refresh"), AllowAnonymous]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Refresh([FromBody] TokenDTO tokenDto)
    {
        var tokenDtoToReturn = await _serviceManager.AuthenticationService.RefreshToken(tokenDto);
        return Ok(tokenDtoToReturn);
    }
}
