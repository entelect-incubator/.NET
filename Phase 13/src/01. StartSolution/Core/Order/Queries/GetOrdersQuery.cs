namespace Core.Order.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Extensions;
using Common.Filters;
using Common.Mapper;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class GetOrdersQuery : IQuery<Result<IEnumerable<OrderDTO>>>
{
    public OrderDTO Data { get; set; }
}

public sealed class GetOrdersQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetOrdersQuery, Result<IEnumerable<OrderDTO>>>
{
    public async Task<Result<IEnumerable<OrderDTO>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Data;
        var entities = databaseContext.Orders
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Product)
            .Include(x => x.Restaurant)
            .Include(x => x.Customer)
            .AsNoTracking()
            .FilterByCustomerId(dto.CustomerId)
            .FilterByRestaurantId(dto.RestaurantId)
            .FilterByAmount(dto.Amount)
            .FilterByCompleted(dto.Completed);

        var count = await entities.CountAsync(cancellationToken);
        var paged = (await entities.ApplyPaging(dto.PagingArgs).OrderBy(dto.OrderBy).ToListAsync(cancellationToken)).Select(x => x.ToDto()).ToList();

        return Result<IEnumerable<OrderDTO>>.Success(paged, count);
    }
}
