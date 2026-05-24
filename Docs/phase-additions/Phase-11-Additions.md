Teaching Thread

- From: Phase 10 established schema migrations and infra practices.
- This phase: prepare services for cloud-native orchestration, observability and containerization.
- Next: Phase 12 focuses on the custom dispatcher migration and pipeline behaviors.

Libraries (why they matter)

- OpenTelemetry / otel-collector: observability and tracing — teach how to instrument handlers and request pipelines.
- Docker / GitHub Actions: CI and container publish workflows.

Clean Code & SOLID (teaching notes)

- Make services observable: bubble trace IDs through dispatcher and handlers.
- Use dependency injection and small abstractions to allow swapping infra components in production.

MediatR policy

- Cloud orchestration does not require MediatR. If MediatR is present for historical reasons, add a note explaining its intended production value and migration plans.

Notes

- Provide commands for local dev docker-compose and GHCR publishing examples.
