Teaching Thread

- From: Phase 14 integrated external services and resilient patterns.
- This phase: containerize services, set up GHCR publishing and deployable artifacts.
- Next: roll-up and maintain the incubator content; provide maintenance and contribution guidance.

Libraries (why they matter)

- Docker multi-stage builds: produce small, secure images for publishing.
- GitHub Actions: CI/CD pipelines for build, test, and publish; document secrets and GHCR usage.

Clean Code & SOLID (teaching notes)

- Keep deployment scripts declarative and idempotent; document expected environment variables and secrets.
- Ensure observability pieces are enabled in production images (OTel, logs).

MediatR policy

- CI/CD and container publishing are orthogonal to MediatR. If MediatR remains anywhere, add a migration item to remove or document it.

Notes

- Include example workflows for build+publish and local image testing.
