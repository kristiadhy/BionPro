using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Web.Services.IHttpRepository;

namespace WebAssembly.Pages;

public partial class Login
{
    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    IAuthenticationHttpService AuthService { get; set; } = default!;
    bool AlertVisible = false;
    string ErrorMessage = string.Empty;

    protected UserAuthenticationDTO loginData = new();
    protected bool IsSaving = false;

    protected async Task OnLogin(UserAuthenticationDTO userDto)
    {
        IsSaving = true;
        try
        {
            await AuthService.Login(userDto);
        }
        catch (Exception ex)
        {
            AlertVisible = true;
            ErrorMessage = ex.Message;
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
