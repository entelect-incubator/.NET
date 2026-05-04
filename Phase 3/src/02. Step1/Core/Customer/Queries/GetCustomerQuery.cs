namespace Core.Customer.Queries;

using Common.Models.Customer;

public record GetCustomer(int Id) : IQuery<Result<CustomerModel>>
{
	public Task<Result<CustomerModel>> ExecuteAsync(Dispatcher dispatcher, CancellationToken ct = default)
		=> dispatcher.Query<GetCustomer, Result<CustomerModel>>(this, ct);
};

public sealed class GetCustomerHandler(DatabaseContext databaseContext) : IQueryHandler<GetCustomer, Result<CustomerModel>>
{
	public async Task<Result<CustomerModel>> Handle(GetCustomer query, CancellationToken cancellationToken = default)
	{
		var findEntity = await CustomerQueries.FindQuery(databaseContext, query.Id);
		return findEntity is null
			? Result<CustomerModel>.Failure("Not Found")
			: Result<CustomerModel>.Success(findEntity.Map());
	}
}