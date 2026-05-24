namespace Core.Delivery.Models;

public sealed class CreateDeliveryRequest
{
    public int OrderId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string DeliveryAddress { get; set; } = string.Empty;

    public string WebhookCallbackUrl { get; set; } = string.Empty;
}
