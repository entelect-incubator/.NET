namespace Pezza.Core.Stock.Commands;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Pezza.Common.DTO;
using Pezza.Common.Entities;
using Pezza.Common.Models;
using Pezza.Core.Helpers;
using Pezza.DataAccess;

public class CreateStockCommand : IRequest<Result<StockDTO>>
{
    public StockDTO Data { get; set; }
}

public class CreateStockCommandHandler : IRequestHandler<CreateStockCommand, Result<StockDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public CreateStockCommandHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<StockDTO>> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        var entity = this.mapper.Map<Stock>(request.Data);
        this.databaseContext.Stocks.Add(entity);

        return await CoreHelper<StockDTO>.Outcome(this.databaseContext, this.mapper, cancellationToken, entity, "Error creating stock");
    }
}
