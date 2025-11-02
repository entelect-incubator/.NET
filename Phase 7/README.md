

# &nbsp;**Pezza - Phase 7 — Events & Background Tasks** [![.NET - Phase 7 - Final Solution](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase7-finalsolution.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase7-finalsolution.yml)

![Pezza logo](./Assets/pezza-logo.png)

## Quick facts

- .NET SDK required: 10 (net10)
- Estimated time: 6 - 10 hours
- Difficulty: ★★★★★ (advanced)
- Audience: developers who completed Phase 6; ready to implement event-driven architecture and background processing
- **Building on**: Phase 6's caching and optimization patterns
- **New Concepts**: Domain events, event handlers, background job scheduling

## Goal

This phase introduces **event-driven architecture** and **background job processing** to complete a production-grade system:

- **Domain Events**: Publish events from command handlers to decouple concerns
- **Event Handlers**: React to events asynchronously without blocking command execution
- **Background Jobs**: Schedule reliable, retryable work (email, notifications, exports) using Hangfire
- **Idempotency**: Ensure background jobs can safely retry without side effects
- **Order Processing**: Complete example of event-driven order workflow

Learn to build scalable, loosely-coupled systems where commands trigger events, and handlers react asynchronously.

## Prerequisites

- Completed Phase 6 (understand caching, optimization, and dispatcher foundation)
- .NET 10 SDK installed and on PATH
- Familiarity with event-driven patterns and background job concepts
- Understanding of async/await and task scheduling

## How to validate this phase locally

1. Build the start solution:

```powershell
dotnet build "Phase 7/src/01. StartSolution/Pezza.sln"
```

2. Run tests:

```powershell
dotnet test "Phase 7/src/01. StartSolution/Pezza.sln"
```

## Topics / learning outcomes

- Implement **domain events** that commands publish
- Create **event handlers** that react to domain events asynchronously
- Understand the **Publisher-Subscriber pattern** and decoupling benefits
- Use **Hangfire** for reliable, scheduled background jobs with retry logic
- Implement **idempotent event handlers** to handle retries safely
- Build an **event-driven order processing workflow** as capstone

## Key Patterns: Events & Background Jobs

**Publishing Domain Events:**

```csharp
// Command publishes event after successful execution
public sealed class CreateOrderCommandHandler(
    DatabaseContext db,
    Dispatcher dispatcher) 
    : ICommandHandler<CreateOrderCommand, Result<OrderModel>>
{
    public async Task<Result<OrderModel>> Handle(CreateOrderCommand command, CancellationToken ct)
    {
        var order = new Order { /* ... */ };
        db.Orders.Add(order);
        await db.SaveChangesAsync(ct);

        // Publish event — handlers will react asynchronously
        await dispatcher.Publish(
            new OrderCreatedEvent { OrderId = order.Id }, 
            ct);

        return Result<OrderModel>.Success(order.Map());
    }
}
```

**Reacting to Events:**

```csharp
// Event handler — executes asynchronously without blocking command
public sealed class SendOrderConfirmationHandler(
    IEmailService emailService) 
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken ct)
    {
        // Send confirmation email asynchronously
        await emailService.SendOrderConfirmationAsync(
            notification.OrderId, 
            ct);
    }
}
```

**Background Jobs with Hangfire:**

```csharp
// Schedule a job to retry email sending
public sealed class EmailServiceJob
{
    public async Task SendOrderEmailAsync(int orderId)
    {
        var order = await db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order is null) return; // Idempotent — safe to retry

        try
        {
            await emailService.SendAsync(order.Email, order.ConfirmationTemplate);
        }
        catch
        {
            // Hangfire will retry automatically
            BackgroundJob.Schedule(
                () => SendOrderEmailAsync(orderId),
                TimeSpan.FromMinutes(5));
        }
    }
}
```

## References

- Domain Events Pattern: [Vaughn Vernon - Domain Events](https://vaughnvernon.com/2010/04/08/domain-events/)
- Publisher-Subscriber Pattern: [Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/pub-sub)
- Hangfire: [https://www.hangfire.io/](https://www.hangfire.io/)
- Idempotency: [Idempotent API Design](https://stackoverflow.com/questions/1077412/what-is-an-idempotent-operation)
- Event Sourcing (advanced): [Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation)

## Architecture Diagram: Event Flow

```
User Request
    ↓
Controller Receives CreateOrderCommand
    ↓
Dispatcher.Send(command)
    ↓
CreateOrderCommandHandler executes
    ├─→ Validates command
    ├─→ Creates order in database
    ├─→ Returns OrderModel result
    └─→ Publishes OrderCreatedEvent
         ↓
    Event Handlers (Async):
    ├─→ SendOrderConfirmationHandler
    ├─→ UpdateInventoryHandler
    ├─→ LogOrderMetricsHandler
         ↓
    Background Jobs (Hangfire):
    ├─→ EmailServiceJob (retry on failure)
    ├─→ NotificationServiceJob
    └─→ ExportOrderJob (scheduled)
```

## Steps

- [ ] [Step 1 - Email Service & Hangfire Setup](Phase%207/src/02.%20Step%201)
- [ ] [Step 2 - Domain Events & Notifications](Phase%207/src/03.%20Step%202)
- [ ] [Step 3 - Background Job Scheduling](Phase%207/src/04.%20Step%203)
- [ ] [Step 4 - Order Processing Workflow](Phase%207/src/04.%20Step%204)
