<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 6 - Step 2**

<br/><br/>

## **Events**

[Domain Event Pattern](https://microservices.io/patterns/data/domain-event.html)

Mediatr allows you to publish domain events after a COmmand has happend. This applies the SOLID principle in seprating Domain Events from Commands. For example after someone has viewed a Customer record you might want to log an audit of that person. Or as the example we will be looking at is, sending out a notification to the customer that there pizza is ready for collection. 

[Read More](https://ardalis.com/immediate-domain-event-salvation-with-mediatr/)

In Pezza.Core create a new folder in Order called Events. Inside of Events create a Event called OrderCompletedEvent.cs. Here we will call the EmailService created in the previous step as well as insert a Notify record in the database.

```cs
namespace Pezza.Core.Order.Events
{
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

        public class ArrivalNotificationEventHandler : INotificationHandler<OrderCompletedEvent>
        {
            private readonly IMediator mediator;

            public ArrivalNotificationEventHandler(IMediator mediator) => this.mediator = mediator;

            public async Task Handle(OrderCompletedEvent notification, CancellationToken cancellationToken)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "\\Email\\Templates\\OrderCompleted.html";
                var html = File.ReadAllText(path);

                html = html.Replace("<%% ORDER %%>", notification.CompletedOrder.Id.ToString());
                var emailService = new EmailService
                {
                    Customer = notification.CompletedOrder?.Customer,
                    HtmlContent = html
                };

                var send = await emailService.SendEmail();

                var customer = notification.CompletedOrder?.Customer;
                var notify = await this.mediator.Send(new CreateNotifyCommand
                {
                    Data = new Common.DTO.NotifyDTO
                    {
                        CustomerId = customer.Id,
                        DateSent = DateTime.Now,
                        Email = customer.Email,
                        Sent = send.Succeeded,
                        Retry = 0
                    }
                });
            }
        }
    }
}

## **Update the UpdateOrderCommand**

We want to trigger the event if the DTO Completed property that gets send is true.

```cs
public async Task<Result<OrderDTO>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
{
    var outcome = await this.dataAcess.UpdateAsync(request.Data);
    if (request.Data.Completed.HasValue)
    {
        await this.mediator.Publish(new OrderCompletedEvent { CompletedOrder = outcome }, cancellationToken);
    }

    return (outcome != null) ? Result<OrderDTO>.Success(outcome) : Result<OrderDTO>.Failure("Error updating a Order");
}
```

## **Phase 7 - Create UI's**

Move to Phase 7
[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%207)