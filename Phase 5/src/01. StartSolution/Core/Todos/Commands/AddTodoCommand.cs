namespace Core.Todos.Commands;

using Common.Models.Todos;

public class AddTodoCommand : IRequest<Result<TodoModel>>
{
	public required CreateTodoModel Data { get; set; }
}

public class AddTodoCommandHandler(DatabaseContext databaseContext) : IRequestHandler<AddTodoCommand, Result<TodoModel>>
{
	private const string ERROR = "Error adding a task";

	public async Task<Result<TodoModel>> Handle(AddTodoCommand request, CancellationToken cancellationToken)
	{
		var entity = request.Data.Map();
		entity.DateCreated = DateTime.UtcNow;
		databaseContext.Todos.Add(entity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<TodoModel>.Success(entity.Map()) : Result<TodoModel>.Failure(ERROR);
	}
}