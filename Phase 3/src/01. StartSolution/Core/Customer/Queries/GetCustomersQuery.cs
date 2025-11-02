namespace Core.Customer.Queries;

using Common.Models.Results;

public interface IGetCustomersQuery
{
	Task<Result<IEnumerable<CustomerModel>>> ExecuteAsync(CancellationToken cancellationToken = default);
}

public sealed class GetCustomersQuery(DatabaseContext databaseContext) : IGetCustomersQuery
{
	public async Task<Result<IEnumerable<CustomerModel>>> ExecuteAsync(CancellationToken cancellationToken = default)
	{
		var entities = databaseContext.Customers.Select(x => x).AsNoTracking();

		var count = await entities.CountAsync(cancellationToken);
		var paged = await entities.ToListAsync(cancellationToken);

		return Result<IEnumerable<CustomerModel>>.Success(paged.Map(), count);
	}
}