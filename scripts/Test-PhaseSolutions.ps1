param(
    [string]$Configuration = "Release",
    [switch]$NoRestore
)

$ErrorActionPreference = "Stop"

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
Write-Host "Repository root: $repoRoot"

function Get-PhaseSortKey {
    param([string]$Name)

    if ($Name -match '^Phase\s+(\d+)$') {
        return [int]$Matches[1]
    }

    return [int]::MaxValue
}

function Get-RelativePath {
    param(
        [string]$BasePath,
        [string]$TargetPath
    )

    try {
        return [System.IO.Path]::GetRelativePath($BasePath, $TargetPath)
    }
    catch {
        $baseUri = New-Object System.Uri(($BasePath.TrimEnd('\\') + '\\'))
        $targetUri = New-Object System.Uri($TargetPath)
        $relativeUri = $baseUri.MakeRelativeUri($targetUri)
        return [System.Uri]::UnescapeDataString($relativeUri.ToString()).Replace('/', '\\')
    }
}

function Get-SolutionProjectPaths {
    param([string]$SolutionPath)

    $solutionDir = Split-Path $SolutionPath
    Get-Content $SolutionPath | ForEach-Object {
        if ($_ -match '<Project\s+Path="([^"]+)"') {
            Join-Path $solutionDir $Matches[1]
        }
    }
}

$phaseDirs = Get-ChildItem -Path $repoRoot -Directory |
    Where-Object { $_.Name -match '^Phase\s+\d+$' } |
    Sort-Object -Property @{ Expression = { Get-PhaseSortKey -Name $_.Name } }, Name

if (-not $phaseDirs) {
    throw "No phase directories were found under '$repoRoot'."
}

$solutions = @()

foreach ($phase in $phaseDirs) {
    $srcPath = Join-Path $phase.FullName "src"
    if (-not (Test-Path $srcPath)) {
        continue
    }

    $phaseSolutions = Get-ChildItem -Path $srcPath -Recurse -File |
        Where-Object { $_.Extension -in @('.sln', '.slnx') } |
        Group-Object DirectoryName |
        ForEach-Object {
            $slnx = $_.Group | Where-Object { $_.Extension -eq '.slnx' } | Select-Object -First 1
            if ($slnx) {
                $slnx
            }
            else {
                $_.Group | Where-Object { $_.Extension -eq '.sln' } | Select-Object -First 1
            }
        } |
        Sort-Object FullName

    $solutions += $phaseSolutions
}

if (-not $solutions) {
    throw "No .sln or .slnx files were found under any Phase */src directory."
}

Write-Host "Discovered $($solutions.Count) solutions to validate."

$failed = @()

foreach ($solution in $solutions) {
    $relativeSolution = Get-RelativePath -BasePath $repoRoot -TargetPath $solution.FullName
    Write-Host ""
    Write-Host "=== Validating: $relativeSolution ==="

    $referencedProjects = Get-SolutionProjectPaths -SolutionPath $solution.FullName
    $missingProjects = $referencedProjects | Where-Object { -not (Test-Path $_) }
    if ($missingProjects) {
        Write-Host "Skipping $relativeSolution because it references missing project file(s):"
        $missingProjects | ForEach-Object {
            $relativeMissing = Get-RelativePath -BasePath $repoRoot -TargetPath $_
            Write-Host " - $relativeMissing"
        }
        continue
    }

    if (-not $NoRestore) {
        & dotnet restore $solution.FullName
        if ($LASTEXITCODE -ne 0) {
            $failed += "RESTORE FAILED: $relativeSolution"
            continue
        }
    }

    $buildArgs = @(
        "build",
        $solution.FullName,
        "--configuration", $Configuration,
        "--nologo",
        "-v", "minimal"
    )

    if (-not $NoRestore) {
        $buildArgs += "--no-restore"
    }

    & dotnet @buildArgs
    if ($LASTEXITCODE -ne 0) {
        $failed += "BUILD FAILED: $relativeSolution"
    }
}

Write-Host ""
if ($failed.Count -gt 0) {
    Write-Host "Validation failed for $($failed.Count) solution(s):"
    $failed | ForEach-Object { Write-Host " - $_" }
    exit 1
}

Write-Host "All phase solutions built successfully."
