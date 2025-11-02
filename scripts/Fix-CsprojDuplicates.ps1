#!/usr/bin/env pwsh
param(
    [int[]]$Phases = @(2, 3, 5, 6, 7, 8)
)

Write-Host "Fixing csproj files (removing duplicate LiteBus entries)..." -ForegroundColor Cyan
Write-Host ""

foreach ($phaseNum in $Phases) {
    $startSolDir = "d:\Dev\Incubator\.NET\Phase $phaseNum\src\01. StartSolution"
    
    if (-not (Test-Path $startSolDir)) {
        Write-Host "Phase $phaseNum not found" -ForegroundColor Yellow
        continue
    }
    
    Write-Host "Processing Phase $phaseNum..."
    
    # Find all csproj files
    $csprojFiles = Get-ChildItem -Path $startSolDir -Include "*.csproj" -Recurse
    
    foreach ($csproj in $csprojFiles) {
        $content = Get-Content $csproj.FullName -Raw
        $original = $content
        
        # Remove duplicate LiteBus entries - keep only one
        $lines = $content -split "`n"
        $litebusLines = $lines | Select-String 'PackageReference Include="LiteBus"'
        
        if ($litebusLines.Count -gt 1) {
            # Keep only the first LiteBus entry
            $keepFirst = $true
            $processed = foreach ($line in $lines) {
                if ($line -match 'PackageReference Include="LiteBus"') {
                    if ($keepFirst) {
                        $line
                        $keepFirst = $false
                    }
                    # Skip duplicate lines
                } else {
                    $line
                }
            }
            $content = $processed -join "`n"
        }
        
        if ($content -ne $original) {
            Set-Content -Path $csproj.FullName -Value $content -NoNewline
            Write-Host "  Fixed $($csproj.Name) - removed duplicate LiteBus entries" -ForegroundColor Green
        }
    }
}

Write-Host ""
Write-Host "Done!" -ForegroundColor Green
