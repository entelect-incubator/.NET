<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 6 - Step 2** [![.NET Core - Phase 6 - Step 2](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step2.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step2.yml)

<br/><br/>

## **Events**

Mediatr allows you to publish domain events when a command is handled. This applies the SOLID principle in seperating domain events from commands. In this example, we will be sending out an email to the customer that there pizza is ready for collection. We achieve this by creating an event that we publish with MediatR when the command for updating an order to completed is handled.

The following material is valuable in getting a better understanding of these patterns:
- [Domain Event Pattern](https://microservices.io/patterns/data/domain-event.html)
- [Immediate Domain Event Salvation with MediatR](https://ardalis.com/immediate-domain-event-salvation-with-mediatr/)

In Pezza.Core create a new folder in Order called Events. Inside of Events create an Event called OrderCompletedEvent.cs. Here we will call the EmailService created in the previous step as well as insert a Notify record in the database.

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
                    Data = new NotifyDTO
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
```

## **Update the UpdateOrderCommand**

We want to trigger the event if the DTO Completed property that gets send is true.

```cs
namespace Pezza.Core.Order.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.Core.Order.Events;
    using Pezza.DataAccess.Contracts;

    public class UpdateOrderCommand : IRequest<Result<OrderDTO>>
    {
        public OrderDTO Data { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result<OrderDTO>>
    {
        private readonly IDataAccess<OrderDTO> dataAccess;

        private readonly IMediator mediator;

        public UpdateOrderCommandHandler(IDataAccess<OrderDTO> dataAccess, IMediator mediator)
            => (this.dataAccess, this.mediator) = (dataAccess, mediator);

        public async Task<Result<OrderDTO>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAccess.UpdateAsync(request.Data);
            if (request.Data.Completed.HasValue)
            {
                await this.mediator.Publish(new OrderCompletedEvent { CompletedOrder = outcome }, cancellationToken);
            }

            return (outcome != null) ? Result<OrderDTO>.Success(outcome) : Result<OrderDTO>.Failure("Error updating a Order");
        }
    }
}
```

## **Step 3 - Background Job Scheduler**

Move to Step 3
[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%206/Step%203)