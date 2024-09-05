using Domain.DTO;
using FluentValidation;

namespace WebAssembly.Validators;

public class UserLoginValidator : AbstractValidator<UserAuthenticationDTO>
{
    public UserLoginValidator()
    {
        RuleFor(prop => prop.UserName)
            .NotEmpty().WithName("Username")
            .MaximumLength(256)
            ;

        RuleFor(prop => prop.Password)
            .NotEmpty()
            ;
    }
}
