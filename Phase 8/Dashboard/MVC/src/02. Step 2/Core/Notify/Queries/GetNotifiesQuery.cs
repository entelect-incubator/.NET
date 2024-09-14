namespace Core.Pizza.Queries;

public class GetNotifiesQuery : IRequest<ListResult<Common.Entities.Notify>>
{
}

public class GetNotifiesQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetNotifiesQuery, ListResult<Common.Entities.Notify>>
{
	public async Task<ListResult<Common.Entities.Notify>> Handle(GetNotifiesQuery request, CancellationToken cancellationToken)
	{
		var entities = databaseContext.Notifies
			.Select(x => x)
			.Include(x => x.Customer)
			.Where(x => x.Sent == false)
			.AsNoTracking();

		var count = entities.Count();
		var data = await entities.ToListAsync(cancellationToken);

		return ListResult<Common.Entities.Notify>.Success(data, count);
	}
}