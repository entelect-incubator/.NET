namespace Scheduler.Jobs;

using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Mappers;
using Core.Email;
using Core.Notify.Commands;
using Core.Pizza.Queries;
using Common.Entities;
using Dispatch;
using Utilities.Results;

public interface IOrderCompleteJob
{
	Task SendNotificationAsync();
}

public sealed class OrderCompleteJob(Dispatcher dispatcher) : IOrderCompleteJob
{
	public async Task SendNotificationAsync()
	{
		var notifiesResult = await dispatcher.Query<GetNotifiesQuery, Result<IEnumerable<Notify>>>(new GetNotifiesQuery());

		if (notifiesResult.Succeeded && notifiesResult.Data?.Any() == true)
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
					var updateNotifyResult = await dispatcher.Send<UpdateNotifyCommand, Result>(new UpdateNotifyCommand
					{
						Id = notification.Id,
						Sent = true
					}, CancellationToken.None);
					if (!updateNotifyResult.Succeeded)
					{
						Logging.LogException(new Exception(string.Join("", updateNotifyResult.Errors)));
					}
				}
			}
		}
	}
}