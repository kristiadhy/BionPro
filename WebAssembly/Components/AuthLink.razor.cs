using Microsoft.AspNetCore.Components;
using Web.Services.IHttpRepository;

namespace WebAssembly.Components;

public partial class AuthLink
{
  [Inject]
  NavigationManager NavigationHelper { get; set; } = default!;
  [Inject]
  IAuthenticationHttpService AuthService { get; set; } = default!;

  private void Logout()
  {
    AuthService.Logout();
    NavigationHelper.NavigateTo($"/login");
  }
}
