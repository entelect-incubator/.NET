namespace Core.Order.Queries;

using Common.Models.Order;

public class GetOrdersQuery : IRequest<ListResult<OrderModel>>
{
	public int CustomerId { get; set; }
}

public class GetOrdersQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetOrdersQuery, ListResult<OrderModel>>
{
	public async Task<ListResult<OrderModel>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
	{
		var entities = databaseContext.Orders
			.Select(x => x)
			.AsNoTracking()
			.FilterByCustomerId(request.CustomerId)
			.OrderBy("DateCreated desc");

		var count = entities.Count();
		var paged = await entities.ToListAsync(cancellationToken);

		return ListResult<OrderModel>.Success(paged.Map(), count);
	}
}