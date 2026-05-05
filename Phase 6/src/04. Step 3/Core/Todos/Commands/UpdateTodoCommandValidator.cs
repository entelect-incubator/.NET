namespace Core.Todos.Commands;

public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
	public UpdateTodoCommandValidator()
	{
		this.RuleFor(x => x.Id)
			.NotEmpty().WithMessage("Task id is required.");

		this.RuleFor(x => x.Data.Task)
			.MinimumLength(3).When(x => x.Data.Task is not null).WithMessage("Task must be at least 3 characters long if provided.").MaximumLength(250)
			.When(x => x.Data.Task is not null).WithMessage("Task must be less then 250 characters long.");

		this.RuleFor(x => x.Data.IsCompleted)
			.NotNull().When(x => x.Data.IsCompleted.HasValue).WithMessage("IsCompleted should be either true or false if provided.");

		this.RuleFor(x => x.Data.SessionId)
			.NotEmpty().WithMessage("SessionId must be a valid non-empty GUID.");
	}
}