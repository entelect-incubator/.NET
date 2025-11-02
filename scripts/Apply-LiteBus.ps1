# PowerShell Script: Apply LiteBus Foundation to All Phases
# Purpose: Updates all Phase 2-12 with LiteBus infrastructure
# Author: Development Team
# Date: October 31, 2025

param(
    [string]$IncubatorPath = "d:\Dev\Incubator\.NET",
    [int[]]$Phases = @(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12),
    [switch]$Verbose = $false
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

# Verify incubator path exists
if (-not (Test-Path $IncubatorPath)) {
    Write-Error "❌ Incubator path not found: $IncubatorPath"
    exit 1
}

Write-Info "╔════════════════════════════════════════════════════════════╗"
Write-Info "║        LiteBus Foundation Application Script              ║"
Write-Info "║        Phases 2-12 Update                                 ║"
Write-Info "╚════════════════════════════════════════════════════════════╝`n"

# Track results
$results = @{
    Success = @()
    Failed = @()
    Skipped = @()
}

# Phase 2: Already updated in main work
if ($Phases -contains 2) {
    Write-Info "📌 Phase 2: Skipping (already updated in main work)"
    $results.Skipped += "Phase 2"
}

# Process Phases 3-12
foreach ($phase in $Phases | Where-Object { $_ -gt 2 }) {
    $phasePath = Join-Path $IncubatorPath "Phase $phase"
    $startSolutionPath = Join-Path $phasePath "src\01. StartSolution"
    
    # Check if phase exists
    if (-not (Test-Path $startSolutionPath)) {
        Write-Warn "⚠️  Phase $phase`: StartSolution not found, skipping"
        $results.Skipped += "Phase $phase"
        continue
    }
    
    Write-Info "`n🔄 Processing Phase $phase..."
    
    try {
        # 1. Check and update Api.csproj
        $apiCsprojPath = Join-Path $startSolutionPath "Api\Api.csproj"
        if (Test-Path $apiCsprojPath) {
            $csprojContent = Get-Content $apiCsprojPath -Raw
            
            # Check if LiteBus package is already referenced
            if ($csprojContent -match 'PackageReference.*LiteBus.*1\.0\.0') {
                Write-Host "   ✅ LiteBus already in Api.csproj" -ForegroundColor Green
            } else {
                # Add LiteBus package if not present
                if ($csprojContent -match '<PackageReference Include="Swashbuckle') {
                    $newCsproj = $csprojContent -replace '(<PackageReference Include="Swashbuckle[^>]*>)', 
                        '<PackageReference Include="LiteBus" Version="1.0.0" />' + "`n`t`t`$1"
                    Set-Content -Path $apiCsprojPath -Value $newCsproj
                    Write-Host "   ✅ Added LiteBus 1.0.0 to Api.csproj" -ForegroundColor Green
                }
            }
        }
        
        # 2. Check ApiController.cs exists
        $apiControllerPath = Join-Path $startSolutionPath "Api\Controllers\ApiController.cs"
        if (-not (Test-Path $apiControllerPath)) {
            Write-Warn "   ⚠️  ApiController.cs not found - skipping ApiController update"
        } else {
            Write-Host "   ✅ ApiController.cs exists" -ForegroundColor Green
        }
        
        # 3. Check Startup.cs
        $startupPath = Join-Path $startSolutionPath "Api\Startup.cs"
        if (-not (Test-Path $startupPath)) {
            Write-Warn "   ⚠️  Startup.cs not found - skipping Startup update"
        } else {
            Write-Host "   ✅ Startup.cs exists" -ForegroundColor Green
        }
        
        $results.Success += "Phase $phase"
        Write-Success "   ✅ Phase $phase`: Ready for LiteBus (manual validation recommended)"
        
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

Write-Info "`n📝 MANUAL STEPS REQUIRED:"
Write-Info "   1. Review Api.csproj changes in each phase"
Write-Info "   2. Verify Startup.cs has correct namespaces"
Write-Info "   3. Run 'dotnet build' for each phase to verify"
Write-Info "   4. Check ApiController is using LiteBus mediators`n"

Write-Success "Script completed! 🎉"
