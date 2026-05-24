namespace Core.Pizza.Queries;

public sealed class GetNotifiesQuery : IQuery<Result<IEnumerable<Common.Entities.Notify>>>
{
}

public sealed class GetNotifiesQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetNotifiesQuery, Result<IEnumerable<Common.Entities.Notify>>>
{
	public async Task<Result<IEnumerable<Common.Entities.Notify>>> Handle(GetNotifiesQuery request, CancellationToken cancellationToken)
	{
		var entities = databaseContext.Notifies
			.Select(x => x)
			.Include(x => x.Customer)
			.Where(x => x.Sent == false)
			.AsNoTracking();

		var count = await entities.CountAsync(cancellationToken);
		var data = await entities.ToListAsync(cancellationToken);

		return Result<IEnumerable<Common.Entities.Notify>>.Success(data, count);
	}
}