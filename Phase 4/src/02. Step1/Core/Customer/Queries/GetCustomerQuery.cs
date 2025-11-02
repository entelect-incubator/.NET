namespace Core.Customer.Queries;

public sealed class GetCustomerQuery : IQuery<Result<CustomerModel>>
{
	public int Id { get; set; }

	public sealed class GetCustomerQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetCustomerQuery, Result<CustomerModel>>
	{
		public async Task<Result<CustomerModel>> HandleAsync(GetCustomerQuery request, CancellationToken cancellationToken)
		{
			var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == id));
			var entity = await query(databaseContext, request.Id);
			if(entity is null)
			{
				return Result<CustomerModel>.Failure("Not Found");
			}

			return Result<CustomerModel>.Success(entity.Map());
		}
	}
}