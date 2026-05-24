namespace Core.Delivery;

using System.Net.Http.Json;
using Core.Delivery.Models;
using Microsoft.Extensions.Logging;

public sealed partial class DeliveryService(
    HttpClient httpClient,
    ILogger<DeliveryService> logger) : IDeliveryService
{
    public async Task<CreateDeliveryResponse?> CreateDeliveryAsync(CreateDeliveryRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            LogCreatingDelivery(logger, request.OrderId);

            var response = await httpClient.PostAsJsonAsync("api/v1/deliveries", request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                LogDeliveryServiceErrorStatus(logger, (int)response.StatusCode, request.OrderId);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<CreateDeliveryResponse>(cancellationToken: cancellationToken);
            LogDeliveryCreated(logger, result?.DeliveryId, request.OrderId);
            return result;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            LogCreateDeliveryFailed(logger, ex, request.OrderId);
            return null;
        }
    }

    public async Task<string?> GetDeliveryStatusAsync(string deliveryId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<CreateDeliveryResponse>(
                $"api/v1/deliveries/{deliveryId}", cancellationToken);
            return response?.Status;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            LogGetDeliveryStatusFailed(logger, ex, deliveryId);
            return null;
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Creating delivery for OrderId: {OrderId}")]
    private static partial void LogCreatingDelivery(ILogger logger, int orderId);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Delivery service returned {StatusCode} for OrderId: {OrderId}")]
    private static partial void LogDeliveryServiceErrorStatus(ILogger logger, int statusCode, int orderId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Delivery created. DeliveryId: {DeliveryId} for OrderId: {OrderId}")]
    private static partial void LogDeliveryCreated(ILogger logger, string? deliveryId, int orderId);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to create delivery for OrderId: {OrderId}")]
    private static partial void LogCreateDeliveryFailed(ILogger logger, Exception ex, int orderId);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to get delivery status for DeliveryId: {DeliveryId}")]
    private static partial void LogGetDeliveryStatusFailed(ILogger logger, Exception ex, string deliveryId);
}