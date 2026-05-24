# Check migration status for all phases
param()

$BaseRoot = "d:\Dev\Incubator\.NET"
$Phases = @(8, 7, 6, 5, 4, 3, 2, 1)

Write-Host "Checking migration status..." -ForegroundColor Cyan

foreach ($Phase in $Phases) {
    $PhaseDir = Join-Path $BaseRoot "Phase $Phase\src\01. StartSolution"
    $PhaseName = "Phase $Phase"
    
    if (-not (Test-Path $PhaseDir)) {
        continue
    }
    
    $MediatRCount = (Get-ChildItem $PhaseDir -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue | Select-String "using MediatR" | Measure-Object).Count
    $LiteBusCount = (Get-ChildItem $PhaseDir -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue | Select-String "using LiteBus" | Measure-Object).Count
    
    if ($LiteBusCount -gt 0) {
        Write-Host "OK $PhaseName - Migrated" -ForegroundColor Green
    }
    elseif ($MediatRCount -gt 0) {
        Write-Host "-> $PhaseName - Ready ($MediatRCount files)" -ForegroundColor Yellow
    }
}
