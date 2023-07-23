namespace Core.Customer.Queries;

public class GetCustomerQuery : IRequest<Result<CustomerModel>>
{
	public int Id { get; set; }
}

public class GetCustomerQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetCustomerQuery, Result<CustomerModel>>
{
	public async Task<Result<CustomerModel>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
	{
		var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == id));
		var entity = await query(databaseContext, request.Id);
		if(entity == null)
		{
			return Result<CustomerModel>.Failure("Not Found");
		}

		return Result<CustomerModel>.Success(entity.Map());
	}
}