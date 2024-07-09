using Domain.DTO;
using FluentValidation;

namespace WebAssembly.Validators;

public class PurchaseValidator : AbstractValidator<PurchaseDto>
{
    public PurchaseValidator()
    {
        RuleFor(prop => prop.SupplierID)
            .NotEmpty()
            .WithMessage("'Supplier' must not be empty");

        RuleFor(prop => prop.Date)
            .NotEmpty();
    }
}
