namespace Core.Order.Commands;

using Common.Models.Order;
using Core.Order.Events;

public sealed class OrderCommand : ICommand<Result>
{
	public required OrderModel Data { get; set; }
}

public sealed class OrderCommandHandler(IMediator mediator) : ICommandHandler<OrderCommand, Result>
{
	public async Task<Result> Handle(OrderCommand request, CancellationToken cancellationToken)
	{
		if(request.Data is null)
		{
			return Result.Failure("Error");
		}

		await mediator.Publish(new OrderEvent { Data = request.Data }, cancellationToken);

		return Result.Success();
	}
}