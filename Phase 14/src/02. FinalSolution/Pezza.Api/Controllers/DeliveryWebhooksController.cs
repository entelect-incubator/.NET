namespace Pezza.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Pezza.Application.Contracts.Delivery;
using Pezza.Application.Services;

/// <summary>
/// Webhook receiver for delivery service notifications
/// 
/// The external delivery service will POST to this endpoint whenever
/// the delivery status changes (PickedUp, OnTheWay, Delivered, Failed, etc.)
/// 
/// This demonstrates:
/// - Receiving webhook callbacks from external APIs
/// - Validating webhook payloads
/// - Updating internal state based on external events
/// - Event-driven architecture patterns
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DeliveryWebhooksController : ControllerBase
{
    private readonly ILogger<DeliveryWebhooksController> logger;

    public DeliveryWebhooksController(ILogger<DeliveryWebhooksController> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Receive delivery status update notification
    /// 
    /// Called by the delivery service whenever a delivery status changes.
    /// Expected to return 200 OK to acknowledge receipt.
    /// 
    /// TODO: Implement the following:
    /// 1. Validate webhook signature (if enabled)
    /// 2. Find Order by payload.DeliveryId (from your database)
    /// 3. Update Order.DeliveryStatus to payload.Status
    /// 4. Update Order.Driver info if available
    /// 5. Publish OrderDeliveryStatusChanged event (MediatR)
    /// 6. Send customer notification (email/SMS)
    /// 7. Log webhook receipt for auditing
    /// 8. Return 200 OK
    /// 
    /// Example payload:
    /// {
    ///   "deliveryId": "abc123def456",
    ///   "orderId": "order-789",
    ///   "status": "OnTheWay",
    ///   "timestamp": "2025-01-18T10:30:00Z",
    ///   "driver": {
    ///     "name": "John Smith",
    ///     "phone": "+27812345678",
    ///     "vehicleNumber": "AB 12 CDE"
    ///   },
    ///   "failureReason": null
    /// }
    /// </summary>
    [HttpPost("delivery")]
    public async Task<IActionResult> OnDeliveryStatusChanged(
        [FromBody] DeliveryWebhookPayload payload,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation(
                "Received delivery webhook. DeliveryId: {DeliveryId}, Status: {Status}, OrderId: {OrderId}",
                payload.DeliveryId,
                payload.Status,
                payload.OrderId);

            // TODO: Validate webhook signature if authentication is enabled
            // var isValid = ValidateWebhookSignature(Request.Headers, payload);
            // if (!isValid) return Unauthorized();

            // TODO: Find the Order associated with this delivery
            // var order = await mediator.Send(
            //     new GetOrderByDeliveryIdQuery(payload.DeliveryId),
            //     cancellationToken);
            // 
            // if (order == null)
            // {
            //     logger.LogWarning("Order not found for delivery {DeliveryId}", payload.DeliveryId);
            //     return NotFound();
            // }

            // TODO: Update order with delivery status
            // var command = new UpdateOrderDeliveryStatusCommand(
            //     orderId: order.Id,
            //     deliveryStatus: payload.Status,
            //     driver: payload.Driver,
            //     failureReason: payload.FailureReason);
            // 
            // var result = await mediator.Send(command, cancellationToken);

            // TODO: If delivery succeeded, handle order completion
            // if (payload.Status == "Delivered")
            // {
            //     await mediator.Publish(
            //         new OrderDeliveredEvent(order.Id, payload.Timestamp),
            //         cancellationToken);
            // }

            logger.LogInformation(
                "Webhook processed successfully. DeliveryId: {DeliveryId}, NewStatus: {Status}",
                payload.DeliveryId,
                payload.Status);

            // Always return 200 OK to acknowledge receipt to the delivery service
            // This prevents the delivery service from retrying the webhook
            return Ok(new { message = "Webhook received and processed" });
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error processing delivery webhook. DeliveryId: {DeliveryId}",
                payload.DeliveryId);

            // Return 500 to signal the delivery service to retry
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { error = "Failed to process webhook" });
        }
    }

    /// <summary>
    /// Manual trigger for testing webhook handler
    /// 
    /// Useful during development to simulate delivery status changes
    /// without waiting for the mock delivery service.
    /// 
    /// Example curl:
    /// curl -X POST http://localhost:5000/api/delivery-webhooks/test \
    ///   -H "Content-Type: application/json" \
    ///   -d '{
    ///     "deliveryId": "test-delivery-123",
    ///     "orderId": "order-456",
    ///     "status": "OnTheWay",
    ///     "timestamp": "2025-01-18T10:30:00Z"
    ///   }'
    /// </summary>
    [HttpPost("test")]
    [ApiExplorerSettings(IgnoreApi = true)] // Hide from Swagger in production
    public async Task<IActionResult> TestWebhook(
        [FromBody] DeliveryWebhookPayload payload,
        CancellationToken cancellationToken)
    {
        logger.LogWarning(
            "TEST WEBHOOK TRIGGERED. DeliveryId: {DeliveryId}, Status: {Status}",
            payload.DeliveryId,
            payload.Status);

        // Call the actual handler
        return await OnDeliveryStatusChanged(payload, cancellationToken);
    }
}
