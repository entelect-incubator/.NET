namespace Pezza.Core.Order.Events
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Core.Notify.Commands;
    using Pezza.DataAccess.Contracts;

    public class OrderCompletedEvent : INotification
    {
        public Common.Entities.Order CompletedOrder { get; set; }

        public class ArrivalNotificationEventHandler : INotificationHandler<OrderCompletedEvent>
        {
            private readonly IDataAccess<Common.Entities.Notify> dataAccess;
            private readonly IMediator mediator;
            private readonly string rootFolder;

            public ArrivalNotificationEventHandler( IMediator mediator)
            {
                this.mediator = mediator;
            }

            public async Task Handle(OrderCompletedEvent notification, CancellationToken cancellationToken)
            {
                var path = this.rootFolder + "\\Template\\NotifyArrival.html";
                var html = File.ReadAllText(path);

                html = html.Replace("<%% ORDER %%>", notification.CompletedOrder.Id.ToString());
                var subject = "Guest Arrival";

                

                //2. Send email(s)
                /*var result = await this.notify.SingleGuestArrivalAsync(emails, subject, html, this.rootFolder);
                if (!result.WasSuccessful)
                {
                    email.Sent = false;
                }

                //save email result
                var data = new NotifyDataDTO()
                {
                    CustomerId = findEntity.CustomerId,
                    Email = findEntity.Customer.Email,
                    DateSent = DateTime.Now,
                    Sent = true
                }
                var result = await this.mediator.Publish(new CreateNotifyCommand { Data =  }, cancellationToken);*/
            }
        }
    }
}
