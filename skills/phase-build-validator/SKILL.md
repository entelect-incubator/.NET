# Skill: Phase Build Validator

Purpose
- Validate every solution in each phase step under Phase */src and fail fast in CI when any step stops building.

Usage
1. From repo root, run:
   - `pwsh ./scripts/Test-PhaseSolutions.ps1 -Configuration Release`
2. Optional local speed-up when dependencies are already restored:
   - `pwsh ./scripts/Test-PhaseSolutions.ps1 -Configuration Release -NoRestore`

What it validates
- Discovers all .sln and .slnx files under Phase */src.
- Restores and builds each solution.
- Prints a compact summary of any failed restores/builds.
- Returns exit code 1 when any validation fails.

CI integration
- Workflow: .github/workflows/dotnet-all-phases-validate.yml
- Triggered on push/pull_request to master for phase and script changes.
- Also supports manual workflow_dispatch.
