namespace Core.Customer.Queries;

using Common.Models.Customer;
using Common.Models.Results;

public interface IGetCustomerQuery
{
	Task<Result<CustomerModel>> ExecuteAsync(int id, CancellationToken cancellationToken = default);
}

public sealed class GetCustomerQuery(DatabaseContext databaseContext) : IGetCustomerQuery
{
	public async Task<Result<CustomerModel>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
	{
		var findEntity = await CustomerQueries.FindQuery(databaseContext, id);
		return findEntity is null
			? Result<CustomerModel>.Failure("Not Found")
			: Result<CustomerModel>.Success(findEntity.Map());
	}
}