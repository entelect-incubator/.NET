<img align="left" width="116" height="116" src="../Assets/logo.png" />

# &nbsp;**E List - Phase 6 - Step 2** [![.NET - Phase 6 - Step 2](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step2.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step2.yml)

<br/><br/>

## **Events**

Mediatr allows you to publish domain events when a command is handled. This applies the SOLID principle in seperating domain events from commands. In this example, we will be sending out an email to the customer to notify them that their pizza is ready for collection. We achieve this by creating an event that we publish with MediatR when the command for updating an order to completed is handled.

The following material is valuable in getting a better understanding of these patterns:

-   [Domain Event Pattern](https://microservices.io/patterns/data/domain-event.html)
-   [Immediate Domain Event Salvation with MediatR](https://ardalis.com/immediate-domain-event-salvation-with-mediatr/)

Inside Todos in Core Project create a folder called Events and inside create EmailEvent.cs. Here we will call the EmailService.cs created in the previous step.

EmailEvent.cs

```cs
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
```

ExpiredTodoCommand.cs

```cs
namespace Core.Todos.Commands;

using Core.Todos.Events;

public class ExpiredTodoCommand : IRequest<Result>
{
	public required string ToEmail { get; set; }
}

public class ExpiredTodoCommandHandler(IMediator mediator) : IRequestHandler<ExpiredTodoCommand, Result>
{
	public async Task<Result> Handle(ExpiredTodoCommand request, CancellationToken cancellationToken)
	{
		await mediator.Publish(new EmailEvent { ToEmail = request.ToEmail, SessionId = request.SessionId }, cancellationToken);

		return Result.Success();
	}
}
```

Fluentvalidation

ExpiredTodoCommandValidator.cs

```cs
namespace Core.Todos.Commands;

public class ExpiredTodoCommandValidator : AbstractValidator<ExpiredTodoCommand>
{
	public ExpiredTodoCommandValidator()
	{
		this.RuleFor(x => x.ToEmail).NotEmpty().WithMessage("Email is required.").EmailAddress();
	}
}
```

## **Step 3 - Background Job Scheduler**

Move to Step 3 [Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%206/Step%203)
