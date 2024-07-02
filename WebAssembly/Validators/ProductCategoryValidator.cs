using Domain.DTO;
using FluentValidation;

namespace WebAssembly.Validators;

public class ProductCategoryValidator : AbstractValidator<ProductCategoryDto>
{
    public ProductCategoryValidator()
    {
        RuleFor(prop => prop.Name)
            .NotEmpty()
            .MaximumLength(200)
            ;
    }
}
