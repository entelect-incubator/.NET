namespace Scheduler.Jobs;

using Utilities.CQRS;

using System.Threading.Tasks;
using Common.DTO;
using Common.Models;
using Core.Customer.Queries;
using Core.Email;
using Core.Notify.Commands;
using Core.Notify.Queries;

public class OrderCompleteJob : IOrderCompleteJob
{
    private readonly Dispatcher mediator;

    public OrderCompleteJob(Dispatcher mediator) => this.mediator = mediator;

    public async Task SendNotificationAsync()
    {
        var notifiesResult = await this.Mediator.Send<GetNotifiesQuery, Result<IEnumerable<NotifyDTO>>>(new GetNotifiesQuery
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
                    var customerResult = await this.Mediator.Send<GetCustomerQuery, Result<CustomerDTO>>(new GetCustomerQuery
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
                            var updateNotifyResult = await this.Mediator.Send<UpdateNotifyCommand, Result<NotifyDTO>>(new UpdateNotifyCommand { Data = notification });
                        }
                    }
                }
            }
        }
    }
}
