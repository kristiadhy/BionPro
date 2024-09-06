using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.Services.IHttpRepository;

namespace WebAssembly.Pages;

public partial class Login
{
    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    IAuthenticationService AuthService { get; set; } = default!;
    bool AlertVisible = false;
    string ErrorMessage = string.Empty;

    protected UserAuthenticationDTO loginData = new();
    protected bool IsSaving = false;

    protected async Task OnLogin(UserAuthenticationDTO userDto)
    {
        IsSaving = true;
        try
        {
            var tokenResponse = await AuthService.Login(userDto);
        }
        catch
        {
            AlertVisible = true;
            ErrorMessage = "Wrong username/password";
            return;
        }
        finally
        {
            IsSaving = false;
        }
        AlertVisible = false;
        NavigationManager.NavigateTo($"/");
    }
}
