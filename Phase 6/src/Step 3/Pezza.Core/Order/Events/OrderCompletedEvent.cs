namespace Pezza.Core.Order.Events
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Core.Customer.Queries;
    using Pezza.Core.Email;
    using Pezza.Core.Notify.Commands;

    public class OrderCompletedEvent : INotification
    {
        public OrderDTO CompletedOrder { get; set; }

        public class ArrivalNotificationEventHandler : INotificationHandler<OrderCompletedEvent>
        {
            private readonly IMediator mediator;

            public ArrivalNotificationEventHandler(IMediator mediator) => this.mediator = mediator;

            public async Task Handle(OrderCompletedEvent notification, CancellationToken cancellationToken)
            {
                try
                {
                    var path = AppDomain.CurrentDomain.BaseDirectory + "\\Email\\Templates\\OrderCompleted.html";
                    var html = File.ReadAllText(path);

                    html = html.Replace("<%% ORDER %%>", notification.CompletedOrder.Id.ToString());
                    if (notification.CompletedOrder.CustomerId.HasValue)
                    {
                        var customer = await this.mediator.Send(new GetCustomerQuery { Id = notification.CompletedOrder.CustomerId.Value });
                        if (customer.Succeeded)
                        {
                            var notify = await this.mediator.Send(new CreateNotifyCommand
                            {
                                Data = new Common.DTO.NotifyDTO
                                {
                                    CustomerId = customer.Data.Id,
                                    DateSent = DateTime.Now,
                                    Email = html,
                                    Sent = false,
                                    Retry = 0
                                }
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Common.Logging.Logging.LogException(e);
                    throw;
                }
            }
        }
    }
}
