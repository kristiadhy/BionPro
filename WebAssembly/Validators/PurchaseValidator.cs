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

    RuleFor(prop => prop.PurchaseDetails)
       .NotEmpty().WithMessage("You should have at least 1 product/item in the table");

    //RuleForEach(p => p.PurchaseDetails)
    //   .SetValidator(new PurchaseDetailValidator());
  }
}
