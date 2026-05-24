Teaching Thread

- From: Phase 9 delivered UI and client integration.
- This phase: implement database migrations with DbUp and ensure schema versioning.
- Next: Phase 11 focuses on cloud-native orchestration and CI/CD.

Libraries (why they matter)

- DbUp: simple, code-driven DB migrations and roll-forward scripts — teaches schema versioning and idempotent migrations.
- Add notes on why choosing DbUp vs EF Migrations matters (team workflow, SQL control).

Clean Code & SOLID (teaching notes)

- Treat migration scripts as code: include them in source control, review, and test in staging.
- Keep migration responsibilities separated from application startup where possible.

MediatR policy

- Migrations are independent of dispatcher choices; do not conflate migration strategy with MediatR usage.

Notes

- Include commands to run migrations locally and in CI pipelines.
