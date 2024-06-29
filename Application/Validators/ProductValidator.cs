using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators;
internal class ProductValidator : AbstractValidator<ProductModel>
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
