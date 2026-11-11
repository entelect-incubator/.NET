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
    $files = Get-ChildItem -Path $PSScriptRoot -Recurse -File |
        Where-Object {
            ($_.Extension -in ".sln", ".slnx") -and
            ($_.FullName -notmatch "\\(bin|obj|\.git|node_modules)\\")
        }

    foreach ($file in $files) {
        $content = Get-Content $file.FullName -Raw
        $projectPaths = if ($file.Extension -eq ".slnx") {
            [regex]::Matches($content, '<Project\s+Path="([^"]+)"') |
                ForEach-Object { $_.Groups[1].Value }
        }
        else {
            [regex]::Matches($content, 'Project\("[^"]+"\)\s*=\s*"[^"]+",\s*"([^"]+)"') |
                ForEach-Object { $_.Groups[1].Value }
        }

        if (-not $projectPaths) {
            Write-Host "Skipping solution with no projects: $($file.FullName)" -ForegroundColor DarkYellow
            continue
        }

        $missingProjects = @(
            $projectPaths | Where-Object {
                $projectPath = Join-Path $file.DirectoryName ($_.Replace('/', '\'))
                -not (Test-Path $projectPath)
            }
        )
        if ($missingProjects) {
            Write-Host "Skipping solution with missing projects: $($file.FullName)" -ForegroundColor DarkYellow
            continue
        }

        $file
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
        "--upgrade",
        "--exclude",
        "StyleCop.Analyzers"
    )
    if ($IncludeTransitive) { $args += "--include-transitive" }
    if ($IncludePrerelease) { $args += "--pre-release" }
    if ($IgnoreFailedSources) { $args += "--ignore-failed-sources" }

    Write-Host "Updating packages for: $targetPath" -ForegroundColor Yellow
    Push-Location $targetDir
    try {
        dotnet restore $targetPath --nologo | Out-Host
        if ($LASTEXITCODE -ne 0) {
            throw "dotnet restore failed for $targetPath"
        }

        $args += "--no-restore"
        dotnet outdated @args | Out-Host
        if ($LASTEXITCODE -ne 0) {
            throw "dotnet outdated failed for $targetPath"
        }
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
