Teaching Thread

- From: Phase 1 scaffolded a layered solution.
- This phase: introduce CQRS handler interfaces and explicit DI injection.
- Next: Phase 3 will centralize routing with a Dispatcher/Mediator.

Libraries (why they matter)

- Scrutor: automatic assembly scanning to register handlers — demonstrates DI patterns and reduces ceremony.
- Result<T> pattern: standardizes success/failure across handlers.

Clean Code & SOLID (teaching notes)

- Keep controller dependencies explicit for clarity at this stage.
- Use small, focused handler interfaces to enforce single responsibility.

MediatR policy

- Phase 2 uses explicit handler interfaces (no MediatR). Avoid adding MediatR here; Phase 3 teaches a custom dispatcher first.

Notes

- Link to canonical patterns: ../Design-Patterns/03-CQRS-Pattern/ and ../Design-Patterns/02-Result-Pattern/
