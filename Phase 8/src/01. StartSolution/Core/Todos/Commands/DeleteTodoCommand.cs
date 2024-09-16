namespace Core.Todos.Commands;

public class DeleteTodoCommand : IRequest<Result>
{
	public required int Id { get; set; }
}

public class DeleteTodoCommandHandler(DatabaseContext databaseContext, IAppCache cache) : IRequestHandler<DeleteTodoCommand, Result>
{
	private const string ERROR = "Error deleting a task";

	public async Task<Result> Handle(DeleteTodoCommand request, CancellationToken cancellationToken = default)
	{
		var todo = await databaseContext.Todos.FindAsync(request.Id, cancellationToken);
		if (todo is not null)
		{
			databaseContext.Todos.Remove(todo);
			var result = await databaseContext.SaveChangesAsync(cancellationToken);

			cache.Remove(Common.CacheData.CacheKey);
			return result > 0 ? Result.Success() : Result.Failure(ERROR);
		}

		return Result.Failure(ERROR);
	}
}