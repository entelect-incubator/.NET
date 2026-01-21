namespace Pezza.Application.Services;

using Pezza.Application.Contracts.Delivery;

/// <summary>
/// Service for interacting with the external Delivery Service API
/// 
/// This service handles:
/// - Creating new deliveries
/// - Querying delivery status
/// - Cancelling deliveries
/// 
/// Built with:
/// - Typed HttpClient with dependency injection
/// - Polly retry policies for resilience
/// - Structured logging
/// </summary>
public interface IDeliveryService
{
    /// <summary>
    /// Create a new delivery in the delivery service
    /// </summary>
    /// <param name="request">Delivery details (pickup, dropoff addresses, webhook)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Delivery response with ID and initial status</returns>
    Task<DeliveryResponse?> CreateDeliveryAsync(
        CreateDeliveryRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the current status of a delivery
    /// </summary>
    /// <param name="deliveryId">The delivery ID returned from CreateDeliveryAsync</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Current delivery details</returns>
    Task<DeliveryResponse?> GetDeliveryAsync(
        string deliveryId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancel an in-progress delivery
    /// </summary>
    /// <param name="deliveryId">The delivery ID to cancel</param>
    /// <param name="reason">Reason for cancellation</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated delivery with cancelled status</returns>
    Task<DeliveryResponse?> CancelDeliveryAsync(
        string deliveryId,
        string reason = "Cancelled by customer",
        CancellationToken cancellationToken = default);
}

public class DeliveryService : IDeliveryService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<DeliveryService> logger;

    public DeliveryService(HttpClient httpClient, ILogger<DeliveryService> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task<DeliveryResponse?> CreateDeliveryAsync(
        CreateDeliveryRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation(
                "Creating delivery for order {OrderId} to {City}",
                request.OrderId,
                request.DeliveryAddress.City);

            var response = await httpClient.PostAsJsonAsync(
                "/api/v1/deliveries",
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<DeliveryResponse>(cancellationToken);

            logger.LogInformation(
                "Delivery created successfully. DeliveryId: {DeliveryId}, OrderId: {OrderId}",
                result?.DeliveryId,
                request.OrderId);

            return result;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(
                ex,
                "Failed to create delivery for order {OrderId}. Error: {Error}",
                request.OrderId,
                ex.Message);

            // In production, you might want to return a Result<T> pattern
            // for better error handling instead of returning null
            return null;
        }
    }

    public async Task<DeliveryResponse?> GetDeliveryAsync(
        string deliveryId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Fetching delivery status for {DeliveryId}", deliveryId);

            var response = await httpClient.GetAsync(
                $"/api/v1/deliveries/{deliveryId}",
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<DeliveryResponse>(cancellationToken);

            logger.LogInformation(
                "Delivery status retrieved. DeliveryId: {DeliveryId}, Status: {Status}",
                deliveryId,
                result?.Status);

            return result;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(
                ex,
                "Failed to get delivery status for {DeliveryId}. Error: {Error}",
                deliveryId,
                ex.Message);

            return null;
        }
    }

    public async Task<DeliveryResponse?> CancelDeliveryAsync(
        string deliveryId,
        string reason = "Cancelled by customer",
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation(
                "Cancelling delivery {DeliveryId}. Reason: {Reason}",
                deliveryId,
                reason);

            var request = new { Reason = reason };

            var response = await httpClient.PostAsJsonAsync(
                $"/api/v1/deliveries/{deliveryId}/cancel",
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<DeliveryResponse>(cancellationToken);

            logger.LogInformation(
                "Delivery cancelled successfully. DeliveryId: {DeliveryId}",
                deliveryId);

            return result;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(
                ex,
                "Failed to cancel delivery {DeliveryId}. Error: {Error}",
                deliveryId,
                ex.Message);

            return null;
        }
    }
}
