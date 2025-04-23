using Domain.Entities;
using FluentValidation;

namespace Application.Validators;
public class SaleValidator : AbstractValidator<SaleModel>
{
  public SaleValidator()
  {
    RuleFor(prop => prop.TransactionCode)
     //.NotEmpty()
     .MaximumLength(15);

    RuleFor(prop => prop.Date)
        .NotEmpty();
  }

  public void ValidateInput(SaleModel purchaseModel)
  {
    var validationResult = Validate(purchaseModel);
    if (!validationResult.IsValid)
      throw new ValidationException(validationResult);
  }
}
