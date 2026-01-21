namespace Pezza.Application.Contracts.Delivery;

public record CreateDeliveryRequest
{
    public required string OrderId { get; init; }
    
    public required DeliveryAddressDto PickupAddress { get; init; }
    
    public required DeliveryAddressDto DeliveryAddress { get; init; }
    
    public string? WebhookUrl { get; init; }
    
    public bool ForceFail { get; init; } = false;
}

public record DeliveryAddressDto
{
    public required string Street { get; init; }
    
    public required string City { get; init; }
    
    public required string PostalCode { get; init; }
}

public record DeliveryResponse
{
    public string DeliveryId { get; init; } = string.Empty;
    
    public string Status { get; init; } = "Created";
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime? PickedUpAt { get; init; }
    
    public DateTime? DeliveredAt { get; init; }
    
    public DateTime? CancelledAt { get; init; }
    
    public string? FailureReason { get; init; }
    
    public DriverDto? Driver { get; init; }
    
    public DeliveryAddressDto? DeliveryAddress { get; init; }
}

public record DriverDto
{
    public string Name { get; init; } = string.Empty;
    
    public string Phone { get; init; } = string.Empty;
    
    public string VehicleNumber { get; init; } = string.Empty;
}

public record DeliveryWebhookPayload
{
    public string DeliveryId { get; init; } = string.Empty;
    
    public string OrderId { get; init; } = string.Empty;
    
    public string Status { get; init; } = string.Empty;
    
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    
    public DriverDto? Driver { get; init; }
    
    public string? FailureReason { get; init; }
}
