namespace Core.Pizza.Queries;

using Common.Entities;

public sealed class GetNotifiesQuery : IQuery<Result<IEnumerable<Notify>>>
{
}

public sealed class GetNotifiesQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetNotifiesQuery, Result<IEnumerable<Notify>>>
{
	public async Task<Result<IEnumerable<Notify>>> Handle(GetNotifiesQuery request, CancellationToken cancellationToken)
	{
		var entities = databaseContext.Notifies
			.Select(x => x)
			.Include(x => x.Customer)
			.Where(x => x.Sent == false)
			.AsNoTracking();

		var count = await entities.CountAsync(cancellationToken);
		var data = await entities.ToListAsync(cancellationToken);

		return Result<IEnumerable<Notify>>.Success(data, count);
	}
}