using Domain.DTO;
using FluentValidation;

namespace WebAssembly.Validators;

public class UserInitialRegistrationValidator : AbstractValidator<UserInitialRegistrationDto>
{
    public UserInitialRegistrationValidator()
    {
        RuleFor(prop => prop.Password)
            .NotEmpty()
            .MinimumLength(8)
            ;

        RuleFor(prop => prop.ConfirmPassword)
            .NotEmpty()
            .Equal(prop => prop.Password).WithMessage("Passwords doesn't match.");

        RuleFor(prop => prop.Email)
            .NotEmpty()
            .EmailAddress()
            ;
    }
}

