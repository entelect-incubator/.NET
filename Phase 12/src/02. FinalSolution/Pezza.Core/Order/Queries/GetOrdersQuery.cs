namespace Core.Order.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.DTO;
using Common.Extensions;
using Common.Filters;
using Common.Models;
using DataAccess;

public sealed class GetOrdersQuery : IQuery<Result<IEnumerable<OrderDTO>>>
{
    public OrderDTO? Data { get; set; }
}

public sealed class GetOrdersQueryHandler(DatabaseContext databaseContext, IMapper mapper)
    : IQueryHandler<GetOrdersQuery, Result<IEnumerable<OrderDTO>>>
{
    public async Task<Result<IEnumerable<OrderDTO>>> HandleAsync(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        if (request.Data == null)
        {
            return Result<IEnumerable<OrderDTO>>.Failure("Order search data is required");
        }

        var dto = request.Data;
        dto.OrderBy ??= "DateCreated desc";

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
        var paged = mapper.Map<List<OrderDTO>>(await entities.ApplyPaging(dto.PagingArgs).OrderBy(dto.OrderBy).ToListAsync(cancellationToken));

        return Result<IEnumerable<OrderDTO>>.Success(paged, count);
    }
}
