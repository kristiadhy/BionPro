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
  IAuthenticationHttpService AuthService { get; set; } = default!;
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
    if (!await ConfirmationModalService.CustomSaveConfirmation("Registration", "Save this registration?"))
      return;

    IsSaving = true;
    StateHasChanged();

    ValidationErrorMessage = null;

    var userDto = new UserRegistrationDTO
    {
      UserName = userInitialRegistrationDto.Username!,
      Email = userInitialRegistrationDto.Email!,
      Password = userInitialRegistrationDto.ConfirmPassword!,
      Roles = ["Administrator"]
    };

    var responseDto = await AuthService.RegisterUser(userDto);

    // if the response is not null, then there is an error
    if (responseDto != null)
    {
      // if the data exists, then there are validation errors
      if (responseDto.Data?.Count > 0)
      {
        ValidationErrorMessage = responseDto.Data.ToList();
      }
      // Otherwise, it's an exception error
      else
      {
        AlertVisible = true;
        ErrorMessage = responseDto.ErrorMessage;
      }
    }
    else
    {
      IsSuccess = true;
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
