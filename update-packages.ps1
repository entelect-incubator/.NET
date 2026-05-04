param(
    [switch]$IncludePrerelease,
    [switch]$IncludeTransitive,
    [switch]$IgnoreFailedSources,
    [ValidateSet("auto", "solutions", "projects")]
    [string]$Scope = "auto"
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

function Get-SolutionFiles {
    Get-ChildItem -Path $PSScriptRoot -Recurse -File |
        Where-Object {
            ($_.Extension -in ".sln", ".slnx") -and
            ($_.FullName -notmatch "\\(bin|obj|\.git|node_modules)\\")
        }
}

function Get-CsProjFiles {
    Get-ChildItem -Path $PSScriptRoot -Filter "*.csproj" -Recurse -File |
        Where-Object { $_.FullName -notmatch "\\(bin|obj|\.git|node_modules)\\" }
}

function Update-Target([string]$targetPath) {
    $targetDir = Split-Path $targetPath -Parent
    $args = @(
        $targetPath,
        "--upgrade"
    )
    if ($IncludeTransitive) { $args += "--include-transitive" }
    if ($IncludePrerelease) { $args += "--pre-release" }
    if ($IgnoreFailedSources) { $args += "--ignore-failed-sources" }

    Write-Host "Updating packages for: $targetPath" -ForegroundColor Yellow
    Push-Location $targetDir
    try {
        dotnet outdated @args | Out-Host
    }
    finally {
        Pop-Location
    }
}

Ensure-DotNetOutdated

$solutions = Get-SolutionFiles
$projects = Get-CsProjFiles

$targets = switch ($Scope) {
    "solutions" { $solutions }
    "projects" { $projects }
    default {
        if ($solutions) { $solutions } else { $projects }
    }
}

if (-not $targets) {
    Write-Host "No solution or project files found under $PSScriptRoot" -ForegroundColor DarkYellow
    exit 0
}

$targets | Sort-Object FullName | ForEach-Object { Update-Target $_.FullName }

Write-Host "Package upgrade run complete (scope: $Scope)." -ForegroundColor Green
