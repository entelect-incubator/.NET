namespace Scheduler.Jobs;

using System.Threading.Tasks;
using Common.DTO;
using Common.Models;
using Core.Customer.Queries;
using Core.Email;
using Core.Notify.Commands;
using Core.Notify.Queries;
using Utilities.CQRS;

public class OrderCompleteJob : IOrderCompleteJob
{
    private readonly Dispatcher dispatcher;

    public OrderCompleteJob(Dispatcher dispatcher) => this.dispatcher = dispatcher;

    public async Task SendNotificationAsync()
    {
        var notifiesResult = await this.dispatcher.Send(new GetNotifiesQuery
        {
            Data = new NotifyDTO
            {
                Sent = false,
                PagingArgs = PagingArgs.Default
            }
        });

        if (notifiesResult.Succeeded)
        {
            foreach (var notification in notifiesResult.Data)
            {
                if (notification.CustomerId.HasValue)
                {
                    var customerResult = await this.dispatcher.Send(new GetCustomerQuery
                    {
                        Id = notification.CustomerId.Value
                    });

                    if (customerResult.Succeeded)
                    {
                        var emailService = new EmailService
                        {
                            Customer = customerResult.Data,
                            HtmlContent = notification.Email
                        };

                        var emailResult = await emailService.SendEmail();

                        if (emailResult.Succeeded)
                        {
                            notification.Sent = true;
                            var updateNotifyResult = await this.dispatcher.Send(new UpdateNotifyCommand { Data = notification });
                        }
                    }
                }
            }
        }
    }
}
