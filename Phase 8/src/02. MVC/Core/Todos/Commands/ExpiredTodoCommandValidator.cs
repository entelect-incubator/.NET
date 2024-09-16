namespace Core.Todos.Commands;

public class ExpiredTodoCommandValidator : AbstractValidator<ExpiredTodoCommand>
{
	public ExpiredTodoCommandValidator()
	{
		this.RuleFor(x => x.ToEmail).NotEmpty().WithMessage("Email is required.").EmailAddress();
	}
}