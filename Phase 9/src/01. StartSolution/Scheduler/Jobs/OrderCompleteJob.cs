namespace Scheduler.Jobs;

using System.Threading.Tasks;
using Common;
using Common.Mappers;
using Core.Email;
using Core.Notify.Commands;
using Core.Pizza.Queries;
using MediatR;

public interface IOrderCompleteJob
{
	Task SendNotificationAsync();
}

public sealed class OrderCompleteJob(IMediator mediator) : IOrderCompleteJob
{
	public async Task SendNotificationAsync()
	{
		var notifiesResult = await mediator.Send(new GetNotifiesQuery());

		if (notifiesResult.Succeeded && notifiesResult.Data.Count != 0)
		{
			foreach (var notification in notifiesResult.Data)
			{
				var emailService = new EmailService
				{
					Customer = notification.Customer.Map(),
					HtmlContent = notification.EmailContent
				};
				var emailResult = await emailService.SendEmail();
				if (emailResult.Succeeded)
				{
					notification.Sent = true;
					var updateNotifyResult = await mediator.Send(new UpdateNotifyCommand
					{
						Id = notification.Id,
						Sent = true
					});
					if (!updateNotifyResult.Succeeded)
					{
						Logging.LogException(new Exception(string.Join("", updateNotifyResult.Errors)));
					}
				}
			}
		}
	}
}