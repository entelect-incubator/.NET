namespace Core.Order.Commands;

using Common.Models.Order;
using Core.Order.Events;

public class OrderCommand : IRequest<Result>
{
	public required OrderModel Data { get; set; }
}

public class OrderCommandHandler(IMediator mediator) : IRequestHandler<OrderCommand, Result>
{
	public async Task<Result> Handle(OrderCommand request, CancellationToken cancellationToken)
	{
		if(request.Data == null)
		{
			return Result.Failure("Error");
		}

		await mediator.Publish(new OrderEvent { Data = request.Data }, cancellationToken);

		return Result.Success();
	}
}