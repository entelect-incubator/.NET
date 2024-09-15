namespace Core.Todos.Queries;

using Common.Models.Todos;

public class GetTodosQuery : IRequest<Result<IEnumerable<TodoModel>>>
{
	public Guid SessionId { get; set; }
}
public class GetTodosQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetTodosQuery, Result<IEnumerable<TodoModel>>>
{
	public async Task<Result<IEnumerable<TodoModel>>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
	{
		var data = databaseContext.Todos.Where(x => x.SessionId == request.SessionId).AsNoTracking();

		var count = data.Count();
		var result = await data.ToListAsync(cancellationToken);

		return Result<IEnumerable<TodoModel>>.Success(result.Map(), count);
	}
}