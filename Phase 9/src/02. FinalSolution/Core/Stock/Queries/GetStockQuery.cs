namespace Core.Stock.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class GetStockQuery : IQuery<Result<PizzaModel>>
{
    public int Id { get; set; }
}

public sealed class GetStockQueryHandler : IQueryHandler<GetStockQuery, Result<PizzaModel>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetStockQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<PizzaModel>> Handle(GetStockQuery request, CancellationToken cancellationToken)
    {
        var result = this.mapper.Map<PizzaModel>(await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken));
        return Result<PizzaModel>.Success(result);
    }
}
