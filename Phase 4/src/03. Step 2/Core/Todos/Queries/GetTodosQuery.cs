namespace Core.Todos.Queries;

using Common.Models.Todos;

public class GetTodosQuery : IRequest<Result<IEnumerable<TodoModel>>>
{
	public required SearchTodoModel Data { get; set; }
}

public class GetTodosQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetTodosQuery, Result<IEnumerable<TodoModel>>>
{
	public async Task<Result<IEnumerable<TodoModel>>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
	{
		var entity = request.Data!;
		if (string.IsNullOrEmpty(entity.OrderBy))
		{
			entity.OrderBy = "DateCreated desc";
		}

		var entities = databaseContext.Todos.Select(x => x)
			.AsNoTracking()
			.FilterByTask(entity.Task)
			.FilterByCompleted(entity.IsCompleted)
			.FilterByDate(entity.DateCreated, entity.Year, entity.Month, entity.Day)
			.OrderBy(entity.OrderBy);		

		var count = entities.Count();
		var paged = await entities.ApplyPaging(entity.PagingArgs).ToListAsync(cancellationToken);

		return Result<IEnumerable<TodoModel>>.Success(paged.Map(), count);
	}
}