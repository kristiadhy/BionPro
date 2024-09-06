using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Web.Services.IHttpRepository;

namespace WebAssembly.Pages;

public partial class UserRegistration
{
    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    CustomModalService ConfirmationModalService { get; set; } = default!;
    [Inject]
    IAuthenticationService AuthService { get; set; } = default!;

    protected UserInitialRegistrationDto initialRegistrationData = new();
    protected bool IsSaving = false;
    protected bool IsSuccess = false;
    private bool AlertVisible = false;
    private string? ErrorMessage;
    protected ErrorBoundary? errorBoundary;

    protected override void OnParametersSet() => errorBoundary?.Recover();

    protected async Task RegisterUser(UserInitialRegistrationDto userInitialRegistrationDto)
    {
        bool confirmationStatus = await ConfirmationModalService.CustomSaveConfirmation("Registration", "Save this registration?");
        if (!confirmationStatus)
            return;

        IsSaving = true;
        UserRegistrationDTO userDto = new()
        {
            Email = userInitialRegistrationDto.Email,
            Password = userInitialRegistrationDto.ConfirmPassword,
            Roles = ["Administrator"]
        };
        try
        {
            await AuthService.RegisterUser(userDto);
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
        IsSuccess = true;
    }

    protected void EvBackToPrevious()
    {
        NavigationManager.NavigateTo($"/login");
    }
}
