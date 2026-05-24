namespace Core.Order.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using Core.Delivery;
using Core.Delivery.Models;
using Core.Helpers;
using DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public sealed class CreateOrderCommand : ICommand<Result<OrderDTO>>
{
    public OrderDTO Data { get; set; }
}

public sealed class CreateOrderCommandHandler(
    DatabaseContext databaseContext,
    IDeliveryService deliveryService,
    IConfiguration configuration,
    ILogger<CreateOrderCommandHandler> logger) : ICommandHandler<CreateOrderCommand, Result<OrderDTO>>
{
    public async Task<Result<OrderDTO>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Data.ToEntity();
        databaseContext.Orders.Add(entity);
        var outcome = await CoreHelper<OrderDTO>.Outcome(databaseContext, cancellationToken, request.Data, "Error creating a Order");

        if (outcome.Succeeded)
        {
            request.Data.Id = entity.Id;
            var webhookUrl = configuration["DeliveryService:WebhookCallbackUrl"] ?? string.Empty;
            var deliveryRequest = new CreateDeliveryRequest
            {
                OrderId = entity.Id,
                CustomerName = entity.Customer?.Name ?? "Customer",
                DeliveryAddress = entity.Customer?.Address ?? string.Empty,
                WebhookCallbackUrl = webhookUrl,
            };
            var deliveryResponse = await deliveryService.CreateDeliveryAsync(deliveryRequest, cancellationToken);

            if (deliveryResponse is not null)
            {
                entity.DeliveryId = deliveryResponse.DeliveryId;
                entity.DeliveryStatus = deliveryResponse.Status;
                await databaseContext.SaveChangesAsync(cancellationToken);

                outcome.Data.DeliveryId = entity.DeliveryId;
                outcome.Data.DeliveryStatus = entity.DeliveryStatus;

                logger.LogInformation(
                    "Order {OrderId} linked to delivery {DeliveryId} with status {Status}",
                    entity.Id,
                    entity.DeliveryId,
                    entity.DeliveryStatus);
            }
            else
            {
                logger.LogWarning("Delivery service unavailable for Order {OrderId}. Order created without delivery tracking.", entity.Id);
            }
        }

        return outcome;
    }
}
