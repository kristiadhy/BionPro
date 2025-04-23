using Domain.Entities;
using FluentValidation;

namespace Application.Validators;

public class CustomerValidator : AbstractValidator<CustomerModel>
{
  public CustomerValidator()
  {
    RuleFor(prop => prop.CustomerName)
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
       //.MustAsync(async (email, _) => await IsUniqueAsync(email))
       ;

    RuleFor(prop => prop.ContactPerson)
       .MaximumLength(100)
       ;
  }

  //Example of how to check uniqueness of email in the database
  private static async Task<bool> IsUniqueAsync(string? email)
  {
    await Task.Delay(300);
    return email?.ToLower() != "mail@my.com";
  }

  public async Task ValidateInput(CustomerModel customerMD)
  {
    var validationResult = await ValidateAsync(customerMD);
    if (!validationResult.IsValid)
      throw new ValidationException(validationResult);
  }
}
