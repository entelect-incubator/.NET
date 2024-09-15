namespace Core.Todos.Events;

using System.Text;
using Core.Email;

public class EmailEvent : INotification
{
	public string ToEmail { get; set; }

	public Guid SessionId { get; set; }
}

public class EmailEventHandler(DatabaseContext databaseContext) : INotificationHandler<EmailEvent>
{
	async Task INotificationHandler<EmailEvent>.Handle(EmailEvent notification, CancellationToken cancellationToken)
	{
		var path = AppDomain.CurrentDomain.BaseDirectory + "\\Email\\Templates\\TodoEmail.html";
		var html = File.ReadAllText(path);

		var tasks = await databaseContext.Todos
			.Where(x => x.SessionId == notification.SessionId && x.IsCompleted == false && x.DateCreated >= DateTime.UtcNow)
			.AsNoTracking()
			.ToListAsync(cancellationToken);

		var content = new StringBuilder();
		foreach (var task in tasks.Where(x => x.DateCreated is not null))
		{
			content.Append($"<tr><td>{task.Task}</td><td>{task.DateCreated.Value:yyyy-MM-dd}</td></tr>");
		}

		html = html.Replace("<%ITEMS%>", content.ToString());

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
