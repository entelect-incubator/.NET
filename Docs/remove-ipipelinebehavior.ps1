#!/usr/bin/env powershell
# Remove old IPipelineBehavior registrations from DependencyInjection.cs files
# Replaces old MediatR pipeline behaviors with modern GlobalExceptionHandler

$dotNetRoot = "d:\Dev\Incubator\.NET"
$phases = 4..9
$DRY_RUN = $true  # Set to $false to actually modify files

Write-Host "=== Removing IPipelineBehavior Registrations ===" -ForegroundColor Cyan
Write-Host "DRY-RUN MODE: $DRY_RUN" -ForegroundColor Yellow
Write-Host ""

$updatedCount = 0
$skippedCount = 0
$filesFound = @()

foreach ($phase in $phases) {
    $phasePath = Join-Path $dotNetRoot "Phase $phase"
    
    if (-not (Test-Path $phasePath)) {
        continue
    }
    
    # Find all DependencyInjection.cs files
    $depInjectionFiles = Get-ChildItem -Path $phasePath -Recurse -Filter "DependencyInjection.cs" -ErrorAction SilentlyContinue
    
    foreach ($file in $depInjectionFiles) {
        $content = Get-Content -Path $file.FullName -Raw
        
        # Check if file has IPipelineBehavior registrations
        if ($content -match "IPipelineBehavior") {
            $filesFound += @{
                Phase = $phase
                File = $file.FullName
                Content = $content
            }
            
            Write-Host "Found IPipelineBehavior registrations in Phase $phase" -ForegroundColor Yellow
            Write-Host "  File: $($file.FullName)" -ForegroundColor Cyan
            
            # Count registrations
            $registrations = $content | Select-String -Pattern "services\.AddTransient\(typeof\(IPipelineBehavior" -AllMatches
            if ($registrations) {
                Write-Host "  Registrations found: $($registrations.Matches.Count)" -ForegroundColor Yellow
            }
        }
    }
}

Write-Host ""
Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host "Files with IPipelineBehavior: $($filesFound.Count)"
Write-Host ""

if ($filesFound.Count -gt 0) {
    Write-Host "Files requiring updates:" -ForegroundColor Green
    foreach ($file in $filesFound) {
        Write-Host "  Phase $($file.Phase): $($file.File)"
    }
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Cyan
    Write-Host "1. Review each file and remove lines like:"
    Write-Host "   services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));"
    Write-Host "   services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));"
    Write-Host "2. Replace with modern exception handling:"
    Write-Host "   services.AddExceptionHandler<GlobalExceptionHandler>();"
    Write-Host "   services.AddProblemDetails();"
    Write-Host ""
    Write-Host "Then register in Configure():"
    Write-Host "   app.UseExceptionHandler();"
} else {
    Write-Host "No IPipelineBehavior registrations found!" -ForegroundColor Green
}
