namespace Core.Stock.Commands;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Common.DTO;
using Common.Entities;
using Common.Models;
using Core.Helpers;
using DataAccess;

public class CreateStockCommand : IRequest<Result<PizzaModel>>
{
    public PizzaModel Data { get; set; }
}

public class CreateStockCommandHandler : IRequestHandler<CreateStockCommand, Result<PizzaModel>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public CreateStockCommandHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<PizzaModel>> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        var entity = this.mapper.Map<Stock>(request.Data);
        this.databaseContext.Stocks.Add(entity);

        return await CoreHelper<PizzaModel>.Outcome(this.databaseContext, this.mapper, cancellationToken, entity, "Error creating pizza");
    }
}
