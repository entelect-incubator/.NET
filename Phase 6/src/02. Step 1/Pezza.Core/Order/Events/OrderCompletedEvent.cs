namespace Pezza.Core.Order.Events
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Core.Notify.Commands;
    using Pezza.DataAccess.Contracts;

    public class OrderCompletedEvent : INotification
    {
        public OrderDTO CompletedOrder { get; set; }

        public class ArrivalNotificationEventHandler : INotificationHandler<OrderCompletedEvent>
        {
            private readonly IDataAccess<Common.Entities.Notify> dataAccess;
            private readonly IMediator mediator;
            private readonly string rootFolder;

            public ArrivalNotificationEventHandler(IMediator mediator)
            {
                this.mediator = mediator;
            }

            public async Task Handle(OrderCompletedEvent notification, CancellationToken cancellationToken)
            {
                var result = await this.mediator.Publish(new CreateNotifyCommand
                {
                    Data = new NotifyDTO
                    {
                        CustomerId = notification.CompletedOrder.CustomerId,
                        Email = notification.CompletedOrder.Customer.Email,
                        DateSent = DateTime.Now,
                        Sent = true
                    }
                }), cancellationToken);

            }
        }
    }
}
