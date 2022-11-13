namespace Pezza.Core.Order.Events;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Pezza.Common.DTO;
using Pezza.Core.Email;
using Pezza.Core.Notify.Commands;

public class OrderCompletedEvent : INotification
{
    public OrderDTO CompletedOrder { get; set; }
}

public class OrderCompletedEventHandler : INotificationHandler<OrderCompletedEvent>
{
    private readonly IMediator mediator;

    public OrderCompletedEventHandler(IMediator mediator) => this.mediator = mediator;

    public async Task Handle(OrderCompletedEvent notification, CancellationToken cancellationToken)
    {
        var path = AppDomain.CurrentDomain.BaseDirectory + "\\Email\\Templates\\OrderCompleted.html";
        var html = File.ReadAllText(path);

        html = html.Replace("<%% ORDER %%>", Convert.ToString(notification.CompletedOrder.Id));
        var emailService = new EmailService
        {
            Customer = notification.CompletedOrder?.Customer,
            HtmlContent = html
        };

        var send = await emailService.SendEmail();

        var customer = notification.CompletedOrder?.Customer;
        var notify = await this.mediator.Send(
            new CreateNotifyCommand
            {
                Data = new NotifyDTO
                {
                    CustomerId = customer.Id,
                    DateSent = DateTime.Now,
                    Email = customer.Email,
                    Sent = send.Succeeded,
                    Retry = 0
                }
            }, cancellationToken);
    }
}
