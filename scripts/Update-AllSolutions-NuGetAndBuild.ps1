param(
    [switch]$IncludePrerelease,
    [switch]$IncludeTransitive,
    [switch]$IgnoreFailedSources,
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Resolve-Path (Join-Path $scriptRoot "..")
$nugetUpdateScript = Join-Path $scriptRoot "Update-AllSolutions-NuGet.ps1"

if (-not (Test-Path $nugetUpdateScript)) {
    throw "Could not find required script: $nugetUpdateScript"
}

Write-Host "[1/2] Updating NuGet packages for all solutions..." -ForegroundColor Cyan
& $nugetUpdateScript -IncludePrerelease:$IncludePrerelease -IncludeTransitive:$IncludeTransitive -IgnoreFailedSources:$IgnoreFailedSources

Write-Host "[2/2] Building all solution files..." -ForegroundColor Cyan

$solutionFiles = Get-ChildItem -Path $repoRoot -Recurse -File |
    Where-Object {
        ($_.Extension -in ".sln", ".slnx") -and
        ($_.FullName -notmatch "\\(bin|obj|\.git|node_modules|\.venv)\\")
    } |
    Sort-Object FullName

if (-not $solutionFiles) {
    Write-Host "No .sln or .slnx files found under $repoRoot" -ForegroundColor DarkYellow
    exit 0
}

$failures = @()
foreach ($solution in $solutionFiles) {
    Write-Host "Building: $($solution.FullName)" -ForegroundColor Yellow
    dotnet build "$($solution.FullName)" -c $Configuration --nologo --verbosity minimal
    if ($LASTEXITCODE -ne 0) {
        $failures += $solution.FullName
    }
}

if ($failures.Count -gt 0) {
    Write-Host "Build failed for the following solutions:" -ForegroundColor Red
    $failures | ForEach-Object { Write-Host " - $_" -ForegroundColor Red }
    exit 1
}

Write-Host "All solution package updates and builds completed successfully." -ForegroundColor Green
