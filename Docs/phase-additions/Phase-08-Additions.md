Teaching Thread

- From: Phase 7 produced API clients and clarified service contracts.
- This phase: focus on OpenAPI, NSwag and reliable client generation.
- Next: Phase 9 shows UI integration and user-facing concerns.

Libraries (why they matter)

- NSwag / Swashbuckle: generate OpenAPI documents and language-specific clients — teaches contract-driven development.
- Newtonsoft.Json vs System.Text.Json: explain choices and serialization trade-offs for generated clients.

Clean Code & SOLID (teaching notes)

- Treat generated client code as a separate artifact—do not hand-edit generated files; layer wrappers if you need to adapt behavior.
- Keep API contracts stable and version them when breaking changes are introduced.

MediatR policy

- NSwag and OpenAPI are orthogonal to dispatcher choices; do not introduce MediatR solely for client generation.

Notes

- Add quick commands to regenerate clients in CI and document expected output locations.
