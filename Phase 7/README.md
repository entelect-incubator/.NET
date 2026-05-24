# &nbsp;**Pezza - Phase 7  Events & Background Tasks** [![.NET - Phase 7 - Final Solution](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase7-finalsolution.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase7-finalsolution.yml)

![Pezza logo](./Assets/pezza-logo.png)

## Quick facts

- .NET SDK required: 10 (net10)
- Estimated time: 6 - 10 hours
- Difficulty:  (advanced)
- Audience: developers who completed Phase 6; ready to implement event-driven architecture and background processing
- **Building on**: Phase 6's caching and optimization patterns
- **New Concepts**: Domain events, event handlers, background job scheduling with Hangfire

## Why Phase 7? Event-Driven Architecture

In previous phases, commands executed synchronously and returned results immediately. But in production systems:

- **Tight Coupling**: Handlers directly call multiple services (email, notifications, exports), blocking responses
- **Scalability Issues**: Long-running work (sending emails, generating reports) slows down API responses
- **Retry Challenges**: If email fails, the entire operation fails; no built-in retry logic
- **Separation of Concerns**: Business logic (order creation) mixes with side effects (notifications)

**Solution**: Decouple commands from their side effects using **domain events** and **background jobs**:

- Commands publish events after success, then return immediately
- Event handlers react asynchronously without blocking the user
- Background jobs (Hangfire) handle retries, scheduling, and reliability
- Result: Faster APIs, loosely-coupled features, production-ready error handling

## What We're Building

A **complete event-driven order processing system** with event handlers and background jobs.

## Design Patterns Used in This Phase

- **[CQRS Pattern](https://github.com/entelect-incubator/Design-Patterns/tree/main/CQRS)**  Commands publish domain events
- **[Dispatcher/Mediator Pattern](https://github.com/entelect-incubator/Design-Patterns/tree/main/Dispatcher-Mediator)**  Custom dispatcher with Publish() method
- **[Publisher-Subscriber Pattern](https://learn.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/pub-sub)**  Event handlers react to events
- **[Result Pattern](https://github.com/entelect-incubator/Design-Patterns/tree/main/Result-Pattern)**  Background jobs use Result<T> for idempotent retries
- **[Idempotent Operations](https://stackoverflow.com/questions/1077412/what-is-an-idempotent-operation)**  Jobs safe to retry

## Prerequisites

- Completed Phase 6
- .NET 10 SDK installed
- Understanding of event-driven patterns
- Familiarity with Hangfire

## How to validate this phase locally

\\\powershell
dotnet build "Phase 7/src/01. StartSolution/Pezza.slnx"
dotnet test "Phase 7/src/01. StartSolution/Pezza.slnx"
\\\

## Topics / learning outcomes

- Implement domain events that commands publish via Dispatcher.Publish()
- Create event handlers (INotificationHandler<>) that react asynchronously
- Understand Publisher-Subscriber pattern and decoupling
- Use Hangfire for reliable background jobs with automatic retries
- Design idempotent event handlers for safe retries
- Build complete event-driven order processing workflow

## Knowledge Check: Event-Driven Architecture

1. **What happens when a command handler publishes a domain event?**
   - **Answer**: The handler calls \dispatcher.Publish(event, ct)\, which returns immediately. The Dispatcher executes all registered event handlers asynchronously in parallel without blocking the command's response.

2. **How does Publisher-Subscriber pattern benefit Phase 7?**
   - **Answer**: Commands focus on core business logic. Side effects (email, inventory, logging) separate into independent handlers. If one fails, others still execute. Handlers can be added/removed without changing commands.

3. **Why are Hangfire jobs idempotent?**
   - **Answer**: On failure, Hangfire automatically retries. Without idempotency, retrying creates duplicates (email twice, deduct stock twice). With checks like \if (order.ConfirmationSentAt != null) return;\, retries are safe.

4. **What's the difference between \dispatcher.Send()\ and \dispatcher.Publish()\?**
   - **Answer**: \Send()\ executes a single command handler synchronously and blocks. \Publish()\ executes all event handlers asynchronously in parallel without blocking. Send is request-response; Publish is fire-and-forget.

5. **How does the Dispatcher register event handlers?**
   - **Answer**: In DependencyInjection.cs, Scrutor scans for types assignable to \INotificationHandler<>\ and registers them as scoped. When \Publish()\ is called, the Dispatcher retrieves handlers via \_serviceProvider.GetServices<INotificationHandler<TEvent>>()\.

6. **When should you use domain events instead of calling methods directly?**
   - **Answer**: Use events for optional side effects (email, logging, notifications). Use direct calls for required operations (validate, deduct stock immediately). Phase 7 decouples optional work into events.

## Steps

- [ ] [Step 1 - Email Service & Hangfire Setup](./Step%201)
- [ ] [Step 2 - Domain Events & Notifications](./Step%202)
- [ ] [Step 3 - Background Job Scheduling](./Step%203)
- [ ] [Step 4 - Order Processing Workflow](./Step%204)

---

**Phase 7 Complete**: You now have an event-driven, production-ready order processing system with reliable background jobs!

[Move to Phase 8](https://github.com/entelect-incubator/.NET/tree/master/Phase%208)
 
## Next Step
Move to [Phase 8](https://github.com/entelect-incubator/.NET/tree/master/Phase%208)
---

Teaching Thread

- From: Phase 6 introduced background jobs and events.
- This phase: show microservices patterns, client generation, and service contracts.
- Next: Phase 8/9 cover security and UI consumption scenarios.

Libraries (why they matter)

- NSwag/OpenAPI tools: generate clients and demonstrate contract-first design benefits.
- Typed HttpClient patterns for resilience and testability.

Clean Code & SOLID (teaching notes)

- Keep service boundaries explicit and document API contracts.
- Ensure client generation is reproducible and part of CI (versioned OpenAPI schema).

MediatR policy

- Microservice integration can use a mediator on the consumer side; prefer documenting if MediatR is selected and why.

Notes

- Link to OpenAPI and client-generation patterns: ../../Design-Patterns/04-Dispatcher-Mediator/
