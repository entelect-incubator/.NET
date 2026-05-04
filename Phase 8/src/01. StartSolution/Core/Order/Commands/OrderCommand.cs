namespace Core.Order.Commands;

using Common.Models.Order;
using Core;
using Core.Order.Events;

public sealed class OrderCommand : ICommand<Result>
{
	public required OrderModel Data { get; set; }
}

public sealed class OrderCommandHandler(Dispatcher dispatcher) : ICommandHandler<OrderCommand, Result>
{
	public async Task<Result> Handle(OrderCommand request, CancellationToken cancellationToken)
	{
		if (request.Data is null)
		{
			return Result.Failure("Error");
		}

		await dispatcher.Publish(new OrderEvent { Data = request.Data }, cancellationToken);

		return Result.Success();
	}
}