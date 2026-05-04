namespace Scheduler.Jobs;

using System.Threading.Tasks;
using Common;
using Common.Mappers;
using Core.Email;
using Core.Notify.Commands;
using Core.Pizza.Queries;
using Utilities.CQRS;

public interface IOrderCompleteJob
{
	Task SendNotificationAsync();
}

public sealed class OrderCompleteJob(Dispatcher dispatcher) : IOrderCompleteJob
{
	public async Task SendNotificationAsync()
	{
		var notifiesResult = await dispatcher.Send(new GetNotifiesQuery());

		if (!notifiesResult.HasError && notifiesResult.Data?.Count() != 0)
		{
			foreach (var notification in notifiesResult.Data!)
			{
				var emailService = new EmailService
				{
					Customer = notification.Customer.Map(),
					HtmlContent = notification.EmailContent
				};
				var emailResult = await emailService.SendEmail();
				if (!emailResult.HasError)
				{
					notification.Sent = true;
					var updateNotifyResult = await dispatcher.Send(new UpdateNotifyCommand
					{
						Id = notification.Id,
						Sent = true
					});
					if (updateNotifyResult.HasError)
					{
						Logging.LogException(new Exception(string.Join("", updateNotifyResult.Errors)));
					}
				}
			}
		}
	}
}