namespace Core.Todos.Commands;

public class CompleteTodoCommandValidator : AbstractValidator<CompleteTodoCommand>
{
	public CompleteTodoCommandValidator()
		=> this.RuleFor(x => x.Id).NotEmpty().WithMessage("Task id is required.");
}