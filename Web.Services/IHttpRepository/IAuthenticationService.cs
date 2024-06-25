using Domain.DTO;

namespace Web.Services.IHttpRepository;

public interface IAuthenticationService
{
    Task<string> GetCurrentTokenFromLocalStorage();
    Task<ResponseDto> RegisterUser(UserRegistrationDTO userForRegistration);
    Task<TokenDTO> Login(UserAuthenticationDTO userForAuthentication);
    Task Logout();
    Task<string> RefreshToken();
}