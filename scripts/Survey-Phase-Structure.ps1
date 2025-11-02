# Update all DependencyInjection.cs and comment out behavior files for phases 2-8

param(
    [int[]]$Phases = @(2,3,4,5,6,7,8)
)

$baseDir = "d:\Dev\Incubator\.NET"
$depInjFiles = 0
$behaviorFiles = 0

Write-Host "`n=== Updating Phase DependencyInjection and Behaviors ===" -ForegroundColor Cyan

foreach ($phaseNum in $Phases) {
    Write-Host "Phase $($phaseNum)..." -ForegroundColor Magenta
    $phaseDir = "$baseDir\Phase $phaseNum\src"
    
    if (-not (Test-Path $phaseDir)) {
        Write-Host "  Not found" -ForegroundColor Red
        continue
    }
    
    # Process all DependencyInjection.cs files
    $depInjFiles_Phase = Get-ChildItem -Path $phaseDir -Filter "DependencyInjection.cs" -Recurse | Measure-Object | Select-Object -ExpandProperty Count
    Write-Host "  Found $depInjFiles_Phase DependencyInjection.cs files" -ForegroundColor Gray
    $depInjFiles += $depInjFiles_Phase
    
    # Count behavior files
    $behaviorFiles_Phase = Get-ChildItem -Path $phaseDir -Filter "*Behavior*.cs" -Recurse | Measure-Object | Select-Object -ExpandProperty Count
    Write-Host "  Found $behaviorFiles_Phase Behavior files" -ForegroundColor Gray
    $behaviorFiles += $behaviorFiles_Phase
}

Write-Host "`nSummary:" -ForegroundColor Green
Write-Host "  DependencyInjection.cs files to update: $depInjFiles" -ForegroundColor Green
Write-Host "  Behavior files to comment out: $behaviorFiles" -ForegroundColor Green
Write-Host "`nNote: Manual updates needed due to variation in file locations and naming" -ForegroundColor Yellow
Write-Host "  Use Find/Replace in VS Code for consistent updates`n" -ForegroundColor Yellow
