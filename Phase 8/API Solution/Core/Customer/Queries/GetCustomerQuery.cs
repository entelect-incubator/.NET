namespace Core.Customer.Queries;

using Common.Models.Customer;
using Core;

public sealed class GetCustomerQuery : IQuery<Result<CustomerModel>>
{
	public int Id { get; set; }
}

public sealed class GetCustomerQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetCustomerQuery, Result<CustomerModel>>
{
	public async Task<Result<CustomerModel>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
	{
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == id));
		var entity = await query(databaseContext, request.Id);
		return entity is null ? Result<CustomerModel>.Failure("Not Found") : Result<CustomerModel>.Success(entity.Map());
	}
}