namespace Core.Todos.Commands;

using Common.Models.Todos;
using LazyCache;

public class CompleteTodoCommand : IRequest<Result<TodoModel>>
{
	public required int? Id { get; set; }
}

public class CompleteTodoCommandHandler(DatabaseContext databaseContext, IAppCache cache) : IRequestHandler<CompleteTodoCommand, Result<TodoModel>>
{
	private const string ERROR = "Error completing a task";

	public async Task<Result<TodoModel>> Handle(CompleteTodoCommand request, CancellationToken cancellationToken)
	{
		var id = request.Id!;
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Todos.FirstOrDefault(c => c.Id == id));
		var findEntity = await query(databaseContext, id.Value);
		if (findEntity is null)
		{
			return Result<TodoModel>.Failure("Not found");
		}

		findEntity.IsCompleted = true;
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		cache.Remove(Common.CacheData.CacheKey);
		return result > 0 ? Result<TodoModel>.Success(findEntity.Map()) : Result<TodoModel>.Failure(ERROR);
	}
}