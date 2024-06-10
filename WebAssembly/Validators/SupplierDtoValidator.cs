using Domain.DTO;
using FluentValidation;

namespace WebAssembly.Validators;

public class SupplierDtoValidator : AbstractValidator<SupplierDto>
{
    public SupplierDtoValidator()
    {
        RuleFor(prop => prop.SupplierName)
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
