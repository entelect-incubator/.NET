namespace Core.Customer.Queries;

public class GetCustomersQuery : IRequest<ListResult<CustomerModel>>
{
}

public class GetCustomersQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetCustomersQuery, ListResult<CustomerModel>>
{
	public async Task<ListResult<CustomerModel>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
	{
		var entities = databaseContext.Customers.Select(x => x).AsNoTracking();

		var count = entities.Count();
		var paged = await entities.ToListAsync(cancellationToken);

		return ListResult<CustomerModel>.Success(paged.Map(), count);
	}
}