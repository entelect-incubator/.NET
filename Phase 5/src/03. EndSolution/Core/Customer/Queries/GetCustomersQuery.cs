namespace Core.Customer.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Models.Customer;
using DataAccess.Filters;

public sealed class GetCustomersQuery : LiteBus.Queries.Abstractions.IQuery<Result<IEnumerable<CustomerModel>>>
{
	public SearchCustomerModel? Data { get; set; }
}

public sealed class GetCustomersQueryHandler(DatabaseContext databaseContext) : LiteBus.Queries.Abstractions.IQueryHandler<GetCustomersQuery, Result<IEnumerable<CustomerModel>>>
{
	public Task<Result<IEnumerable<CustomerModel>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
		=> this.HandleAsync(request, cancellationToken);

	public async Task<Result<IEnumerable<CustomerModel>>> HandleAsync(GetCustomersQuery request, CancellationToken cancellationToken)
	{
		var search = request.Data;
		if (search is null)
		{
			return Result<IEnumerable<CustomerModel>>.Failure("Search Customer Model empty");
		}

		var entities = databaseContext.Customers
			.Select(x => x)
			.AsNoTracking()
			.FilterByName(search.Name)
			.FilterByAddress(search.Address)
			.FilterByPhone(search.Cellphone)
			.FilterByEmail(search.Email)
			.FilterByDateCreated(search.DateCreated);

		var count = await entities.CountAsync(cancellationToken);
		var items = await entities.ToListAsync(cancellationToken);

		return Result<IEnumerable<CustomerModel>>.Success(items.Map(), count);
	}
}