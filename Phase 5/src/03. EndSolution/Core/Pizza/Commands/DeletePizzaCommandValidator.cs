namespace Core.Pizza.Commands;

public sealed class DeletePizzaCommandValidator : AbstractValidator<DeletePizzaCommand>
{
	public DeletePizzaCommandValidator()
	{
		this.RuleFor(r => r.Id)
			.NotEmpty();
	}
}