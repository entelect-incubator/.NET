namespace Core.Stock.Commands;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.DTO;
using Common.Models;
using Core.Helpers;
using DataAccess;

public sealed class UpdateStockCommand : ICommand<Result<PizzaModel>>
{
    public PizzaModel Data { get; set; }
}

public sealed class UpdateStockCommandHandler : ICommandHandler<UpdateStockCommand, Result<PizzaModel>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public UpdateStockCommandHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<PizzaModel>> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;
        var findEntity = await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
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

        var outcome = this.databaseContext.Stocks.Update(findEntity);

        return await CoreHelper<PizzaModel>.Outcome(this.databaseContext, this.mapper, cancellationToken, findEntity, "Error updating pizza");
    }
}