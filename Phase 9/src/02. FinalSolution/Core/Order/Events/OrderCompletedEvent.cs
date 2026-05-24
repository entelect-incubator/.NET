namespace Core.Order.Events;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Core.Email;
using Core.Notify.Commands;
using Utilities.CQRS;

public class OrderCompletedEvent : INotification
{
    public OrderDTO CompletedOrder { get; set; }
}

public class OrderCompletedEventHandler : INotificationHandler<OrderCompletedEvent>
{
    private readonly Dispatcher dispatcher;

    public OrderCompletedEventHandler(Dispatcher dispatcher) => this.dispatcher = dispatcher;

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
        await this.dispatcher.Send<CreateNotifyCommand, Result<NotifyDTO>>(
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
