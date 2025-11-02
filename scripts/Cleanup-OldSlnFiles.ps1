#!/usr/bin/env pwsh
param(
    [int[]]$Phases = @(2, 3, 5, 6, 7, 8)
)

Write-Host "Removing old .sln files (keeping .slnx)..." -ForegroundColor Cyan
Write-Host ""

foreach ($phaseNum in $Phases) {
    $startSolDir = "d:\Dev\Incubator\.NET\Phase $phaseNum\src\01. StartSolution"
    
    if (-not (Test-Path $startSolDir)) {
        Write-Host "Phase $phaseNum not found" -ForegroundColor Yellow
        continue
    }
    
    $oldSln = Get-ChildItem -Path $startSolDir -File -Filter "*.sln" | Where-Object { $_.Name -notmatch "\.slnx" }
    
    if ($oldSln) {
        Remove-Item -Path $oldSln.FullName -Force
        Write-Host "Phase $phaseNum - Deleted $($oldSln.Name)" -ForegroundColor Green
    } else {
        Write-Host "Phase $phaseNum - No old .sln found" -ForegroundColor Gray
    }
}

Write-Host ""
Write-Host "Done!" -ForegroundColor Green
