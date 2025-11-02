namespace Core.Order.Queries;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.DTO;
using Common.Models;
using DataAccess;

public sealed class GetOrderQuery : IQuery<Result<OrderDTO>>
{
    public int Id { get; set; }
}

public sealed class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, Result<OrderDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetOrderQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<OrderDTO>> HandleAsync(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var result = this.mapper.Map<OrderDTO>(await this.databaseContext.Orders.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken));
        return Result<OrderDTO>.Success(result);
    }
}
