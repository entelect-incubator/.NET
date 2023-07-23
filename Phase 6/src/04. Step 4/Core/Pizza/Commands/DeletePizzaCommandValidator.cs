namespace Core.Customer.Commands;

public class DeletePizzaCommandValidator : AbstractValidator<DeletePizzaCommand>
{
	public DeletePizzaCommandValidator()
	{
		this.RuleFor(r => r.Id)
			.NotEmpty();
	}
}