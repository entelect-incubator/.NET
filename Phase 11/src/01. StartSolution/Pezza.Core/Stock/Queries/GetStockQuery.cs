namespace Core.Stock.Queries;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Pezza.Pezza.Common.DTO;
using Pezza.Pezza.Common.Models;
using DataAccess;

public sealed class GetStockQuery : ICommand<Result<PizzaModel>>
{
    public int Id { get; set; }
}

public sealed class GetStockQueryHandler : ICommandHandler<GetStockQuery, Result<PizzaModel>>
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

