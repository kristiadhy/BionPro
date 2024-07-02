using Domain.Entities;
using FluentValidation;

namespace Application.Validators;
public class ProductValidator : AbstractValidator<ProductModel>
{
    public ProductValidator()
    {
        RuleFor(prop => prop.Name)
         .NotEmpty()
         .MaximumLength(200);

        RuleFor(prop => prop.Price)
            .NotEmpty();
    }

    public void ValidateInput(ProductModel productModel)
    {
        var validationResult = Validate(productModel);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult);
    }
}
