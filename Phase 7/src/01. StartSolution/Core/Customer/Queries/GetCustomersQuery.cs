namespace Core.Customer.Queries;

using Core;
using DataAccess.Filters;

public sealed class GetCustomersQuery : IQuery<ListResult<CustomerModel>>
{
	public SearchCustomerModel Data { get; set; }
}

public sealed class GetCustomersQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetCustomersQuery, ListResult<CustomerModel>>
{
	public async Task<ListResult<CustomerModel>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
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