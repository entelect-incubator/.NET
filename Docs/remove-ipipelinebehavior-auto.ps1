#!/usr/bin/env powershell
# Automatically remove IPipelineBehavior registrations from DependencyInjection.cs files
# Creates backups before modifying

$dotNetRoot = "d:\Dev\Incubator\.NET"
$phases = 4..9
$DRY_RUN = $true  # Set to $false to actually modify files

Write-Host "=== Removing IPipelineBehavior Lines ===" -ForegroundColor Cyan
Write-Host "DRY-RUN MODE: $DRY_RUN" -ForegroundColor Yellow
Write-Host ""

$modifiedCount = 0
$skippedCount = 0

$depInjectionFiles = @(
    "D:\Dev\Incubator\.NET\Phase 4\src\02. Step1\Core\DependencyInjection.cs"
    "D:\Dev\Incubator\.NET\Phase 4\src\03. Step2\Core\DependencyInjection.cs"
    "D:\Dev\Incubator\.NET\Phase 6\src\02. Step 1\Core\DependencyInjection.cs"
    "D:\Dev\Incubator\.NET\Phase 6\src\03. Step 2\Core\DependencyInjection.cs"
    "D:\Dev\Incubator\.NET\Phase 7\src\02. Step 1\Core\DependencyInjection.cs"
    "D:\Dev\Incubator\.NET\Phase 7\src\03. Step 2\Core\DependencyInjection.cs"
    "D:\Dev\Incubator\.NET\Phase 7\src\04. Step 3\Core\DependencyInjection.cs"
    "D:\Dev\Incubator\.NET\Phase 7\src\04. Step 4\Core\DependencyInjection.cs"
    "D:\Dev\Incubator\.NET\Phase 8\src\02. EndSolution\Core\DependencyInjection.cs"
    "D:\Dev\Incubator\.NET\Phase 9\src\01. StartSolution\Core\DependencyInjection.cs"
    "D:\Dev\Incubator\.NET\Phase 9\src\02. FinalSolution\Core\DependencyInjection.cs"
)

foreach ($filePath in $depInjectionFiles) {
    if (-not (Test-Path $filePath)) {
        Write-Host "SKIP: File not found: $filePath" -ForegroundColor Gray
        continue
    }
    
    $content = Get-Content -Path $filePath -Raw
    
    # Check if file has IPipelineBehavior
    if (-not ($content -match "IPipelineBehavior")) {
        continue
    }
    
    Write-Host "Processing: $filePath" -ForegroundColor Yellow
    
    # Remove lines with IPipelineBehavior registrations (both commented and uncommented)
    $newContent = $content -replace "^\s*////?\s*services\.AddTransient\(typeof\(IPipelineBehavior.*\n", ""
    $newContent = $newContent -replace "^\s*services\.AddTransient\(typeof\(IPipelineBehavior.*\n", ""
    
    # Check if anything changed
    if ($newContent -ne $content) {
        if ($DRY_RUN) {
            Write-Host "  [DRY-RUN] Would remove IPipelineBehavior lines" -ForegroundColor Cyan
            $modifiedCount++
        } else {
            # Create backup
            $backupPath = "$filePath.backup"
            Copy-Item -Path $filePath -Destination $backupPath -Force
            Write-Host "  [BACKUP] $backupPath" -ForegroundColor Green
            
            # Write modified content
            $newContent | Out-File -FilePath $filePath -Encoding UTF8 -Force
            Write-Host "  [MODIFIED] IPipelineBehavior lines removed" -ForegroundColor Green
            $modifiedCount++
        }
    } else {
        Write-Host "  [NO CHANGE] No IPipelineBehavior patterns matched" -ForegroundColor Gray
        $skippedCount++
    }
}

Write-Host ""
Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host "Files modified: $modifiedCount"
Write-Host "Files skipped: $skippedCount"
Write-Host ""

if ($DRY_RUN) {
    Write-Host "To execute changes, edit script and set DRY_RUN to `$false" -ForegroundColor Green
    Write-Host ""
    Write-Host "IMPORTANT: After removal, you MUST add GlobalExceptionHandler to each phase:" -ForegroundColor Red
    Write-Host "  1. Create Api/Handlers/GlobalExceptionHandler.cs"
    Write-Host "  2. Add to DependencyInjection: services.AddExceptionHandler<GlobalExceptionHandler>();"
    Write-Host "  3. Add to Startup: app.UseExceptionHandler();"
} else {
    Write-Host "IPipelineBehavior lines removed from all files!" -ForegroundColor Green
    Write-Host ""
    Write-Host "NEXT: Add GlobalExceptionHandler to each phase" -ForegroundColor Red
}
