#!/usr/bin/env pwsh

# Fix Phase 8 namespace references from Common to Pezza.Common and DataAccess to Pezza.DataAccess
$phase8Path = "d:\Dev\Incubator\.NET\Phase 8\src\01. StartSolution"

Get-ChildItem $phase8Path -Filter "*.cs" -Recurse | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    $updated = $content
    
    # Replace using statements
    $updated = $updated -replace "using Common\.", "using Pezza.Common."
    $updated = $updated -replace "using DataAccess\.", "using Pezza.DataAccess."
    
    # Replace namespace references in code
    $updated = $updated -replace "Common\.Entities", "Pezza.Common.Entities"
    $updated = $updated -replace "Common\.Models", "Pezza.Common.Models"
    $updated = $updated -replace "Common\.DTO", "Pezza.Common.DTO"
    
    if ($updated -ne $content) {
        Set-Content $_.FullName $updated
        Write-Host "Fixed: $($_.Name)"
    }
}
