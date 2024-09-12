using Domain.DTO;

namespace Web.Services.IHttpRepository;

public interface IAuthenticationHttpService
{
    Task<string> GetCurrentTokenFromLocalStorage();
    Task<ApiResponseDto<List<string>>?> RegisterUser(UserRegistrationDTO userForRegistration);
    Task Login(UserAuthenticationDTO userForAuthentication);
    Task Logout();
    Task ConfirmEmail(string email, string token);
    Task<string> RefreshToken();
}