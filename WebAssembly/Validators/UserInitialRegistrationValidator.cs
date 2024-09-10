using Domain.DTO;
using FluentValidation;

namespace WebAssembly.Validators;

public class UserInitialRegistrationValidator : AbstractValidator<UserInitialRegistrationDto>
{
    public UserInitialRegistrationValidator()
    {
        RuleFor(prop => prop.Password)
            .NotEmpty()
            //We need to add these rules to the password to force the validation executed in the front end
            //Because in the backend we use the Microsoft Identity framework to validate the password which implement these rules
            .MinimumLength(10)
            .Matches(@"\d").WithMessage("Password must contain at least one digit.");

        RuleFor(prop => prop.ConfirmPassword)
            .NotEmpty()
            .Equal(prop => prop.Password).WithMessage("Passwords doesn't match.");

        RuleFor(prop => prop.Email)
            .NotEmpty()
            .EmailAddress()
            ;

        RuleFor(prop => prop.Username)
            .NotEmpty();
    }
}

