using Domain.DTO;
using Domain.Enum;
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
    [Inject]
    UserRegistrationState UserRegistrationState { get; set; } = default!;

    protected bool IsSaving = false;
    protected bool IsSuccess = false;
    private bool AlertVisible = false;
    private string? ErrorMessage;
    private string? SuccessfulEmailRegistered;
    protected List<string>? ValidationErrorMessage;

    protected async Task RegisterUser(UserInitialRegistrationDto userInitialRegistrationDto)
    {
        bool confirmationStatus = await ConfirmationModalService.CustomSaveConfirmation("Registration", "Save this registration?");
        if (!confirmationStatus)
            return;

        IsSaving = true;
        StateHasChanged();

        //Need to reset the valdation error message before calling the service
        ValidationErrorMessage = null;

        UserRegistrationDTO userDto = new()
        {
            UserName = userInitialRegistrationDto.Username,
            Email = userInitialRegistrationDto.Email,
            Password = userInitialRegistrationDto.ConfirmPassword,
            Roles = ["Administrator"]
        };
        var responseDto = await AuthService.RegisterUser(userDto);
        //If responseDto is not null, it means there is an error
        if (responseDto is not null)
        {
            //Check the error type, is it validation error or not
            //Validation error will be recognized with the message "INVALID_VALIDATION"
            if (responseDto?.Type == ErrorMessageEnum.InvalidValidation)
            {
                ValidationErrorMessage = responseDto?.Errors?.ToList();
            }
            //If it is not a validation error, then show the error as an alert
            else
            {
                AlertVisible = true;
                ErrorMessage = responseDto?.Message;
            }
        }
        // If responseDto is null, it means there is no error, then the registration is successful
        else
        {
            IsSuccess = true;
            //Need to set the email that has been registered successfully because the state will be reset
            SuccessfulEmailRegistered = UserRegistrationState.UserRegistration.Email;
            UserRegistrationState.ResetUserRegistration();
        }

        IsSaving = false;
    }

    protected void EvBackToPrevious()
    {
        NavigationManager.NavigateTo($"/login");
    }
}
