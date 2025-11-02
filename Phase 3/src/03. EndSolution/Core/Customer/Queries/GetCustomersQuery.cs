namespace Core.Customer.Queries;

using Utilities.CQRS;
using Utilities.Results;

public record GetCustomers() : IQuery<Result<IEnumerable<CustomerModel>>>
{
	public Task<Result<IEnumerable<CustomerModel>>> ExecuteAsync(Dispatcher dispatcher, CancellationToken ct = default)
		=> dispatcher.Query<GetCustomers, Result<IEnumerable<CustomerModel>>>(this, ct);
};

public sealed class GetCustomersHandler(DatabaseContext databaseContext) : IQueryHandler<GetCustomers, Result<IEnumerable<CustomerModel>>>
{
	public async Task<Result<IEnumerable<CustomerModel>>> Handle(GetCustomers query, CancellationToken cancellationToken = default)
	{
		var entities = databaseContext.Customers.Select(x => x).AsNoTracking();

		var count = await entities.CountAsync(cancellationToken);
		var paged = await entities.ToListAsync(cancellationToken);

		return Result<IEnumerable<CustomerModel>>.Success(paged.Map(), count);
	}
}