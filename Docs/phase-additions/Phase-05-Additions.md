Teaching Thread

- From: Phase 4 added validation and data handling.
- This phase: enforce code standards, analyzers, and centralized error handling.
- Next: Phase 6 will add background jobs and events.

Libraries (why they matter)

- StyleCop / Roslyn analyzers: enforce consistent code quality and automations for learning good habits.
- Serilog (or similar): structured logging for observability.

Clean Code & SOLID (teaching notes)

- Use analyzers to enforce naming and layout; keep solutions consistent to reduce cognitive load.
- Map `Result<T>` to ProblemDetails in a single place (exception handler) rather than ad-hoc responses.

MediatR policy

- Phase 5 expects the custom dispatcher to be used in production-like flows; do not introduce MediatR without documenting the reasons.

Notes

- Link to Clean Code guide and testing patterns: ../../Design-Patterns/08-Clean-Code-Principles/
