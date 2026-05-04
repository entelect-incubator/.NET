namespace Core.Order.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using Core.Helpers;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class UpdateOrderCommand : ICommand<Result<OrderDTO>>
{
    public OrderDTO Data { get; set; }
}

public sealed class UpdateOrderCommandHandler(DatabaseContext databaseContext) : ICommandHandler<UpdateOrderCommand, Result<OrderDTO>>
{
    public async Task<Result<OrderDTO>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;
        var findEntity = await databaseContext.Orders.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
        if (findEntity is null)
        {
            return null;
        }

        findEntity.Completed = dto.Completed ?? findEntity.Completed;
        findEntity.RestaurantId = dto.RestaurantId ?? findEntity.RestaurantId;
        findEntity.CustomerId = dto.CustomerId ?? findEntity.CustomerId;
        findEntity.Amount = dto.Amount ?? findEntity.Amount;

        databaseContext.Orders.Update(findEntity);

        return await CoreHelper<OrderDTO>.Outcome(databaseContext, cancellationToken, findEntity.ToDto(), "Error updating order");
    }
}