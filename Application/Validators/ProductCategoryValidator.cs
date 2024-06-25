using Domain.Entities;
using FluentValidation;

namespace Application.Validators;
public class ProductCategoryValidator : AbstractValidator<ProductCategoryModel>
{
    public ProductCategoryValidator()
    {
        RuleFor(prop => prop.Name)
         .NotEmpty()
         .MaximumLength(200);
    }

    public void ValidateInput(ProductCategoryModel productCategoryModel)
    {
        var validationResult = Validate(productCategoryModel);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult);
    }
}
