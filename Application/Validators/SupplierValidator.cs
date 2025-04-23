using Domain.Entities;
using FluentValidation;

namespace Application.Validators;

public class SupplierValidator : AbstractValidator<SupplierModel>
{
  public SupplierValidator()
  {
    RuleFor(prop => prop.SupplierName)
        .NotEmpty()
        .MaximumLength(200)
        ;

    RuleFor(prop => prop.PhoneNumber)
        //.NotEmpty()
        .MaximumLength(50)
        ;

    RuleFor(prop => prop.Email)
       //.NotEmpty()
       .MaximumLength(100)
       .EmailAddress()
       ;

    RuleFor(prop => prop.ContactPerson)
       .MaximumLength(100)
       ;
  }

  public void ValidateInput(SupplierModel supplierModel)
  {
    var validationResult = Validate(supplierModel);
    if (!validationResult.IsValid)
      throw new ValidationException(validationResult);
  }
}
