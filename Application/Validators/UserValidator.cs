using Domain.DTO;
using FluentValidation;

namespace Application.Validators;

public class UserValidator : AbstractValidator<UserRegistrationDTO>
{
  public UserValidator()
  {
    RuleFor(prop => prop.UserName)
        .NotEmpty();

    RuleFor(prop => prop.Roles)
        .NotEmpty();

    RuleFor(prop => prop.Password)
        .NotEmpty();

    RuleFor(prop => prop.Email)
      .NotEmpty()
      .EmailAddress();
  }

  public async Task ValidateInput(UserRegistrationDTO userRegistrationDTO)
  {
    var validationResult = await ValidateAsync(userRegistrationDTO);
    if (!validationResult.IsValid)
      throw new ValidationException(validationResult);
  }
}
