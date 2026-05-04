namespace Core.Order.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using Core.Helpers;
using DataAccess;

public sealed class CreateOrderCommand : ICommand<Result<OrderDTO>>
{
    public OrderDTO Data { get; set; }
}

public sealed class CreateOrderCommandHandler(DatabaseContext databaseContext) : ICommandHandler<CreateOrderCommand, Result<OrderDTO>>
{
    public async Task<Result<OrderDTO>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Data.ToEntity();
        databaseContext.Orders.Add(entity);
        request.Data.Id = entity.Id;

        return await CoreHelper<OrderDTO>.Outcome(databaseContext, cancellationToken, request.Data, "Error creating a Order");
    }
}
