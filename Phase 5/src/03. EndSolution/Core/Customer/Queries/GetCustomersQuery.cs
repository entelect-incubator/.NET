namespace Core.Customer.Queries;

using Common.Models.Customer;
using DataAccess.Filters;

public sealed class GetCustomersQuery : LiteBus.Queries.Abstractions.IQuery<ListResult<CustomerModel>>
{
	public SearchCustomerModel Data { get; set; }
}

public sealed class GetCustomersQueryHandler(DatabaseContext databaseContext) : LiteBus.Queries.Abstractions.IQueryHandler<GetCustomersQuery, ListResult<CustomerModel>>
{
	public async Task<ListResult<CustomerModel>> HandleAsync(GetCustomersQuery request, CancellationToken cancellationToken)
	{
		var entity = request.Data;
		if (string.IsNullOrEmpty(entity.OrderBy))
		{
			entity.OrderBy = "DateCreated desc";
		}

		var entities = databaseContext.Customers
			.Select(x => x)
			.AsNoTracking()
			.FilterByName(entity.Name)
			.FilterByAddress(entity.Address)
			.FilterByPhone(entity.Cellphone)
			.FilterByEmail(entity.Email)
			.OrderBy(entity.OrderBy);

		var count = await entities.CountAsync(cancellationToken);
		var paged = await entities.ApplyPaging(entity.PagingArgs).ToListAsync(cancellationToken);

		return ListResult<CustomerModel>.Success(paged.Map(), count);
	}
}