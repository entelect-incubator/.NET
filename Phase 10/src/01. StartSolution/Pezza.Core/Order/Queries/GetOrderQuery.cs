namespace Core.Order.Queries;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Pezza.Pezza.Common.DTO;
using Pezza.Pezza.Common.Models;
using DataAccess;

public sealed class GetOrderQuery : ICommand<Result<OrderDTO>>
{
    public int Id { get; set; }
}

public sealed class GetOrderQueryHandler : ICommandHandler<GetOrderQuery, Result<OrderDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetOrderQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<OrderDTO>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var result = this.mapper.Map<OrderDTO>(await this.databaseContext.Orders.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken));
        return Result<OrderDTO>.Success(result);
    }
}

