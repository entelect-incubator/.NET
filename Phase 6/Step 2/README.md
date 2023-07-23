<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 6 - Step 2** [![.NET - Phase 6 - Step 2](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step2.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step2.yml)

<br/><br/>

## **Events**

Mediatr allows you to publish domain events when a command is handled. This applies the SOLID principle in seperating domain events from commands. In this example, we will be sending out an email to the customer to notify them that their pizza is ready for collection. We achieve this by creating an event that we publish with MediatR when the command for updating an order to completed is handled.

The following material is valuable in getting a better understanding of these patterns:
- [Domain Event Pattern](https://microservices.io/patterns/data/domain-event.html)
- [Immediate Domain Event Salvation with MediatR](https://ardalis.com/immediate-domain-event-salvation-with-mediatr/)

Create a new folder Common Models folder called Order and inside create a new Order Model.

OrderModel.cs

```cs
namespace Common.Models.Order;

public class OrderModel
{
	public required CustomerModel Customer { get; set; }

	public required List<PizzaModel> Pizzas { get; set; }
}
```

In Core create a new folder Order. Inside of Order create a folder called Commands and inside a Command called OrderCommand.cs. Inside Order create a folder called Events and inside create EmailEvent.cs. Here we will call the EmailService.cs created in the previous step.

OrderCommand.cs

```cs
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
```

OrderEvent.cs

```cs
namespace Core.Order.Events;

using System.Text;
using Common.Models.Order;
using Core.Email;

public class OrderEvent : INotification
{
	public OrderModel Data { get; set; }
}

public class OrderEventHandler(DatabaseContext databaseContext) : INotificationHandler<OrderEvent>
{
	async Task INotificationHandler<OrderEvent>.Handle(OrderEvent notification, CancellationToken cancellationToken)
	{
		var path = AppDomain.CurrentDomain.BaseDirectory + "\\Email\\Templates\\OrderCompleted.html";
		var html = File.ReadAllText(path);

		html = html.Replace("%name%", Convert.ToString(notification.Data.Customer.Name));

		var pizzasContent = new StringBuilder();
		foreach (var pizza in notification.Data.Pizzas)
		{
			pizzasContent.AppendLine($"<strong>{pizza.Name}</strong> - {pizza.Description}<br/>");
		}

		html = html.Replace("%pizzas%", pizzasContent.ToString());
		var emailService = new EmailService
		{
			Customer = notification.Data.Customer,
			HtmlContent = html
		};

		var send = await emailService.SendEmail();
	}
}
```

## **Step 3 - Background Job Scheduler**

Move to Step 3
[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%206/Step%203)