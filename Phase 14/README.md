# Phase 14: External API Integration - Mock Delivery Service

This phase teaches you how to integrate with **external APIs** using the **Pezza Mock Delivery Service** as a real-world example. You'll learn to consume HTTP APIs, handle webhooks, implement retry logic, and manage asynchronous operations in a production environment.

Requires .NET SDK: 10.0.x

Estimated time: 4–8 hours — Difficulty: ★★★★☆

Prerequisites: Complete Phase 1-13, understand HTTP clients, dependency injection, and background services.

> ✅ **CODE QUALITY NOTE**: DeliveryService and DeliveryWebhooksController use C# 12+ primary constructors with `[LoggerMessage]` source-generated static logging — allocation-free, high-performance log calls. Unit tests use **[Imposter](https://www.nuget.org/packages/Imposter)** (source-generator mock library) instead of Moq.
> ```csharp
> // ✅ Primary constructor + [LoggerMessage] static logging
> public sealed partial class DeliveryService(
>     HttpClient httpClient,
>     ILogger<DeliveryService> logger) : IDeliveryService
> {
>     [LoggerMessage(Level = LogLevel.Information, Message = "Creating delivery for OrderId: {OrderId}")]
>     private static partial void LogCreatingDelivery(ILogger logger, int orderId);
>
>     // Allocation-free call site — no string interpolation, no boxing
>     // LogCreatingDelivery(logger, request.OrderId);
> }
> ```
> See Phase 13 FinalSolution for additional examples of primary constructor patterns.

## Purpose

Learn how to:

- ✅ Call external REST APIs using `HttpClient`
- ✅ Handle async/await patterns with external services
- ✅ Implement webhook receivers for async notifications
- ✅ Design resilient integrations with retry policies
- ✅ Mock external services for testing
- ✅ Integrate docker-compose for multi-service development
- ✅ Handle idempotent operations
- ✅ Manage state transitions with external systems

## Learning Outcomes

1. **HTTP Client Patterns**
   - Configuring typed `HttpClient` with dependency injection
   - Handling timeouts and connection pooling
   - Adding retry policies with Polly

2. **Webhook Implementation**
   - Receiving async notifications from external services
   - Validating webhook signatures
   - Handling delivery retries

3. **Integration Patterns**
   - Producer-Consumer pattern with delivery service
   - Event-driven architecture
   - Coordinating state between systems

4. **Testing External Integrations**
   - Using mock delivery service in docker-compose
   - Testing webhook handlers with **[Imposter](https://www.nuget.org/packages/Imposter)** (source-generator mock library)
   - Simulating failures and retries

5. **Production Readiness**
   - Configuration management for different environments
   - Structured logging of API calls
   - Monitoring and alerting

## Architecture Overview

```md
┌──────────────────────────────────────────────────────────────┐
│                     Pezza Backend (.NET)                      │
│                                                               │
│  ┌────────────────────────────────────────────────────────┐ │
│  │           Order Service (Clean Architecture)           │ │
│  │                                                         │ │
│  │  ┌─────────────────────────────────────────────────┐  │ │
│  │  │  API Controller (POST /orders)                  │  │ │
│  │  └────────────────┬────────────────────────────────┘  │ │
│  │                   │                                     │ │
│  │  ┌────────────────▼────────────────────────────────┐  │ │
│  │  │  CreateOrderCommand (CQRS)                      │  │ │
│  │  └────────────────┬────────────────────────────────┘  │ │
│  │                   │                                     │ │
│  │  ┌────────────────▼────────────────────────────────┐  │ │
│  │  │  OrderCommandHandler                           │  │ │
│  │  │  1. Save order to database                     │  │ │
│  │  │  2. Call Delivery Service API                  │  │ │
│  │  │  3. Store DeliveryId in Order                 │  │ │
│  │  └────────────────┬────────────────────────────────┘  │ │
│  │                   │                                     │ │
│  └───────────────────┼─────────────────────────────────────┘ │
│                      │                                         │
│                      │ HTTP POST                               │
│                      │ /api/v1/deliveries                      │
│                      │                                         │
└──────────────────────┼──────────────────────────────────────────┘
                       │
        ┌──────────────▼───────────────┐
        │  Mock Delivery Service       │
        │  (Pezzza-Mock-Delivery)     │
        │                              │
        │  • In-memory storage        │
        │  • Status transitions       │
        │  • Webhook callbacks        │
        │  • Background worker        │
        └──────────────┬───────────────┘
                       │
                       │ HTTP POST
                       │ /webhooks/delivery
                       │
┌──────────────────────▼──────────────────────────────────────────┐
│                     Pezza Backend (continued)                    │
│                                                                  │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │  Webhook Receiver Endpoint (POST /webhooks/delivery)       │ │
│  │                                                             │ │
│  │  1. Receive delivery status change notification            │ │
│  │  2. Validate webhook signature (if security enabled)       │ │
│  │  3. Find Order by DeliveryId                              │ │
│  │  4. Update Order.DeliveryStatus                           │ │
│  │  5. Publish OrderDeliveryStatusChanged event              │ │
│  │  6. Send notification to customer                         │ │
│  │  7. Log webhook receipt                                   │ │
│  │  8. Return 200 OK (acknowledge receipt)                   │ │
│  │                                                             │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
└──────────────────────────────────────────────────────────────────┘
```

## Delivery Status Flow

```md
User Places Order
        ↓
POST /orders (Pezza Backend)
        ↓
OrderCommandHandler executes
        ├─ Save Order to DB
        └─ Call Delivery Service
          ├─ POST /api/v1/deliveries
          └─ Receive DeliveryId
        ↓
Order.DeliveryId = "guid"
Order.DeliveryStatus = "Created"
        ↓
[Mock Delivery Service - Background Worker]
Every 30 seconds (configurable):
        ├─ Created → PickedUp (sends webhook)
        ├─ PickedUp → OnTheWay (sends webhook)
        └─ OnTheWay → Delivered (sends webhook)
        ↓
[Pezza Backend - Webhook Handler]
POST /webhooks/delivery
        ├─ Verify signature
        ├─ Find Order
        ├─ Update Order.DeliveryStatus
        ├─ Publish event
        └─ Send customer notification
        ↓
Customer sees: "Your order is on the way! 🚗"
```

## Phase Solutions

### Solution 1: StartSolution

A skeleton project structure with:

- Domain models with DeliveryReference
- Application layer with HTTP client configuration
- API controllers ready for implementation
- Webhook receiver endpoint stub
- appsettings.json prepared for delivery service URL

### Solution 2: FinalSolution

Complete implementation with:

- Fully configured `HttpClient` with Polly retry policies
- `DeliveryService` class that calls external API
- `DeliveryWebhookHandler` for receiving status updates
- Integration with existing Order entity
- Docker-compose setup with Mock Delivery Service
- Unit tests for API integration
- Example webhook payload validation

## Key Patterns Taught

### 1. Typed HttpClient Pattern

```csharp
// Dependency injection
services.AddHttpClient<IDeliveryService, DeliveryService>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://localhost:8081"))
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());
```

---

Teaching Thread

- From: Phase 13 integrated AI capabilities.
- This phase: implement external API integration (webhooks, resilient calls, retry policies).
- Next: Phase 15 covers CI/CD and container publishing.

Libraries (why they matter)

- Polly: resilience and retry policies — document why to choose it and example policies.
- Typed HttpClient: demonstrate typed clients and testable integration layers.

Clean Code & SOLID (teaching notes)

- Keep external API adapters thin and testable; wrap retries at the adapter boundary and avoid leaking retries into business logic.

MediatR policy

- External integration should not rely on MediatR; if used, document the choice and where MediatR pipelines provide value.

Notes

- Add examples for webhook receivers and idempotency handling.

### 2. Webhook Handler Pattern

```csharp
[HttpPost("/webhooks/delivery")]
public async Task<IActionResult> HandleDeliveryWebhook(
    [FromBody] DeliveryWebhookPayload payload)
{
    var order = await orderService.GetByDeliveryIdAsync(payload.DeliveryId);
    order.DeliveryStatus = payload.Status.ToString();
    await orderService.UpdateAsync(order);
    return Ok();
}
```

### 3. Resilience Policies

```csharp
private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
    HttpPolicyExtractor
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
            onRetry: (outcome, timespan, attempt, context) =>
                logger.LogWarning("Retrying delivery API call. Attempt: {Attempt}", attempt)
        );
```

## Docker Compose Setup

Complete docker-compose.yml for local development:

- Pezza backend (.NET API)
- Mock Delivery Service
- Database (SQL Server)
- Adminer (database viewer)

Run entire stack locally:

```bash
docker-compose up -d
```

## Code Examples

See `/src` folder:

**01. StartSolution/**

- `Models/DeliveryReference.cs` - Domain model
- `Services/IDeliveryService.cs` - Interface only
- `Controllers/OrdersController.cs` - Skeleton
- `appsettings.json` - Configuration template

**02. FinalSolution/**

- Complete working implementation
- HTTP client with retry policies
- Webhook receiver endpoint
- Unit tests using **Imposter** source-generator mocks (no Moq)
- Docker-compose with all services

## External Resources

- 📡 **Mock Delivery Service**: https://github.com/entelect-incubator/Pezzza-Mock-Delivery-Service
  - Start: `docker run -p 8081:8080 ghcr.io/entelect-incubator/pezzza-mock-delivery-service:latest`
  - Docs: README.md, openapi/delivery-api.yaml
  - Training: TRAINING.md, AI-INTEGRATION-GUIDE.md

- 📚 **HttpClient Best Practices**: https://docs.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
- 🔄 **Polly Resilience Library**: https://github.com/App-vNext/Polly
- 🔗 **MediatR CQRS Pattern**: https://github.com/jbogard/MediatR
- 🪝 **Webhook Best Practices**: https://docs.gitea.io/en-us/webhooks/ (general patterns)

## Learning Path

1. **Start with FinalSolution**
   - Run `docker-compose up -d`
   - Review `DeliveryService.cs`
   - Understand HttpClient configuration
   - See webhook handler implementation

2. **Hands-on Exercise**
   - Try implementing from StartSolution
   - Follow TODO comments in code
   - Create HTTP client with retry logic
   - Implement webhook receiver

3. **Testing**
   - Run unit tests
   - Use Scalar UI to test Mock Delivery API (http://localhost:8081/scalar/v1)
   - Send test webhooks manually with curl
   - Trigger failure scenarios

4. **Extend**
   - Add signature validation to webhooks
   - Implement idempotency checks
   - Add custom headers to API calls
   - Implement circuit breaker pattern

## Validation

**Build the solution:**

```bash
dotnet build "Phase 14/src/02. FinalSolution/Pezza.slnx" -c Release
```

**Run tests:**

```bash
dotnet test "Phase 14/src/02. FinalSolution/Pezza.slnx" -c Release
```

**Start services:**

```bash
cd "Phase 14/src/02. FinalSolution"
docker-compose up -d
```

**Verify Mock Delivery Service:**

```bash
curl http://localhost:8081/health
curl http://localhost:8081/scalar/v1  # Interactive API docs
```

## Deliverables

✅ Clean architecture with integration layer  
✅ Typed HttpClient with dependency injection  
✅ Retry policies using Polly  
✅ Webhook receiver endpoint  
✅ Integration with Order entity  
✅ Docker-compose for multi-service development  
✅ Unit tests for happy path and error scenarios  
✅ Configuration for dev/prod environments  
✅ Comprehensive logging and monitoring  
✅ Error handling and graceful degradation  

## Modern .NET Patterns

This phase demonstrates:

- **Primary Constructors** - Clean dependency injection
- **`[LoggerMessage]` Static Logging** - Source-generated, allocation-free log methods
- **Records for DTOs** - Immutable webhook payloads
- **Async/Await** - Non-blocking API calls
- **Dependency Injection** - Typed HttpClient pattern
- **Result<T> Pattern** - Consistent error handling
- **MediatR** - CQRS command handling
- **Minimal APIs** (reference) - See Mock Delivery Service implementation
- **Extension Methods** - Polly policy extensions
- **Imposter** - Source-generator mocking for unit tests (`[assembly: GenerateImposter(typeof(...))]`)

[Move to Phase 15](https://github.com/entelect-incubator/.NET/tree/master/Phase%2015)
