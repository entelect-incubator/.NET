namespace Core.Delivery.Models;

public sealed class DeliveryWebhookPayload
{
    public string DeliveryId { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string OrderId { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }

    public string? DriverName { get; set; }

    public string? Notes { get; set; }
}
