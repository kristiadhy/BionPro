using Domain.DTO;
using Microsoft.AspNetCore.Components;
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
    private string ErrorMessage = string.Empty;
    private bool AlertVisible = false;

    protected async Task RegisterUser(UserInitialRegistrationDto userInitialRegistrationDto)
    {
        bool confirmationStatus = await ConfirmationModalService.CustomSaveConfirmation("User registration", "Save this registration?");
        if (!confirmationStatus)
            return;

        IsSaving = true;
        try
        {
            UserRegistrationDTO userDto = new()
            {
                Email = userInitialRegistrationDto.Email,
                Password = userInitialRegistrationDto.ConfirmPassword,
                Roles = ["Administrator"]
            };
            var responseDto = await AuthService.RegisterUser(userDto);
            if (!responseDto.IsSuccess)
                ErrorMessage = responseDto.Error;
        }
        catch
        {
            AlertVisible = true;
            ErrorMessage = "Registration Failed";
            return;
        }
        finally
        {
            IsSaving = false;
        }
        AlertVisible = false;

        IsSaving = false;
    }

    protected void EvBackToPrevious()
    {
        NavigationManager.NavigateTo($"/login");
    }
}
