namespace Core.Order.Queries;

using Common.Models.Order;

public sealed class GetOrdersQuery : IQuery<Result<IEnumerable<OrderModel>>>
{
	public int CustomerId { get; set; }
}

public sealed class GetOrdersQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetOrdersQuery, Result<IEnumerable<OrderModel>>>
{
	public async Task<Result<IEnumerable<OrderModel>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
	{
		var entities = databaseContext.Orders
			.Select(x => x)
			.AsNoTracking()
			.FilterByCustomerId(request.CustomerId)
			.OrderBy("DateCreated desc");

		var count = await entities.CountAsync(cancellationToken);
		var paged = await entities.ToListAsync(cancellationToken);

		return Result<IEnumerable<OrderModel>>.Success(paged.Map(), count);
	}
}