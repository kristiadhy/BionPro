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
    protected List<string>? ValidationErrorMessage;

    protected async Task RegisterUser(UserInitialRegistrationDto userInitialRegistrationDto)
    {
        bool confirmationStatus = await ConfirmationModalService.CustomSaveConfirmation("Registration", "Save this registration?");
        if (!confirmationStatus)
            return;

        IsSaving = true;
        StateHasChanged();

        ValidationErrorMessage = null;

        UserRegistrationDTO userDto = new()
        {
            FirstName = userInitialRegistrationDto.FirstName,
            LastName = userInitialRegistrationDto.LastName,
            Email = userInitialRegistrationDto.Email,
            UserName = userInitialRegistrationDto.Email,
            Password = userInitialRegistrationDto.ConfirmPassword,
            Roles = ["Administrator"]
        };
        var responseDto = await AuthService.RegisterUser(userDto);
        if (responseDto is not null)
        {
            if (responseDto?.Message == "INVALID_VALIDATION")
            {
                ValidationErrorMessage = responseDto?.Errors?.ToList();
            }
            else
            {
                AlertVisible = true;
                ErrorMessage = responseDto?.Message;
            }
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
