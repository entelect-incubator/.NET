namespace Core.Delivery;

using Core.Delivery.Models;

public interface IDeliveryService
{
    Task<CreateDeliveryResponse?> CreateDeliveryAsync(CreateDeliveryRequest request, CancellationToken cancellationToken = default);

    Task<string?> GetDeliveryStatusAsync(string deliveryId, CancellationToken cancellationToken = default);
}
