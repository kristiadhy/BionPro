using Domain.DTO;

namespace WebAssembly.StateManagement;

public class UserRegistrationState
{
  public UserInitialRegistrationDto UserRegistration { get; set; } = new();

  public void ResetUserRegistration()
  {
    UserRegistration = new UserInitialRegistrationDto();
  }
}
