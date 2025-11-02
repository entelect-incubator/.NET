#!/usr/bin/env pwsh

# Fix Phase 8 net7.0 -> net8.0
$phase8Path = "d:\Dev\Incubator\.NET\Phase 8\src\01. StartSolution"

$csprojFiles = @(
    "$phase8Path\Pezza.Core\Pezza.Core.csproj",
    "$phase8Path\Pezza.Common\Pezza.Common.csproj",
    "$phase8Path\Pezza.DataAccess\Pezza.DataAccess.csproj",
    "$phase8Path\Pezza.Api\Pezza.Api.csproj",
    "$phase8Path\Pezza.BackEnd\Pezza.Portal.csproj",
    "$phase8Path\Pezza.Scheduler\Pezza.Scheduler.csproj",
    "$phase8Path\Pezza.Test\Pezza.Test.csproj"
)

foreach ($file in $csprojFiles) {
    if (Test-Path $file) {
        (Get-Content $file -Raw) -replace "<TargetFramework>net7\.0</TargetFramework>", "<TargetFramework>net8.0</TargetFramework>" | Set-Content $file
        Write-Host "Updated: $(Split-Path $file -Leaf)" -ForegroundColor Green
    }
}
