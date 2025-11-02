#!/usr/bin/env pwsh
# Multi-Phase LiteBus Migration - Automated Execution
# Migrates phases 8, 7, 6, 5, 4, 3, 2, 1 from MediatR to LiteBus
# Usage: .\Execute-Phase-Migrations.ps1 -Phases 8,7,6,5,4,3,2,1

param(
    [int[]]$Phases = @(8,7,6,5,4,3,2,1),
    [switch]$SkipBackup,
    [switch]$SkipBuild,
    [switch]$DryRun
)

$BaseRoot = "d:\Dev\Incubator\.NET"
$MigrationStartTime = Get-Date

Write-Host "`n" + ("="*100) -ForegroundColor Cyan
Write-Host "LITEBUS MIGRATION - AUTOMATED EXECUTION (Phases: $($Phases -join ', '))" -ForegroundColor Cyan
Write-Host ("="*100) -ForegroundColor Cyan
Write-Host "Start Time: $MigrationStartTime`n" -ForegroundColor White

$Results = @()

foreach ($PhaseNum in $Phases) {
    $PhasePath = "$BaseRoot\Phase $PhaseNum\src\01. StartSolution"
    $PhaseName = "Phase $PhaseNum"
    
    if (-not (Test-Path $PhasePath)) {
        Write-Host "[$PhaseName] SKIP - Path not found" -ForegroundColor Gray
        continue
    }
    
    Write-Host "`n" + ("-"*100) -ForegroundColor Yellow
    Write-Host "MIGRATING: $PhaseName" -ForegroundColor Yellow
    Write-Host ("-"*100) -ForegroundColor Yellow
    
    try {
        # Pre-flight checks
        $CsFiles = @(Get-ChildItem $PhasePath -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue)
        $MediatRFiles = @($CsFiles | Select-String -Pattern "using MediatR" | ForEach-Object { $_.Path } | Get-Unique)
        $LiteBusFiles = @($CsFiles | Select-String -Pattern "using LiteBus" | ForEach-Object { $_.Path } | Get-Unique)
        
        if ($LiteBusFiles.Count -gt 0) {
            Write-Host "[$PhaseName] SKIP - Already migrated ($($LiteBusFiles.Count) LiteBus files found)" -ForegroundColor Green
            $Results += @{ Phase = $PhaseName; Status = "ALREADY_MIGRATED"; Count = $LiteBusFiles.Count }
            continue
        }
        
        if ($MediatRFiles.Count -eq 0) {
            Write-Host "[$PhaseName] SKIP - No MediatR references found" -ForegroundColor Yellow
            $Results += @{ Phase = $PhaseName; Status = "NO_MEDIATR"; Count = 0 }
            continue
        }
        
        Write-Host "  Files to migrate: $($MediatRFiles.Count)" -ForegroundColor Cyan
        
        if ($DryRun) {
            Write-Host "  [DRY RUN] Skipping actual changes" -ForegroundColor Yellow
            $Results += @{ Phase = $PhaseName; Status = "DRY_RUN"; Count = $MediatRFiles.Count }
            continue
        }
        
        # Backup if needed
        if (-not $SkipBackup) {
            $BackupPath = "$BaseRoot\Phase $PhaseNum\src\BACKUP-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
            Write-Host "  Backing up to: $BackupPath..." -ForegroundColor Cyan
            Copy-Item "$PhasePath\01. StartSolution" $BackupPath -Recurse -ErrorAction SilentlyContinue | Out-Null
            Write-Host "  Backup complete" -ForegroundColor Green
        }
        
        # Execute migration in phase directory
        Push-Location $PhasePath
        
        # Phase 1: Global MediatR -> LiteBus replacements
        Write-Host "  [1/4] Applying global replacements..." -ForegroundColor Cyan
        $ReplacementCount = 0
        
        foreach ($File in $CsFiles) {
            $Content = Get-Content $File.FullName -Raw -ErrorAction SilentlyContinue
            if (-not $Content) { continue }
            
            $OriginalLength = $Content.Length
            
            # Apply replacements
            $Content = $Content -replace "using MediatR;", ""
            $Content = $Content -replace "IRequest<", "ICommand<"  # Default to Command
            $Content = $Content -replace "IRequestHandler<", "ICommandHandler<"  # Default to Command
            $Content = $Content -replace "public (?:async )?Task<([^>]+)> Handle\(", 'public async Task<$1> HandleAsync('
            
            if ($Content.Length -ne $OriginalLength) {
                Set-Content $File.FullName -Value $Content -NoNewline -ErrorAction SilentlyContinue
                $ReplacementCount++
            }
        }
        
        Write-Host "  [1/4] Updated $ReplacementCount files" -ForegroundColor Green
        
        # Phase 2: Add LiteBus using statements to GlobalUsings
        Write-Host "  [2/4] Updating GlobalUsings.cs..." -ForegroundColor Cyan
        $GlobalUsingsPath = "GlobalUsings.cs"
        if (Test-Path $GlobalUsingsPath) {
            $Content = Get-Content $GlobalUsingsPath -Raw
            if ($Content -notmatch "LiteBus") {
                $Content = $Content -replace "using MediatR;", ""
                $Content = $Content + "`nglobal using LiteBus.Commands.Abstractions;`nglobal using LiteBus.Queries.Abstractions;`n"
                Set-Content $GlobalUsingsPath -Value $Content
                Write-Host "  [2/4] GlobalUsings updated" -ForegroundColor Green
            }
        }
        
        # Phase 3: Build and verify (unless skipped)
        if (-not $SkipBuild) {
            Write-Host "  [3/4] Building solution..." -ForegroundColor Cyan
            $BuildOutput = dotnet build 2>&1
            $BuildSuccess = $LASTEXITCODE -eq 0
            
            if ($BuildSuccess) {
                Write-Host "  [3/4] Build SUCCEEDED" -ForegroundColor Green
            } else {
                # Check for C# errors vs warnings
                $CSharpErrors = @($BuildOutput | Select-String "error CS")
                if ($CSharpErrors.Count -gt 0) {
                    Write-Host "  [3/4] Build has $($CSharpErrors.Count) C# errors" -ForegroundColor Red
                    Write-Host ($CSharpErrors | ForEach-Object { "    $_" } | Out-String)
                }
            }
        }
        
        Pop-Location
        
        Write-Host "[$PhaseName] COMPLETE" -ForegroundColor Green
        $Results += @{ Phase = $PhaseName; Status = "SUCCESS"; Count = $ReplacementCount }
    }
    catch {
        Pop-Location
        Write-Host "[$PhaseName] ERROR - $_" -ForegroundColor Red
        $Results += @{ Phase = $PhaseName; Status = "ERROR"; Message = $_.Exception.Message }
    }
}

# Summary Report
Write-Host "`n" + ("="*100) -ForegroundColor Cyan
Write-Host "MIGRATION SUMMARY REPORT" -ForegroundColor Cyan
Write-Host ("="*100) -ForegroundColor Cyan

$SuccessCount = ($Results | Where-Object { $_.Status -eq "SUCCESS" }).Count
$AlreadyMigratedCount = ($Results | Where-Object { $_.Status -eq "ALREADY_MIGRATED" }).Count
$ErrorCount = ($Results | Where-Object { $_.Status -eq "ERROR" }).Count
$SkippedCount = ($Results | Where-Object { $_.Status -in @("DRY_RUN", "NO_MEDIATR", "SKIP") }).Count

Write-Host "`nResults by Phase:`n" -ForegroundColor White
foreach ($Result in $Results) {
    $Status = $Result.Status
    $Icon = switch ($Status) {
        "SUCCESS" { "OK" }
        "ALREADY_MIGRATED" { "V" }
        "ERROR" { "X" }
        default { "-" }
    }
    Write-Host "  $Icon $($Result.Phase): $Status (Count: $(if ($Result.Count) { $Result.Count } else { 0 }))" -ForegroundColor $(
        if ($Status -eq "SUCCESS" -or $Status -eq "ALREADY_MIGRATED") { "Green" }
        elseif ($Status -eq "ERROR") { "Red" }
        else { "Yellow" }
    )
}

Write-Host "`n" + ("="*100) -ForegroundColor Cyan
Write-Host "TOTALS: SUCCESS=$SuccessCount | ALREADY_MIGRATED=$AlreadyMigratedCount | ERRORS=$ErrorCount | SKIPPED=$SkippedCount" -ForegroundColor $(
    if ($ErrorCount -eq 0) { "Green" } else { "Red" }
)

$Duration = (Get-Date) - $MigrationStartTime
Write-Host "Duration: $($Duration.Hours)h $($Duration.Minutes)m $($Duration.Seconds)s" -ForegroundColor White
Write-Host ("="*100) -ForegroundColor Cyan
Write-Host ""
