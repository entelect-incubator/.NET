namespace Core.Order.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using DataAccess;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Result<OrderDTO>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var result = this.mapper.Map<OrderDTO>(await this.databaseContext.Orders.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken));
        return Result<OrderDTO>.Success(result);
    }
}
