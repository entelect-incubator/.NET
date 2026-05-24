namespace Core.Stock.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using Core.Helpers;
using DataAccess;

public sealed class CreateStockCommand : ICommand<Result<PizzaModel>>
{
    public PizzaModel Data { get; set; }
}

public sealed class CreateStockCommandHandler(DatabaseContext databaseContext) : ICommandHandler<CreateStockCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Data.ToEntity();
        databaseContext.Stocks.Add(entity);
        var outcome = await CoreHelper<PizzaModel>.Outcome(databaseContext, cancellationToken, request.Data, "Error creating pizza");
        if (outcome.Succeeded)
        {
            request.Data.Id = entity.Id;
        }

        return outcome;
    }
}
