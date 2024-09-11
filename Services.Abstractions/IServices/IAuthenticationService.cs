using Domain.DTO;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts;
public interface IAuthenticationService
{
    public Task EmailConfirmation(string email, string token);
    Task<ApiResponseDto<List<string>>> RegisterAndSendConfirmationLink(UserRegistrationDTO userForRegistration);
    Task<ApiResponseDto<string>> ValidateUser(UserAuthenticationDTO userForAuth);
    Task<TokenDTO> CreateToken(bool populateExp);
    Task<TokenDTO> RefreshToken(TokenDTO tokenDto);
}
