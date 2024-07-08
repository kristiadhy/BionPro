using Domain.Entities;
using FluentValidation;

namespace Application.Validators;
public class PurchaseValidator : AbstractValidator<PurchaseModel>
{
    public PurchaseValidator()
    {
        RuleFor(prop => prop.TransactionCode)
         .NotEmpty()
         .MaximumLength(15);

        RuleFor(prop => prop.Date)
            .NotEmpty();
    }

    public void ValidateInput(PurchaseModel purchaseModel)
    {
        var validationResult = Validate(purchaseModel);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult);
    }
}
