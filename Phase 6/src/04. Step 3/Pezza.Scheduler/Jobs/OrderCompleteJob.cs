namespace Pezza.Scheduler.Jobs
{
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.Core.Customer.Queries;
    using Pezza.Core.Email;
    using Pezza.Core.Notify.Queries;

    public class OrderCompleteJob : IOrderCompleteJob
    {
        private IMediator mediator;

        public OrderCompleteJob(IMediator mediator) => this.mediator = mediator;

        public async Task SendNotficationAsync()
        {
            var result = await this.mediator.Send(new GetNotifiesQuery
            {
                dto = new NotifyDTO
                {
                    Sent = false,
                    PagingArgs = PagingArgs.Default
                }
            });
            if (result.Succeeded)
            {
                foreach (var notification in result.Data)
                {
                    if (notification.CustomerId.HasValue)
                    {
                        var customer = await this.mediator.Send(new GetCustomerQuery
                        {
                            Id = notification.CustomerId.Value
                        });

                        if (customer.Succeeded)
                        {
                            var emailService = new EmailService
                            {
                                Customer = customer.Data,
                                HtmlContent = notification.Email
                            };

                            var send = await emailService.SendEmail();
                        }
                    }
                }
            }
        }
    }
}
