param(
    [switch]$IncludePrerelease,
    [switch]$IncludeTransitive,
    [switch]$IgnoreFailedSources
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Ensure-DotNetOutdated {
    $toolName = "dotnet-outdated-tool"
    $installed = dotnet tool list -g | Select-String -Quiet $toolName
    if (-not $installed) {
        Write-Host "Installing $toolName ..." -ForegroundColor Cyan
        dotnet tool install -g $toolName | Out-Host
    }
}

function Get-CsProjFiles {
    Get-ChildItem -Path $PSScriptRoot -Filter "*.csproj" -Recurse -File |
        Where-Object { $_.FullName -notmatch "\\(bin|obj)\\" }
}

function Update-Project([string]$projectPath) {
    $projDir = Split-Path $projectPath -Parent
    $args = @(
        "--upgrade",
        "--parallel"
    )
    if ($IncludeTransitive) { $args += "--include-transitive" }
    if ($IncludePrerelease) { $args += "--pre-release" }
    if ($IgnoreFailedSources) { $args += "--ignore-failed-sources" }

    Write-Host "Updating packages for: $projectPath" -ForegroundColor Yellow
    Push-Location $projDir
    try {
        dotnet outdated @args | Out-Host
    }
    finally {
        Pop-Location
    }
}

Ensure-DotNetOutdated
$projects = Get-CsProjFiles

if (-not $projects) {
    Write-Host "No .csproj files found under $PSScriptRoot" -ForegroundColor DarkYellow
    exit 0
}

$projects | ForEach-Object { Update-Project $_.FullName }

Write-Host "Package upgrade run complete." -ForegroundColor Green
