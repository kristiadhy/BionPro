using Domain.DTO;
using FluentValidation;

namespace WebAssembly.Validators;

public class SaleValidator : AbstractValidator<SaleDto>
{
  public SaleValidator()
  {
    RuleFor(prop => prop.CustomerID)
        .NotEmpty()
        .WithMessage("'Customer' must not be empty");

    RuleFor(prop => prop.Date)
        .NotEmpty();

    RuleFor(prop => prop.SaleDetails)
       .NotEmpty().WithMessage("You should have at least 1 product/item in the table");

    //RuleForEach(p => p.SaleDetails)
    //   .SetValidator(new SaleDetailValidator());
  }
}
