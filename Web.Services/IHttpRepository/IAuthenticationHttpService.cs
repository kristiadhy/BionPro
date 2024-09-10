using Domain.DTO;

namespace Web.Services.IHttpRepository;

public interface IAuthenticationHttpService
{
    Task<string> GetCurrentTokenFromLocalStorage();
    Task<ApiResponseDto<List<string>>?> RegisterUser(UserRegistrationDTO userForRegistration);
    Task Login(UserAuthenticationDTO userForAuthentication);
    Task Logout();
    Task<string> RefreshToken();
}