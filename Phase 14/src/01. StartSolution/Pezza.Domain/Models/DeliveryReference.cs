// Domain model to reference external delivery service
namespace Pezza.Domain.Models;

public class DeliveryReference
{
    /// <summary>
    /// Unique identifier for this order (from Pezza system)
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The external delivery service's delivery ID
    /// </summary>
    public string DeliveryServiceId { get; set; } = string.Empty;

    /// <summary>
    /// Current status of the delivery from external service
    /// Values: Created, PickedUp, OnTheWay, Delivered, Failed, Cancelled
    /// </summary>
    public string Status { get; set; } = "Created";

    /// <summary>
    /// When delivery was created in external service
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When delivery was marked as picked up
    /// </summary>
    public DateTime? PickedUpAt { get; set; }

    /// <summary>
    /// When delivery was completed or failed
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Reason if delivery failed (e.g., "Customer not available")
    /// </summary>
    public string? FailureReason { get; set; }

    /// <summary>
    /// Driver assigned by delivery service
    /// </summary>
    public string? DriverName { get; set; }

    /// <summary>
    /// Driver contact number
    /// </summary>
    public string? DriverPhone { get; set; }

    /// <summary>
    /// Vehicle registration number
    /// </summary>
    public string? VehicleNumber { get; set; }
}
