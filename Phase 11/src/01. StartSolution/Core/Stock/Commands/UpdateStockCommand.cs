namespace Core.Stock.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using Core.Helpers;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class UpdateStockCommand : ICommand<Result<PizzaModel>>
{
    public PizzaModel Data { get; set; }
}

public sealed class UpdateStockCommandHandler(DatabaseContext databaseContext) : ICommandHandler<UpdateStockCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;
        var findEntity = await databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
        if (findEntity is null)
        {
            return null;
        }

        findEntity.Name = !string.IsNullOrEmpty(dto.Name) ? dto.Name : findEntity.Name;
        findEntity.UnitOfMeasure = !string.IsNullOrEmpty(dto.UnitOfMeasure) ? dto.UnitOfMeasure : findEntity.UnitOfMeasure;
        findEntity.ValueOfMeasure = dto.ValueOfMeasure ?? findEntity.ValueOfMeasure;
        findEntity.Quantity = dto.Quantity ?? findEntity.Quantity;
        findEntity.ExpiryDate = dto.ExpiryDate ?? findEntity.ExpiryDate;
        findEntity.Comment = dto.Comment;

        databaseContext.Stocks.Update(findEntity);

        return await CoreHelper<PizzaModel>.Outcome(databaseContext, cancellationToken, findEntity.ToDto(), "Error updating pizza");
    }
}