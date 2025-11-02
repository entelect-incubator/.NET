namespace Core.Order.Commands;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.DTO;
using Common.Models;
using Core.Helpers;
using DataAccess;

public sealed class UpdateOrderCommand : ICommand<Result<OrderDTO>>
{
    public OrderDTO Data { get; set; }
}

public sealed class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand, Result<OrderDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public UpdateOrderCommandHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<OrderDTO>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;
        var findEntity = await this.databaseContext.Orders.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
        if (findEntity is null)
        {
            return null;
        }

        findEntity.Completed = dto.Completed ?? findEntity.Completed;
        findEntity.RestaurantId = dto.RestaurantId ?? findEntity.RestaurantId;
        findEntity.CustomerId = dto.CustomerId ?? findEntity.CustomerId;
        findEntity.Amount = dto.Amount ?? findEntity.Amount;

        this.databaseContext.Orders.Update(findEntity);

        return await CoreHelper<OrderDTO>.Outcome(this.databaseContext, this.mapper, cancellationToken, findEntity, "Error updating order");
    }
}