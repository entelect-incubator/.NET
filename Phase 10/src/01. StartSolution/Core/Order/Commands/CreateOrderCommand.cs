namespace Core.Order.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Entities;
using Core.Helpers;
using DataAccess;

public sealed class CreateOrderCommand : ICommand<Result<OrderDTO>>
{
    public OrderDTO Data { get; set; }
}

public sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Result<OrderDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public CreateOrderCommandHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<OrderDTO>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = this.mapper.Map<Order>(request.Data);
        this.databaseContext.Orders.Add(entity);

        return await CoreHelper<OrderDTO>.Outcome(this.databaseContext, this.mapper, cancellationToken, entity, "Error creating a Order");
    }
}
