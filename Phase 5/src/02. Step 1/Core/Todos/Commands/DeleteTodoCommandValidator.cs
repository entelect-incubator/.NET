namespace Core.Todos.Commands;

public class DeleteTodoCommandValidator : AbstractValidator<DeleteTodoCommand>
{
	public DeleteTodoCommandValidator()
		=> this.RuleFor(x => x.Id).NotEmpty().WithMessage("Task id is required.");
}