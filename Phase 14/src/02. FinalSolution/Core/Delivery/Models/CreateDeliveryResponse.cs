namespace Core.Delivery.Models;

public sealed class CreateDeliveryResponse
{
    public string DeliveryId { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime EstimatedDeliveryTime { get; set; }
}
