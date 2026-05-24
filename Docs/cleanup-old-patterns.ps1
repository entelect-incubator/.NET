#!/usr/bin/env powershell
# Remove old ValidationBehavior.cs files from all phases
# These files are no longer needed with custom MediatorLite implementation

$dotNetRoot = "d:\Dev\Incubator\.NET"
$phases = 4..9
$DRY_RUN = $false  # Set to $false to actually delete

Write-Host "=== Removing ValidationBehavior.cs Files ===" -ForegroundColor Cyan
Write-Host "DRY-RUN MODE: $DRY_RUN" -ForegroundColor Yellow
Write-Host ""

$deletedCount = 0
$skippedCount = 0

foreach ($phase in $phases) {
    $phasePath = Join-Path $dotNetRoot "Phase $phase"
    
    if (-not (Test-Path $phasePath)) {
        continue
    }
    
    $validationBehaviorFiles = Get-ChildItem -Path $phasePath -Recurse -Filter "ValidationBehavior.cs" -ErrorAction SilentlyContinue
    
    foreach ($file in $validationBehaviorFiles) {
        if ($DRY_RUN) {
            Write-Host "[DRY-RUN] DELETE: $($file.FullName)" -ForegroundColor Yellow
            $deletedCount++
        } else {
            try {
                Remove-Item -Path $file.FullName -Force
                Write-Host "[DELETED] $($file.FullName)" -ForegroundColor Green
                $deletedCount++
            } catch {
                Write-Host "[ERROR] Failed to delete: $($file.FullName)" -ForegroundColor Red
                Write-Host "        $($_.Exception.Message)"
                $skippedCount++
            }
        }
    }
}

Write-Host ""
Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host "Files to delete: $deletedCount"
Write-Host "Errors: $skippedCount"
Write-Host ""

if ($DRY_RUN) {
    Write-Host "To execute deletion, edit script and set DRY_RUN to `$false" -ForegroundColor Green
} else {
    Write-Host "All ValidationBehavior.cs files have been deleted!" -ForegroundColor Green
}
