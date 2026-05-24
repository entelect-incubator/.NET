namespace Core.Delivery;

using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

public static class DeliveryPolicyExtensions
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger) =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (outcome, timespan, attempt, context) =>
                    logger.LogWarning(
                        "Retry attempt {Attempt} for delivery API call after {Delay}s. Reason: {Reason}",
                        attempt,
                        timespan.TotalSeconds,
                        outcome.Exception?.Message ?? outcome.Result?.ReasonPhrase));

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(ILogger logger) =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, breakDelay) =>
                    logger.LogWarning(
                        "Delivery service circuit breaker opened for {BreakDelay}s. Reason: {Reason}",
                        breakDelay.TotalSeconds,
                        outcome.Exception?.Message ?? outcome.Result?.ReasonPhrase),
                onReset: () =>
                    logger.LogInformation("Delivery service circuit breaker reset."));
}
