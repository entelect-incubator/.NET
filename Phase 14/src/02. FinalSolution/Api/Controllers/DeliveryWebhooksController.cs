namespace Api.Controllers;

using System.Threading;
using System.Threading.Tasks;
using Common.Entities;
using Core.Delivery.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("webhooks")]
public partial class DeliveryWebhooksController(
    DatabaseContext databaseContext,
    ILogger<DeliveryWebhooksController> logger) : ControllerBase
{
    /// <summary>
    /// Receive delivery status update webhook from Mock Delivery Service.
    /// </summary>
    /// <param name="payload">Delivery webhook payload.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>200 OK to acknowledge receipt.</returns>
    /// <response code="200">Webhook processed successfully.</response>
    /// <response code="400">Invalid payload.</response>
    [HttpPost("delivery")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> HandleDeliveryWebhook(
        [FromBody] DeliveryWebhookPayload payload,
        CancellationToken cancellationToken = default)
    {
        if (payload is null || string.IsNullOrWhiteSpace(payload.DeliveryId))
        {
            return BadRequest("Invalid webhook payload.");
        }

        LogWebhookReceived(logger, payload.DeliveryId, payload.Status, payload.Timestamp);

        var order = await databaseContext.Orders
            .FirstOrDefaultAsync(o => o.DeliveryId == payload.DeliveryId, cancellationToken);

        if (order is null)
        {
            // Return 200 so the delivery service does not keep retrying an unknown delivery
            LogOrderNotFound(logger, payload.DeliveryId);
            return Ok();
        }

        order.DeliveryStatus = payload.Status;

        if (string.Equals(payload.Status, "Delivered", StringComparison.OrdinalIgnoreCase))
        {
            order.Completed = true;
        }

        await databaseContext.SaveChangesAsync(cancellationToken);

        LogDeliveryStatusUpdated(logger, order.Id, payload.Status);

        return Ok();
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Received delivery webhook. DeliveryId: {DeliveryId}, Status: {Status}, Timestamp: {Timestamp}")]
    private static partial void LogWebhookReceived(ILogger logger, string deliveryId, string status, DateTime timestamp);

    [LoggerMessage(Level = LogLevel.Warning, Message = "No order found for DeliveryId: {DeliveryId}")]
    private static partial void LogOrderNotFound(ILogger logger, string deliveryId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Order {OrderId} delivery status updated to {Status}")]
    private static partial void LogDeliveryStatusUpdated(ILogger logger, int orderId, string status);
}
