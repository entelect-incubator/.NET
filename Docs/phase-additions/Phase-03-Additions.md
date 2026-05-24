Teaching Thread

- From: Phase 2 used explicit handler injection.
- This phase: build a minimal Dispatcher (`MediatorLite`) to centralize routing and enable cross-cutting concerns.
- Next: Phase 4 introduces validation behaviors and decorators.

Libraries (why they matter)

- Scrutor: DI registration of handlers via assembly scanning — reduces manual registration.
- EF Core: taught with compiled queries to demonstrate performance improvements.
- Result<T>: consistent operation results across handlers.

Clean Code & SOLID (teaching notes)

- Keep handlers focused on business logic; mapping and persistence should be separate.
- Use interfaces for handler dependencies for testability.

MediatR policy

- Phase 3 should avoid MediatR; the goal is to teach how dispatching works under the hood. If MediatR is used later, document rationale.

Notes

- Link to Dispatcher/Mediator design-pattern guide: ../../Design-Patterns/04-Dispatcher-Mediator/
