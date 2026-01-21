#!/usr/bin/env powershell
# Audit script to find old MediatR patterns
param(
    [Parameter(Mandatory=$false)]
    [string]$ReportPath = "MIGRATION_AUDIT.md"
)

$ErrorActionPreference = "Stop"
$dotNetRoot = "d:\Dev\Incubator\.NET"
$phases = 4..9

Write-Host "=== Migration Audit: MediatR vs Custom MediatorLite ===" -ForegroundColor Cyan
Write-Host ""

# Initialize counters
$validationBehaviorCount = 0
$performanceBehaviourCount = 0
$ipipelineCount = 0
$missingHandlerCount = 0

foreach ($phase in $phases) {
    $phasePath = Join-Path $dotNetRoot "Phase $phase"
    
    if (-not (Test-Path $phasePath)) {
        continue
    }
    
    Write-Host "Scanning Phase $phase..." -ForegroundColor Yellow
    
    # Find ValidationBehavior files
    $validationBehaviorFiles = Get-ChildItem -Path $phasePath -Recurse -Filter "ValidationBehavior.cs" -ErrorAction SilentlyContinue
    if ($validationBehaviorFiles) {
        $validationBehaviorCount += $validationBehaviorFiles.Count
        foreach ($file in $validationBehaviorFiles) {
            Write-Host "  REMOVE: ValidationBehavior.cs" -ForegroundColor Red
            Write-Host "          $($file.FullName)"
        }
    }
    
    # Find PerformanceBehaviour with IPipelineBehavior
    $perfFiles = Get-ChildItem -Path $phasePath -Recurse -Filter "PerformanceBehaviour.cs" -ErrorAction SilentlyContinue
    foreach ($file in $perfFiles) {
        $content = Get-Content -Path $file.FullName -Raw
        if ($content -match "IPipelineBehavior") {
            $performanceBehaviourCount++
            Write-Host "  UPDATE: PerformanceBehaviour.cs uses IPipelineBehavior" -ForegroundColor Yellow
            Write-Host "          $($file.FullName)"
        }
    }
    
    # Find IPipelineBehavior references
    $ipcReferences = Get-ChildItem -Path $phasePath -Recurse -Filter "*.cs" -ErrorAction SilentlyContinue |
        Select-String -Pattern "IPipelineBehavior" -ErrorAction SilentlyContinue
    
    if ($ipcReferences) {
        $ipipelineCount += $ipcReferences.Count
        foreach ($ref in $ipcReferences | Select-Object -First 3) {
            Write-Host "  CHECK: IPipelineBehavior reference" -ForegroundColor Cyan
            Write-Host "         $($ref.Path):$($ref.LineNumber)"
        }
        if ($ipcReferences.Count -gt 3) {
            Write-Host "         ... and $($ipcReferences.Count - 3) more references"
        }
    }
}

Write-Host ""
Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host "ValidationBehavior files found: $validationBehaviorCount (REMOVE)"
Write-Host "PerformanceBehaviour issues: $performanceBehaviourCount (UPDATE)"
Write-Host "IPipelineBehavior references: $ipipelineCount (CHECK)"
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Green
Write-Host "1. Delete all ValidationBehavior.cs files"
Write-Host "2. Update PerformanceBehaviour files to use GlobalExceptionHandler"
Write-Host "3. Review and remove all IPipelineBehavior registrations in DependencyInjection.cs"
Write-Host "4. Ensure GlobalExceptionHandler is implemented in each phase"
Write-Host ""
