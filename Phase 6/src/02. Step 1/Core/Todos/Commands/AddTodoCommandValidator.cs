namespace Core.Todos.Commands;

public class AddTodoCommandValidator : AbstractValidator<AddTodoCommand>
{
	public AddTodoCommandValidator()
	{
		this.RuleFor(x => x.Data.Task)
			.NotEmpty().WithMessage("Task name is required.")
			.MinimumLength(3).WithMessage("Task must be at least 3 characters long.")
			.MaximumLength(250).WithMessage("Task must be less then 250 characters long.");

		this.RuleFor(x => x.Data.IsCompleted)
			.NotNull().WithMessage("Task name is required.")
			.NotEmpty().WithMessage("Task name is required.");

		this.RuleFor(x => x.Data.SessionId)
			.NotEmpty().WithMessage("Session Id must be a valid non-empty GUID.");
	}
}