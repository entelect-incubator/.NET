param(
    [switch]$IncludePrerelease,
    [switch]$IncludeTransitive,
    [switch]$IgnoreFailedSources
)

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Resolve-Path (Join-Path $scriptRoot "..")
$mainScript = Join-Path $repoRoot "update-packages.ps1"

if (-not (Test-Path $mainScript)) {
    throw "Could not find update-packages.ps1 at $mainScript"
}

& $mainScript -Scope solutions -IncludePrerelease:$IncludePrerelease -IncludeTransitive:$IncludeTransitive -IgnoreFailedSources:$IgnoreFailedSources
