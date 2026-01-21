namespace Core.Customer.Queries;

public sealed class GetCustomersQuery : IQuery<Result<IEnumerable<CustomerModel>>>
{
}

public sealed class GetCustomersQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetCustomersQuery, Result<IEnumerable<CustomerModel>>>
{
	public Task<Result<IEnumerable<CustomerModel>>> Handle(GetCustomersQuery query, CancellationToken ct) => throw new NotImplementedException();

	public async Task<Result<IEnumerable<CustomerModel>>> HandleAsync(GetCustomersQuery request, CancellationToken cancellationToken)
	{
		var entities = databaseContext.Customers.Select(x => x).AsNoTracking();

		var count = await entities.CountAsync(cancellationToken);
		var paged = await entities.ToListAsync(cancellationToken);

		return Result<IEnumerable<CustomerModel>>.Success(paged.Map(), count);
	}
}