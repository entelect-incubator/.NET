namespace Core.Stock.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class GetStockQuery : IQuery<Result<PizzaModel>>
{
    public int Id { get; set; }
}

public sealed class GetStockQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetStockQuery, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> Handle(GetStockQuery request, CancellationToken cancellationToken)
    {
        var result = (await databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken))?.ToDto();
        return Result<PizzaModel>.Success(result);
    }
}
