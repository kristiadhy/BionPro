using Domain.DTO;
using FluentValidation;

namespace Application.Validators;

public class UserValidator : AbstractValidator<UserRegistrationDTO>
{
    public UserValidator()
    {
        RuleFor(prop => prop.Roles)
            .NotEmpty();

        RuleFor(prop => prop.Password)
            .NotEmpty();

        RuleFor(prop => prop.Email)
          .NotEmpty()
          .EmailAddress();
    }

    public void ValidateInput(UserRegistrationDTO userRegistrationDTO)
    {
        var validationResult = Validate(userRegistrationDTO);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult);
    }
}
