namespace Core.Todos.Commands;

using Common.Models.Todos;

public class UpdateTodoCommand : IRequest<Result<TodoModel>>
{
	public required int Id { get; set; }

	public required UpdateTodoModel Data { get; set; }
}

public class UpdateTodoCommandHandler(DatabaseContext databaseContext) : IRequestHandler<UpdateTodoCommand, Result<TodoModel>>
{
	private const string ERROR = "Error updating a task";

	public async Task<Result<TodoModel>> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
	{
		var id = request.Id;
		var model = request.Data;
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Todos.FirstOrDefault(c => c.Id == id));
		var findEntity = await query(databaseContext, id);
		if (findEntity is null)
		{
			return Result<TodoModel>.Failure("Not found");
		}

		findEntity.Task = !string.IsNullOrEmpty(model.Task) ? model.Task : findEntity.Task;
		findEntity.IsCompleted = model.IsCompleted ?? findEntity.IsCompleted;
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<TodoModel>.Success(findEntity.Map()) : Result<TodoModel>.Failure(ERROR);
	}
}