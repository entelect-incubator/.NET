Teaching Thread

- From: Previous phase introduces scaffolding and Clean Architecture.
- This phase: learn project layout, layers, and basic DI.
- Next: Phase 2 introduces CQRS handler patterns and explicit handler injection.

Libraries (why they matter)

- Mapperly: code generation for mapping DTOs and entities — reduces boilerplate and teaches mapping separation.
- EF Core InMemory: simplifies testing and demonstrates EF patterns before switching to real DB providers.

Clean Code & SOLID (teaching notes)

- Single Responsibility: keep mapping, persistence and controllers separate.
- Dependency Inversion: depend on interfaces (`IPizzaCore`) for testability.
- Open/Closed: design modules so new features add handlers/services without changing existing code.

MediatR policy

- Phase 1 should not introduce MediatR. The incubator teaches a custom dispatcher pattern later. If a phase intentionally uses MediatR, add explicit rationale in that phase README (pipeline behaviors, notifications, production needs).

Notes

- Link to canonical patterns: ../Design-Patterns/README.md (SOLID, Result, Dispatcher/Mediator)
