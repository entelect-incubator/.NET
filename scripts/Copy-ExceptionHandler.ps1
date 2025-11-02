# PowerShell Script: Copy IExceptionHandler to All Phases
# Purpose: Copies GlobalExceptionHandler.cs and updates DependencyInjection to all phases
# Author: Development Team
# Date: October 31, 2025

param(
    [string]$IncubatorPath = "d:\Dev\Incubator\.NET",
    [int[]]$Phases = @(3, 4, 5, 6, 7, 8, 9, 10, 11, 12),
    [switch]$Verbose = $false,
    [switch]$Force = $false
)

$ErrorActionPreference = "Stop"

# Color output helper
function Write-Success {
    Write-Host $args -ForegroundColor Green
}

function Write-Error {
    Write-Host $args -ForegroundColor Red
}

function Write-Info {
    Write-Host $args -ForegroundColor Cyan
}

function Write-Warn {
    Write-Host $args -ForegroundColor Yellow
}

# Verify source file
$sourceHandlerPath = Join-Path $IncubatorPath "Phase 2\src\02. EndSolution\Common\Handlers\GlobalExceptionHandler.cs"
if (-not (Test-Path $sourceHandlerPath)) {
    Write-Error "❌ Source GlobalExceptionHandler.cs not found at: $sourceHandlerPath"
    exit 1
}

Write-Info "╔════════════════════════════════════════════════════════════╗"
Write-Info "║     Copy IExceptionHandler to All Phases                  ║"
Write-Info "║     Phases 3-12 Update                                    ║"
Write-Info "╚════════════════════════════════════════════════════════════╝`n"

# Track results
$results = @{
    Success = @()
    Failed = @()
    Skipped = @()
}

foreach ($phase in $Phases) {
    $phasePath = Join-Path $IncubatorPath "Phase $phase"
    $commonPath = Join-Path $phasePath "src\01. StartSolution\Common"
    $corePath = Join-Path $phasePath "src\01. StartSolution\Core"
    
    # Check if phase exists
    if (-not (Test-Path $commonPath)) {
        Write-Warn "⚠️  Phase $phase`: Common directory not found"
        $results.Skipped += "Phase $phase"
        continue
    }
    
    Write-Info "`n🔄 Processing Phase $phase..."
    
    try {
        # 1. Create Handlers directory if it doesn't exist
        $handlersPath = Join-Path $commonPath "Handlers"
        if (-not (Test-Path $handlersPath)) {
            New-Item -ItemType Directory -Path $handlersPath | Out-Null
            Write-Host "   ✅ Created Handlers directory" -ForegroundColor Green
        } else {
            Write-Host "   ✅ Handlers directory exists" -ForegroundColor Green
        }
        
        # 2. Copy GlobalExceptionHandler.cs
        $destHandlerPath = Join-Path $handlersPath "GlobalExceptionHandler.cs"
        Copy-Item -Path $sourceHandlerPath -Destination $destHandlerPath -Force:$Force
        Write-Host "   ✅ Copied GlobalExceptionHandler.cs" -ForegroundColor Green
        
        # 3. Check and update DependencyInjection.cs
        $depInjectionPath = Join-Path $corePath "DependencyInjection.cs"
        if (Test-Path $depInjectionPath) {
            $content = Get-Content $depInjectionPath -Raw
            
            # Check if GlobalExceptionHandler is already imported
            if ($content -match "using Common\.Handlers") {
                Write-Host "   ✅ Common.Handlers already imported in DependencyInjection.cs" -ForegroundColor Green
            } else {
                # Add the using statement
                $newContent = $content -replace '(using Common\.Behaviour;)', '${1}' + "`nusing Common.Handlers;"
                $newContent = $newContent -replace '(// This replaces)', 'services.AddExceptionHandler<GlobalExceptionHandler>();' + "`n`n`t`t// This replaces"
                
                # Make sure AddExceptionHandler is not already there
                if (-not ($newContent -match "AddExceptionHandler")) {
                    # Insert the handler registration before return statement
                    $newContent = $newContent -replace '(\s+return services;)', "`n`t`t// Register global exception handler using .NET 8+ IExceptionHandler pattern`n`t`tservices.AddExceptionHandler<GlobalExceptionHandler>();`n`n`$1"
                }
                
                Set-Content -Path $depInjectionPath -Value $newContent
                Write-Host "   ✅ Updated DependencyInjection.cs" -ForegroundColor Green
            }
        } else {
            Write-Warn "   ⚠️  DependencyInjection.cs not found - skipping update"
        }
        
        $results.Success += "Phase $phase"
        Write-Success "   ✅ Phase $phase`: GlobalExceptionHandler installed"
        
    } catch {
        Write-Error "   ❌ Error processing Phase $phase`: $_"
        $results.Failed += "Phase $phase"
    }
}

# Summary
Write-Info "`n╔════════════════════════════════════════════════════════════╗"
Write-Info "║                  SUMMARY REPORT                           ║"
Write-Info "╚════════════════════════════════════════════════════════════╝`n"

Write-Success "✅ Successful: $($results.Success.Count)"
foreach ($phase in $results.Success) {
    Write-Host "   • $phase" -ForegroundColor Green
}

if ($results.Skipped.Count -gt 0) {
    Write-Warn "`n⚠️  Skipped: $($results.Skipped.Count)"
    foreach ($phase in $results.Skipped) {
        Write-Host "   • $phase" -ForegroundColor Yellow
    }
}

if ($results.Failed.Count -gt 0) {
    Write-Error "`n❌ Failed: $($results.Failed.Count)"
    foreach ($phase in $results.Failed) {
        Write-Host "   • $phase" -ForegroundColor Red
    }
}

Write-Info "`n📝 NEXT STEPS:"
Write-Info "   1. Review changes in each phase's DependencyInjection.cs"
Write-Info "   2. Update Api/Startup.cs to add:"
Write-Info "      - services.AddExceptionHandler() in ConfigureServices"
Write-Info "      - app.UseExceptionHandler() at start of Configure method"
Write-Info "   3. Run 'dotnet build' to verify compilation`n"

Write-Success "Script completed! 🎉"
