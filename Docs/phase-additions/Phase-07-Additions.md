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
