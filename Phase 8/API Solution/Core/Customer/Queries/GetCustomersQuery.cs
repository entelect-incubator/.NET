namespace Core.Customer.Queries;

using Common.Models.Customer;
using Core;
using DataAccess.Filters;

public sealed class GetCustomersQuery : IQuery<Result<IEnumerable<CustomerModel>>>
{
	public SearchCustomerModel Data { get; set; } = new();
}

public sealed class GetCustomersQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetCustomersQuery, Result<IEnumerable<CustomerModel>>>
{
	public async Task<Result<IEnumerable<CustomerModel>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
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
		var paged = await Common.Extensions.Extensions.ApplyPaging(entities, entity.PagingArgs).ToListAsync(cancellationToken);

		return Result<IEnumerable<CustomerModel>>.Success(paged.Map(), count);
	}
}