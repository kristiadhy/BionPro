using Domain.DTO;
using FluentValidation;

namespace WebAssembly.Validators;

public class SaleDetailValidator : AbstractValidator<SaleDetailDto>
{
  public SaleDetailValidator()
  {
    RuleFor(prop => prop.ProductID)
        .NotEmpty()
        .WithMessage("'Product' must not be empty");

    RuleFor(prop => prop.Quantity)
        .NotEmpty();

    RuleFor(prop => prop.Price);
  }
}
