namespace Core.Customer.Commands;

public class CreatePizzaCommandValidator : AbstractValidator<CreatePizzaCommand>
{
	public CreatePizzaCommandValidator()
	{
		this.RuleFor(r => r.Data.Name)
			.MaximumLength(100)
			.NotEmpty();

		this.RuleFor(r => r.Data.Description)
			.MaximumLength(500)
			.NotEmpty();

		this.RuleFor(r => r.Data.Price)
			.PrecisionScale(4, 2, false)
			.NotEmpty();
	}
}