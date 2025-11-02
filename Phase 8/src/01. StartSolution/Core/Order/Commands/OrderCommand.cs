namespace Core.Order.Commands;

using Common.Models.Order;

public sealed class OrderCommand : ICommand<Result>
{
	public required OrderModel Data { get; set; }
}

public sealed class OrderCommandHandler : ICommandHandler<OrderCommand, Result>
{
	public async Task<Result> Handle(OrderCommand request, CancellationToken cancellationToken)
	{
		if(request.Data is null)
		{
			return Result.Failure("Error");
		}

		return Result.Success();
	}
}