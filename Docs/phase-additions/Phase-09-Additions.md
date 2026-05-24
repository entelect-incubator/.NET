Teaching Thread

- From: Phase 8 focused on API contracts and client generation.
- This phase: build user interfaces (MVC, Razor, Blazor) and integrate with back-end APIs.
- Next: Phase 10 moves to DB migrations and infra concerns.

Libraries (why they matter)

- Choose UI frameworks based on learning goals: Razor for server-side rendering, Blazor for C#-centric frontends.
- Explain any library that simplifies integration (identity, auth clients).

Clean Code & SOLID (teaching notes)

- Keep presentation logic thin; controllers should orchestrate, views/components should be pure.
- Reuse DTOs/contracts from API client generation to avoid mapping drift.

MediatR policy

- If you see MediatR in UI/back-end coupling, document why. Prefer custom dispatcher for server-side consistency unless MediatR provides clear benefit here.

Notes

- Ensure README links to demo pages and explains how the UI uses the generated client.
