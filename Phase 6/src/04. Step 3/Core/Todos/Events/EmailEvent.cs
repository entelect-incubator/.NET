namespace Core.Todos.Events;

using System.Text;
using Core.Email;

public class EmailEvent : INotification
{
	public string ToEmail { get; set; }
}

public class EmailEventHandler(DatabaseContext databaseContext) : INotificationHandler<EmailEvent>
{
	async Task INotificationHandler<EmailEvent>.Handle(EmailEvent notification, CancellationToken cancellationToken)
	{
		var path = AppDomain.CurrentDomain.BaseDirectory + "\\Email\\Templates\\TodoEmail.html";
		var html = File.ReadAllText(path);

		var tasks = await databaseContext.Todos
			.Where(x => x.IsCompleted == false && x.DateCreated >= DateTime.UtcNow)
			.AsNoTracking()
			.ToListAsync(cancellationToken);

		var content = new StringBuilder();
		foreach (var task in tasks.Where(x => x.DateCreated is not null))
		{
			content.Append($"<tr><td>{task.Task}</td><td>{task.DateCreated.Value:yyyy-MM-dd}</td></tr>");
		}

		html = html.Replace("<%ITEMS%>", content.ToString());
		var emailService = new EmailService
		{
			ToEmail = notification.ToEmail,
			HtmlContent = html
		};

		var send = await emailService.SendEmail();
	}
}
