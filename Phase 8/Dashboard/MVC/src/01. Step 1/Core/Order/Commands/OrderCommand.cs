namespace Core.Order.Commands;

using Common.Models.Order;
using Core.Order.Events;
using DataAccess;

public class OrderCommand : IRequest<Result>
{
	public required OrderModel Data { get; set; }
}

public class OrderCommandHandler(IMediator mediator, DatabaseContext databaseContext) : IRequestHandler<OrderCommand, Result>
{
	public async Task<Result> Handle(OrderCommand request, CancellationToken cancellationToken)
	{
		if(request.Data == null)
		{
			return Result.Failure("Error");
		}

		var model = request.Data;
		var customer = await databaseContext.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.CustomerId, cancellationToken);
		model.Customer = customer.Map();
		await mediator.Publish(new OrderEvent { Data = model }, cancellationToken);

		return Result.Success();
	}
}