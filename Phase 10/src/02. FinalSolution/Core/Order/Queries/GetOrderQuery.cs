namespace Core.Order.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class GetOrderQuery : IQuery<Result<OrderDTO>>
{
    public int Id { get; set; }
}

public sealed class GetOrderQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetOrderQuery, Result<OrderDTO>>
{
    public async Task<Result<OrderDTO>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var result = (await databaseContext.Orders.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken))?.ToDto();
        return Result<OrderDTO>.Success(result);
    }
}
