#!/usr/bin/env pwsh

# Upgrade Phase 8 from net10.0 to net10.0
$phase8Path = "d:\Dev\Incubator\.NET\Phase 8"

# Find all .csproj files
$csprojFiles = Get-ChildItem -Path $phase8Path -Filter "*.csproj" -Recurse

foreach ($file in $csprojFiles) {
    $content = Get-Content $file.FullName -Raw
    
    if ($content -contains "net10.0") {
        $updated = $content -replace "<TargetFramework>net7\.0</TargetFramework>", "<TargetFramework>net10.0</TargetFramework>"
        Set-Content -Path $file.FullName -Value $updated
        Write-Host "Upgraded: $($file.Name)" -ForegroundColor Green
    }
}

Write-Host "`nPhase 8 framework upgrade complete!" -ForegroundColor Cyan
