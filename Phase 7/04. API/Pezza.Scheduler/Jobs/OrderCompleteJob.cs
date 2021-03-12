namespace Pezza.Scheduler.Jobs
{
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
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
                SearchModel = new NotifyDTO
                {
                    Sent = false,
                    PagingArgs = PagingArgs.Default
                }
            });
            if (result.Succeeded)
            {
                foreach (var notification in result.Data)
                {
                    var emailService = new EmailService
                    {
                        Customer = notification.Customer,
                        HtmlContent = notification.Email
                    };

                    var send = await emailService.SendEmail();
                }
            }
        }
    }
}
