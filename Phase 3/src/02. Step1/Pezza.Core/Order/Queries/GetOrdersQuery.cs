namespace Pezza.Core.Order.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pezza.Common.DTO;
using Pezza.Common.Models;
using Pezza.DataAccess;

public class GetOrdersQuery : IRequest<ListResult<OrderDTO>>
{
}

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ListResult<OrderDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetOrdersQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<ListResult<OrderDTO>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var entities = this.databaseContext.Orders.Select(x => x).AsNoTracking();

        var count = entities.Count();
        var paged = this.mapper.Map<List<OrderDTO>>(await entities.ToListAsync(cancellationToken));

        return ListResult<OrderDTO>.Success(paged, count);
    }
}
