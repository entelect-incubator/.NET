namespace Core.Pizza.Queries;

using Common.Entities;

public class GetNotifiesQuery : IRequest<ListResult<Notify>>
{
}

public class GetNotifiesQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetNotifiesQuery, ListResult<Notify>>
{
	public async Task<ListResult<Notify>> Handle(GetNotifiesQuery request, CancellationToken cancellationToken)
	{
		var entities = databaseContext.Notifies
			.Select(x => x)
			.Include(x => x.Customer)
			.Where(x => x.Sent == false)
			.AsNoTracking();

		var count = entities.Count();
		var data = await entities.ToListAsync(cancellationToken);

		return ListResult<Notify>.Success(data, count);
	}
}