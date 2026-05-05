namespace Core.Todos.Queries;

using Common;
using Common.Entities;
using Common.Models.Todos;
using LazyCache;

public class GetTodosQuery : IRequest<Result<IEnumerable<TodoModel>>>
{
	public required SearchTodoModel Data { get; set; }
}

public class GetTodosQueryHandler(DatabaseContext databaseContext, IAppCache cache) : IRequestHandler<GetTodosQuery, Result<IEnumerable<TodoModel>>>
{
	private readonly TimeSpan CacheExpiry = new(12, 0, 0);

	public async Task<Result<IEnumerable<TodoModel>>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
	{
		var entity = request.Data!;

		Task<IEnumerable<Todo>> DataDelegate() => this.GetData();
		var cachedData = await cache.GetOrAddAsync(CacheData.CacheKey, DataDelegate, this.CacheExpiry);

		if (cachedData is not null)
		{
			var data = cachedData?.AsQueryable()
				.FilterByTask(entity.Task)
				.FilterByCompleted(entity.IsCompleted)
				.FilterByDate(entity.DateCreated, entity.Year, entity.Month, entity.Day)
				.OrderBy(x => x.DateCreated)
				.ToList()!;

			return Result<IEnumerable<TodoModel>>.Success(data.Map().ToList(), cachedData.Count());
		}

		var entities = databaseContext.Todos.Select(x => x)
			.AsNoTracking()
			.FilterByTask(entity.Task)
			.FilterByCompleted(entity.IsCompleted)
			.FilterByDate(entity.DateCreated, entity.Year, entity.Month, entity.Day)
			.OrderBy(entity.OrderBy);		

		var count = entities.Count();

		if(count is 0)
		{
			return Result<IEnumerable<TodoModel>>.Success([], 0);
		}

		var paged = await entities.ApplyPaging(entity.PagingArgs).ToListAsync(cancellationToken);

		return Result<IEnumerable<TodoModel>>.Success(paged.Map(), count);
	}

	private async Task<IEnumerable<Todo>> GetData()
	{
		var entities = await databaseContext.Todos.Select(x => x)
			.AsNoTracking()
			.ToListAsync();

		return entities;
	}
}