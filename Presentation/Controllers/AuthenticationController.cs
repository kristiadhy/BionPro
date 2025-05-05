using Asp.Versioning;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;

namespace Presentation.Controllers;

[ApiVersion("1.0")]
[ApiController]
[AllowAnonymous]
[Route("api/authentication")]
public class AuthenticationController(IServiceManager serviceManager) : ControllerBase
{
  private readonly IServiceManager _serviceManager = serviceManager;

  [HttpPost("registration")]
  [ServiceFilter(typeof(ValidationFilterAttribute))]
  public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO userForRegistration)
  {
    var apiResponseDto = await _serviceManager.AuthenticationService.RegisterAndSendConfirmationLink(userForRegistration);
    if (!apiResponseDto.IsSuccess)
      return BadRequest(apiResponseDto);

    return Created();
  }

  [HttpPost("login")]
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

  [HttpPost("refresh")]
  [ServiceFilter(typeof(ValidationFilterAttribute))]
  public async Task<IActionResult> Refresh([FromBody] TokenDTO tokenDto)
  {
    var tokenDtoToReturn = await _serviceManager.AuthenticationService.RefreshToken(tokenDto);
    return Ok(tokenDtoToReturn);
  }

  [HttpGet("emailconfirmation")]
  public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
  {
    await _serviceManager.AuthenticationService.EmailConfirmation(email, token);
    return Ok();
  }
}
