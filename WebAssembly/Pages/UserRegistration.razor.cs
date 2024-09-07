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
        StateHasChanged();

        UserRegistrationDTO userDto = new()
        {
            FirstName = userInitialRegistrationDto.FirstName,
            LastName = userInitialRegistrationDto.LastName,
            Email = userInitialRegistrationDto.Email,
            UserName = userInitialRegistrationDto.Email,
            Password = userInitialRegistrationDto.ConfirmPassword,
            Roles = ["Administrator"]
        };
        var response = await AuthService.RegisterUser(userDto);
        if (response is not null && !response.IsSuccess)
        {
            AlertVisible = true;
            foreach (var error in response.Errors!)
                ErrorMessage = $"• {error}";
        }
        else
            IsSuccess = true;

        IsSaving = false;
    }

    protected void EvBackToPrevious()
    {
        NavigationManager.NavigateTo($"/login");
    }
}
