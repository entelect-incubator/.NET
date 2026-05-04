# .NET Phases Audit Report

Generated: 2026-01-21

Summary
- Goal: audit all Phase READMEs and source for:
  - Presence of 'why / how / outcome / steps' in README
  - Correlation of README steps to `src/` code
  - Use of Mediator libraries (MediatR) vs. custom Dispatcher (MediatorLite/LiteBus)
  - External libraries with missing justification
  - Design-pattern docs that should live in `Design-Patterns/`

Top-level findings (high level)
- The repo intentionally moves toward a small, custom dispatcher (`MediatorLite` / `LiteBus`) for teaching CQRS and avoiding MediatR pipelines.
- However, many phases still contain `using MediatR;` or `<PackageReference Include="MediatR" .../>` in code and csproj files. Notable phases with MediatR package references and code: Phase 9, Phase 11, Phase 12, Phase 13 (see Evidence below).
- There are migration scripts and docs present (`.NET/scripts/*`, `Migrate-MediatR-To-LiteBus*.ps1`) indicating an in-flight or partial migration away from MediatR.
- Several READMEs (per-phase) already document the intent to use a custom dispatcher; a few README files still mention MediatR or provide MediatR installation examples (templates).
- Design patterns (Dispatcher/Mediator, SOLID, Result pattern) are already documented in `Design-Patterns/` and should remain authoritative there; phase READMEs should link to those pattern docs rather than rehosting full pattern guides.

Evidence (representative)
- CSProj package references (examples):
  - Phase 11/src/02. FinalSolution/Pezza.Core/Pezza.Core.csproj — contains `PackageReference Include="MediatR" Version="11.0.0"`
  - Phase 9/src/02. FinalSolution/Core/Core.csproj — contains `PackageReference Include="MediatR" Version="11.0.0"`
  - Phase 12 & Phase 13 similar package references in `Pezza.Core` and scheduler/back-end projects.
- Code files with `using MediatR;` appear across Phase 9..13 and in scheduler jobs (Phase 8/9 Scheduler Job examples) and `Common/Behaviours/*` (PerformanceBehaviour, ValidationBehavior).
- Migration tooling exists: `.NET/scripts/Migrate-MediatR-To-LiteBus-*.ps1` and `audit-migration-patterns.ps1`.

Main issues identified
1. Inconsistent messaging: many READMEs claim "no MediatR — custom dispatcher" while some code/csproj still reference MediatR. This confuses learners.
2. Missing library rationale: several NuGet packages are present in projects without a short README justification explaining why they were chosen and what learners gain from them.
3. Teaching flow: some phase READMEs include detailed pattern explanations that duplicate content in `Design-Patterns/`; these should link to the canonical pattern repo and tell the learning story (why/how/outcome) instead of copy-paste.
4. Cleanup: migration scripts exist but the codebase appears only partially migrated (leftover `using MediatR` and behaviors). Either finish migration or document why MediatR remains for those targets.

Recommendations (actionable)
1. Per-phase canonical audit: produce a per-phase checklist (README present? why/how/outcome? steps map to code? MediatR present?) — I can generate these automatically.
2. Decide a policy: enforce either (A) fully remove MediatR and finish migration to custom dispatcher (LiteBus/MediatorLite) OR (B) document explicit exceptions where MediatR is intentionally used and why. I recommend A for consistency with teaching goals.
3. For each external library used (NSwag, LiteBus, Dapper/EF etc.), add a one-paragraph justification in the phase README under a `Libraries` or `Why this library` section.
4. Move full pattern docs to `Design-Patterns/` and replace pattern blocks in phase READMEs with concise context plus links to the canonical pattern pages. Keep a short "Teaching Story" in each phase README showing the learning progression.
5. Add a root-level `AI + Copilot` section to the main README modeled after "Awesome GitHub Copilot" (agents, prompts, instructions, skills, collections) — I can draft that and open a PR.
6. Add a `CLEANUP` task list and optionally run the migration scripts to remove leftover MediatR references (I can run and apply fixes, but will request confirmation before editing code).

Next steps I can take now (pick or confirm):
- Generate a detailed per-phase audit markdown (`.NET/Docs/phase-audits/Phase-1.md` ... `Phase-15.md`) mapping README sections to code locations and listing MediatR occurrences. (recommended)
- Produce a suggested patch for each phase README to: (a) add `Libraries` justification bullets, (b) replace pattern duplication with links to `Design-Patterns/`, (c) add a 3-line SOLID/clean-code teaching note.
- Draft the root README AI/Copilot integration section following the "Awesome Copilot" structure.
- Optionally run the migration scripts to remove MediatR and replace with LiteBus usages across phases, then run `dotnet build` for sanity. (I will not modify code without your approval.)

Where I focused the automated scan
- Searched for `using MediatR`, `PackageReference Include="MediatR"`, `Mediator.Send`, readme mentions of "Mediator" and pattern docs.

Files I created/updated
- Created this file: [.NET/Docs/PHASES_AUDIT_REPORT.md](.NET/Docs/PHASES_AUDIT_REPORT.md)

Questions / confirmation
- Shall I proceed to generate the per-phase detailed audit files and then prepare README patches automatically? Or would you prefer I start by drafting the root README AI/Copilot addition and one exemplar phase README patch for review first?

--
Audit run by assistant. If you want, I can now (A) generate per-phase audit files, (B) create example README patches for Phase 3 and Phase 9 (one custom-dispatch example and one MediatR-cleanup example), or (C) draft the main README AI/Copilot integration content immediately.