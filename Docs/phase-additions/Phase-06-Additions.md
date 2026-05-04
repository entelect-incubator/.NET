Teaching Thread

- From: Phase 5 enforced standards and error handling.
- This phase: introduce domain events, background jobs, and distributed-safe patterns.
- Next: Phase 7 expands microservices and service-to-service communication.

Libraries (why they matter)

- Hangfire / Background worker frameworks: teaches background processing patterns and reliability concerns.
- Lightweight messaging/eventing utilities: demonstrate domain events without full message brokers.

Clean Code & SOLID (teaching notes)

- Design event handlers with single responsibility; prefer idempotent handlers.
- Keep performance and observability responsibilities out of domain handlers; inject logging/telemetry instead.

MediatR policy

- Continue using the custom dispatcher for command/query flows; if events require richer mediator features, document why a third-party mediator is used.

Notes

- Link to Design-Patterns for Events and CQRS: ../../Design-Patterns/03-CQRS-Pattern/
