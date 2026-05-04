Teaching Thread

- From: Phase 11 prepared cloud-native concerns.
- This phase: implement the custom Dispatcher/LiteBus migration and pipeline behaviors for cross-cutting concerns.
- Next: Phase 13 adds MCP Server and AI integration.

Libraries (why they matter)

- LiteBus / custom dispatcher: purpose-built lightweight CQRS dispatcher used to explain mediator semantics without MediatR complexity.
- Scrutor: auto-registration of handlers for migration parity.

Clean Code & SOLID (teaching notes)

- Keep pipeline behaviors small and testable; prefer explicit decorators over magical global behaviors for teaching clarity.
- Ensure exception handling is centralized and consistent.

MediatR policy

- Phase 12 is the canonical migration phase away from MediatR toward the custom dispatcher. If any MediatR remains, mark it as legacy and add a migration task.

Notes

- Provide a migration checklist and scripts to replace `using MediatR` and package references safely.
