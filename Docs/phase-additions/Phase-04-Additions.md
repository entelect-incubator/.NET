Teaching Thread

- From: Phase 3 provided a custom dispatcher foundation.
- This phase: add validation, filtering, pagination and structured exception handling.
- Next: Phase 5 focuses on code standards, analyzers and production-ready error handling.

Libraries (why they matter)

- FluentValidation: expressive validation rules and clear testability benefits.
- ProblemDetails/AddProblemDetails: standardize API error shapes (RFC 7231).

Clean Code & SOLID (teaching notes)

- Prefer explicit validation helpers and keep validators small and focused.
- Use exception handlers to separate error response concerns from business logic.

MediatR policy

- Phase 4 continues to use the custom dispatcher; avoid introducing MediatR unless a later phase documents its added value.

Notes

- Link to validation and clean-code patterns: ../../Design-Patterns/08-Clean-Code-Principles/
