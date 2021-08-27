namespace Pezza.Core.Customer.Commands
{
    using FluentValidation;
    using Pezza.Common.Validators;

    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            this.RuleFor(r => r.Data.Name)
                .MaximumLength(100)
                .NotEmpty();

            this.RuleFor(r => r.Data.Phone)
                .MaximumLength(20)
                .Matches(@"^[0-9]*$")
                .NotEmpty();

            this.RuleFor(r => r.Data.Email)
                .MaximumLength(200)
                .EmailAddress()
                .NotEmpty();

            this.RuleFor(r => r.Data.ContactPerson)
                .MaximumLength(200)
                .NotEmpty();

            this.RuleFor(r => r.Data.Address)
                .NotNull()
                .SetValidator(new AddressValidator());
        }
    }
}
