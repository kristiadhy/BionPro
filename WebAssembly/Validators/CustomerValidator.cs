using Domain.DTO;
using FluentValidation;

namespace WebAssembly.Validators;

public class CustomerValidator : AbstractValidator<CustomerDTO>
{
    public CustomerValidator()
    {
        RuleFor(prop => prop.CustomerName)
            .NotEmpty()
            .MaximumLength(200)
            ;

        RuleFor(prop => prop.PhoneNumber)
            .MaximumLength(50)
            ;

        RuleFor(prop => prop.Email)
           .MaximumLength(100)
           .EmailAddress()
           ;

        RuleFor(prop => prop.ContactPerson)
           .MaximumLength(100)
           ;
    }
}
