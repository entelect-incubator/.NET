<!--
  Memory / Documentation guidelines for the .NET Incubator (Pezza)
  - Purpose: single source for reviewer guidance, AI prompts, checklists, templates
  - Location: Docs/Memory/README.md
-->

# Documentation Memory: .NET Incubator (Pezza)

This file collects documentation best-practices, reviewer checklists, AI/automation prompts, and templates to keep the incubator docs aligned with code and consistent across phases.

Use this as the canonical checklist when you update phase READMEs or create new markdown docs.

## Goals

- Give maintainers and reviewers a short, repeatable checklist to verify docs against code.
- Provide AI prompt templates (for PR helpers or local assistants) to enforce SOLID, Clean Architecture and layered-architecture recommendations when reviewing code and docs.
- Offer a README template for each Phase that includes time estimates, difficulty, prerequisites, and a copy-paste-ready code-snippet validation checklist.

## Quick summary: recommended Estimated Times & Difficulty

These are rough estimates for an individual learner. Adjust for team-based or mentor-led sessions.

- Phase 1 — Getting started (scaffold a clean architecture solution): 3–6 hours — Difficulty: ★★☆☆☆
- Phase 2 — Scaffolding & CRUD, CQRS: 4–8 hours — Difficulty: ★★★☆☆
- Phase 3 — Validation, pagination, EF Core: 3–6 hours — Difficulty: ★★★☆☆
- Phase 4 — Coding standards & error handling: 2–4 hours — Difficulty: ★☆☆☆☆
- Phase 5 — Performance (caching, compression): 3–6 hours — Difficulty: ★★★☆☆
- Phase 6 — Events & background jobs: 3–6 hours — Difficulty: ★★★☆☆
- Phase 7 — Microservices & API clients: 6–12 hours — Difficulty: ★★★★☆
- Phase 8 — Security (JWT, antiforgery, secrets): 4–8 hours — Difficulty: ★★★★☆
- Phase 9 — UI (MVC/Blazor): 6–12 hours — Difficulty: ★★★★☆
- Phase 10 — Recommended libraries & extras: 1–3 hours — Difficulty: ★★☆☆☆

Notes: these assume the learner is familiar with general programming concepts. A complete beginner will need more time. Senior engineers may finish faster but should use the time to add tests and polish architecture.

## Phase README template

When creating or editing a Phase README, use this minimal template (copy into the phase README):

- Title: short name
- Purpose: 1–2 sentences describing the learning outcome(s)
- Prerequisites: list of knowledge / installed tools (exact versions)
- Estimated time: X hours (range)
- Difficulty: 1–5 stars (brief justification)
- Step-by-step tasks: numbered steps with code snippets that are copy/paste-ready
- Validation: commands to run (e.g., `dotnet build`, `dotnet test`), and expected outputs
- API/code links: exact relative paths to the projects referenced (e.g., `Phase 1/src/01. StartSolution/`)
- Notes & further reading: links & references

Always include the SDK version required at the top (e.g., `Requires .NET SDK: 10.0.x`).

## Doc-to-code alignment checklist (reviewer)

Run these checks every time you change examples or publish a phase:

1. Build: run `dotnet build` in the solution referenced by the phase and ensure it succeeds.
2. Tests: run `dotnet test` for the projects included in the phase.
3. Snippet compile: copy any code sample in the README into a small scratch project and confirm it compiles (or add a CI job to do this automatically).
4. Path verification: ensure file paths in docs match the repository (case-insensitive on Windows, but keep consistent naming).
5. Versioning: confirm SDK and NuGet package versions used in docs match the project files (csproj). If docs reference an older .NET version, update or mark as intentional.
6. Spelling & wording: run a spell-checker (e.g., codespell, Vale, or a GitHub Action) against the repo; check for common typos: "Prerequirements" -> "Prerequisites", "Dataccess" -> "DataAccess", "consitency" -> "consistency".
7. Terminology: ensure terms are consistent across docs (use "Clean Architecture" and "layered architecture" definitions consistently).

## AI + GitHub review helper: prompt templates

Use the following prompt when asking an LLM (locally or in CI) to review a PR or a set of files. Provide the repository (or file excerpt) and this Memory README as context.

LLM prompt (short):

"You are a senior C#/.NET engineer. Review the changes in these files for: correctness, SOLID/principles, layered/clean-architecture alignment, spelling/wording, and doc-code consistency. For each issue, produce (1) a short description, (2) severity (low/medium/high), (3) suggested fix (code or text), (4) an exact patch (if possible). Use the Memory README (`Docs/Memory/README.md`) as the ruleset."

LLM review focus areas (checklist for the prompt):

- SOLID violations (single responsibility, open/closed, LSP, ISP, DIP)
- Clean Architecture: separation of Core/Domain, Application, Infrastructure, Presentation
- Layered responsibilities: avoid business logic in controllers, avoid EF Core usage leaking into services
- Error handling: centralized error handling and consistent error responses
- Tests: ensure behaviour is covered by unit/integ tests where appropriate
- Spelling/wording and accurate technical terms

GitHub Actions automation idea:

- Add a workflow that triggers on PRs with a job to run `dotnet build`, `dotnet test`, and a docspell/markdown lint.
- Optionally call an LLM-runner action that uses the LLM prompt above to create an automated 'review' comment summarizing issues and suggested changes.

## SOLID & Clean Architecture quick rules (for reviewers and mentors)

- Single Responsibility: each class should have exactly one reason to change. If a class manages data mapping and business decisions, split it.
- Open/Closed: prefer extension via interfaces/DI rather than editing existing classes for new behaviour.
- Liskov Substitution: derived classes must be substitutable for their base types; watch for unexpected exceptions or state changes.
- Interface Segregation: smaller, specific interfaces are better than large monolith interfaces.
- Dependency Inversion: depend on abstractions (interfaces) in higher layers; inject concrete implementations in composition root (startup/Host).

Clean Architecture / Layered guidance:

- Core/Domain: entities, domain exceptions, domain services, and interfaces. No framework dependencies (no EF Core types here).
- Application (Core.Contracts / Use Cases): DTOs, commands/queries (if using CQRS), interfaces that the infrastructure will implement.
- Infrastructure (DataAccess): EF Core, repository implementations, mappings. This layer can reference the framework and persistence libraries.
- Presentation (Api / MVC / UI): controllers, endpoints, views. This layer depends on Application abstractions only.

Example anti-pattern to watch for: `Controller -> new DbContext()` or `Controller -> new Repository()` instead of using injected interfaces.

## Spelling & wording issues observed (examples & recommended fixes)

I performed a quick pass on several key docs and found a number of recurring issues. Below are representative examples and recommended actions.

- "Prerequirements.md" — non-standard; rename to `Prerequisites.md` and update all links.
- "Dataccess" / "Dataccess" project name inconsistencies — pick `DataAccess` and update csproj and docs where possible.
- Typos and grammar: "mffethods", "consitency", "Ctr+K" (should be Ctrl+K), "ProperyGroup" -> "PropertyGroup".
- Outdated references: Some docs mention older .NET versions (e.g., .NET 3.1 / .NET 5) — update to current target (.NET 10) or mark historical context.

Suggested bulk actions:

1. Run a spell-check tool (codespell or Vale) and produce a PR with the fixes. Keep those changes focused and small.
2. Replace `Prerequirements.md` with `Prerequisites.md` and add redirects or update all links.
3. Add a small CI job that runs `dotnet build` and `dotnet test` on the main example solutions for each phase.

## Docs CI suggestions (quick wins)

- Add a GitHub Action workflow `docs/validate.yml` that:
  - checks for broken links (markdown-link-check)
  - runs a spelling linter
  - runs `dotnet build` in each `Phase X/src/` solution and fails PR if build fails

- Add a small script (PowerShell) `scripts/validate-phase.ps1` that:
  - accepts a phase directory and runs `dotnet build` and `dotnet test` for the solution(s) found.

## Example reviewer checklist (to add to PR template)

1. Did `dotnet build` succeed for the affected projects?
2. Do code examples in the README compile/run as-is?
3. Are all file/project paths correct and case-consistent?
4. Any new code: are there unit tests or TODOs tracking tests?
5. Docs: run spellcheck; fix high-severity typos.
6. Architecture: does the change keep layers separated? If not, why?

## Actionable next steps I can take now (pick one):

1. Create `Docs/Memory/README.md` (this file) — done.
2. Produce a PR that auto-fixes high-frequency typos (small, safe changes).
3. Add a basic GitHub Action to run `dotnet build` and a spell-check on PRs.
4. Run a deeper pass across all Phase README files and propose concrete edits per file (one PR per phase recommended).

If you want, I can start with creating the automated spellcheck + docs CI job (option 3) or open a PR fixing the most frequent typos (option 2). Tell me which to do next.

---

Generated: automatic review starter — update this file as the canonical memory for doc/code alignment.
