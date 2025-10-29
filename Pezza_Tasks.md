# Pezza / Pizza Tasks and Memory

This file tracks the high-level tasks, decisions, commits, and progress for restoring and upgrading the Pezza (pizza) apps.

## Tasks (from user request)

1. Restore Pezza app to last working version (before TODO changes).
2. Upgrade all .NET packages and target frameworks to .NET 10.
3. Review Pezza app for consistency and spelling; fix small issues.
4. Phase 8: Create a Blazor pizza-ordering app using Tailwind CSS.
5. Phase 8: Create a Pizza ordering MVC app using Tailwind CSS.

## Status
- 1-6: Todos created in internal task tracker.

## Plan / Next actions
- Locate the Pezza project path(s) in the repo and the commit history that contains the last working version.
- Use git to check out or revert to the commit just prior to the unwanted changes (verify tests/build).
- Update project TargetFramework to `net10.0` (or `net10`) and update NuGet packages using `dotnet list package --outdated` / `dotnet add package`.
- Run `dotnet build` and fix any compile/runtime issues.
- Run code spell checks and review naming; apply fixes in small PRs.
- Scaffold new Blazor and MVC projects in `Phase 8/src/` with Tailwind integration.

## Notes / Assumptions
- Assumes git history is intact and the repo is the working copy with remotes configured.
- Assumes .NET 10 SDK is installed on the machine that will perform the upgrade.

## Links / Found references (initial search results)
- `Phase 1/src/02. EndSolution/Pezza.Test/` (references found in build artifacts)
- `Phase 8/src/02. MVC/Test/Setup/TestData/Todos/TodoTestData.cs` (namespace includes `Pizza`) 

## Audit trail
- Created by automation on: 2025-10-28


---

Update this file as progress is made.
