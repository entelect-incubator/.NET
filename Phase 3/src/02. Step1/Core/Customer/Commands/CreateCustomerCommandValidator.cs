namespace Core.Customer.Commands;

using FluentValidation;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        this.RuleFor(r => r.Data.Name)
            .MaximumLength(100)
            .NotEmpty();

        this.RuleFor(r => r.Data.Cellphone)
            .MaximumLength(50)
            .Matches(@"^[0-9]*$")
            .NotEmpty();

        this.RuleFor(r => r.Data.Email)
            .MaximumLength(500)
            .EmailAddress()
            .NotEmpty();

        this.RuleFor(r => r.Data.Address)
			.MaximumLength(500)
			.NotNull()
			.NotEmpty();
	}
}
