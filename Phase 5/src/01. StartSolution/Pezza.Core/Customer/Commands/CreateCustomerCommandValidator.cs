namespace Core.Customer.Commands;

using FluentValidation;
using Common.Behaviours.Validators;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(r => r.Data.Name)
            .MaximumLength(100)
            .NotEmpty();

        RuleFor(r => r.Data.Phone)
            .MaximumLength(20)
            .Matches(@"^[0-9]*$")
            .NotEmpty();

        RuleFor(r => r.Data.Email)
            .MaximumLength(200)
            .EmailAddress()
            .NotEmpty();

        RuleFor(r => r.Data.ContactPerson)
            .MaximumLength(200)
            .NotEmpty();

        RuleFor(r => r.Data.Address)
            .NotNull()
            .SetValidator(new AddressValidator());
    }
}
