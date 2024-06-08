using Domain.DTO;
using FluentValidation;

namespace WebAssembly.Validators;

public class CustomerDtoValidator : AbstractValidator<CustomerDTO>
{
    public CustomerDtoValidator()
    {
        RuleFor(customer => customer.CustomerName)
            .NotEmpty()
            .MaximumLength(200)
            ;

        RuleFor(customer => customer.PhoneNumber)
             .NotEmpty()
            .MaximumLength(50)
            ;

        RuleFor(customer => customer.Email)
             .NotEmpty()
           .MaximumLength(100)
           .EmailAddress()
           ;

        RuleFor(customer => customer.ContactPerson)
             .NotEmpty()
           .MaximumLength(100)
           ;
    }
}
