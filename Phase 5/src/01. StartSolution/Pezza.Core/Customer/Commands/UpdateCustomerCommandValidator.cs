namespace Core.Customer.Commands;

using FluentValidation;
using Common.Behaviours.Validators;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        this.RuleFor(r => r.Data)
            .NotNull();

        this.RuleFor(r => r.Data.Id)
            .NotEmpty();

        this.RuleFor(r => r.Data.Name)
            .MaximumLength(100);

        this.RuleFor(r => r.Data.Phone)
            .MaximumLength(20)
            .Matches(@"^[0-9]*$");

        this.RuleFor(r => r.Data.Email)
            .MaximumLength(200)
            .EmailAddress();

        this.RuleFor(r => r.Data.ContactPerson)
            .MaximumLength(200);

        this.RuleFor(r => r.Data.Address)
            .SetValidator(new AddressUpdateValidator())
            .When(x => x.Data.Address != null);
    }
}
