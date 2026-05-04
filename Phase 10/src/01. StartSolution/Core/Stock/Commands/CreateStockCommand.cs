namespace Core.Stock.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Entities;
using Core.Helpers;
using DataAccess;

public sealed class CreateStockCommand : ICommand<Result<PizzaModel>>
{
    public PizzaModel Data { get; set; }
}

public sealed class CreateStockCommandHandler : ICommandHandler<CreateStockCommand, Result<PizzaModel>>
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
