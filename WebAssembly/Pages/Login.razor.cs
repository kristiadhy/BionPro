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
    string? returnUrl;

    protected UserAuthenticationDTO loginData = new();
    protected bool IsSaving = false;

    protected override void OnInitialized()
    {
        var uri = new Uri(NavigationManager.Uri);
        var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        if (queryParams.TryGetValue("returnUrl", out var returnUrlParam))
        {
            returnUrl = returnUrlParam;
        }
    }

    protected async Task OnLogin(UserAuthenticationDTO userDto)
    {
        IsSaving = true;
        try
        {
            await AuthService.Login(loginData);
            AlertVisible = false;

            //Check the return URL. If it is not null, navigate to the return URL that user have visited before. Otherwise, navigate to the home page.
            if (!string.IsNullOrEmpty(returnUrl))
                NavigationManager.NavigateTo(returnUrl);
            else
                NavigationManager.NavigateTo("/");
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
    }
}
