using Domain.DTO;
using FluentValidation;

namespace WebAssembly.Validators;

public class ProductValidator : AbstractValidator<ProductDto>
{
    public ProductValidator()
    {
        RuleFor(prop => prop.CategoryID)
            .NotEmpty()
            .WithMessage("'Product Category' must not be empty");

        RuleFor(prop => prop.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}
