namespace Core.Order.Events;

using System.Text;
using Common.Entities;
using Common.Models.Order;
using DataAccess;

public class OrderEvent : INotification
{
	public required OrderModel Data { get; set; }
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

		databaseContext.Notifies.Add(new Notify
		{
			CustomerId = notification.Data.Customer.Id,
			Customer = new Customer
			{
				Id = notification.Data.Customer.Id,
				Name = notification.Data.Customer.Name,
				Email = notification.Data.Customer.Email,
				Address = notification.Data.Customer.Address,
				Cellphone = notification.Data.Customer.Cellphone,
				DateCreated = notification.Data.Customer.DateCreated
			},
			CustomerEmail = notification.Data?.Customer?.Email,
			DateSent = null,
			EmailContent = html,
			Sent = false
		});
		await databaseContext.SaveChangesAsync(cancellationToken);
	}
}