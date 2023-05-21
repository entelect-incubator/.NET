namespace Core.Customer.Commands;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
	public CreateCustomerCommandValidator()
	{
		this.RuleFor(r => r.Data.Name)
			.MaximumLength(100)
			.NotEmpty();

		this.RuleFor(r => r.Data.Address)
			.MaximumLength(500)
			.NotEmpty();

		this.RuleFor(r => r.Data.Cellphone)
			.MaximumLength(50);

		this.RuleFor(r => r.Data.Email)
			.MaximumLength(500);
	}
}