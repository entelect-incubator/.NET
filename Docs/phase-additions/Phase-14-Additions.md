Teaching Thread

- From: Phase 13 integrated AI capabilities.
- This phase: implement external API integration (webhooks, resilient calls, retry policies).
- Next: Phase 15 covers CI/CD and container publishing.

Libraries (why they matter)

- Polly: resilience and retry policies — document why to choose it and example policies.
- Typed HttpClient: demonstrate typed clients and testable integration layers.

Clean Code & SOLID (teaching notes)

- Keep external API adapters thin and testable; wrap retries at the adapter boundary and avoid leaking retries into business logic.

MediatR policy

- External integration should not rely on MediatR; if used, document the choice and where MediatR pipelines provide value.

Notes

- Add examples for webhook receivers and idempotency handling.
