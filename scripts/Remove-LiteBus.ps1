# Remove-LiteBus.ps1
# Removes all LiteBus package references from .csproj files across all phases

param(
    [string]$RootPath = "c:\Dev\Incubator\.NET",
    [switch]$WhatIf
)

$csprojFiles = Get-ChildItem -Path $RootPath -Filter "*.csproj" -Recurse | Where-Object {
    $_.FullName -notmatch '\\obj\\' -and
    $_.FullName -notmatch '\\bin\\' -and
    $_.FullName -notmatch '\\\.git\\'
}

$liteBusPackages = @(
    'LiteBus"',
    'LiteBus.Commands"',
    'LiteBus.Queries"',
    'LiteBus.Commands.Abstractions"',
    'LiteBus.Queries.Abstractions"',
    'LiteBus.Messaging.Abstractions"',
    'LiteBus.Events.Extensions.MicrosoftDependencyInjection"',
    'LiteBus.Extensions.MicrosoftDependencyInjection"'
)

$filesModified = 0
$linesRemoved = 0

Write-Host "Scanning for LiteBus package references..." -ForegroundColor Cyan
Write-Host "Root path: $RootPath" -ForegroundColor Gray

foreach ($file in $csprojFiles) {
    $content = Get-Content $file.FullName -Raw
    $originalContent = $content
    $modified = $false
    $fileLineCount = 0

    foreach ($package in $liteBusPackages) {
        # Match lines containing the package reference
        $pattern = '^\s*<PackageReference\s+Include="[^"]*' + [regex]::Escape($package.TrimEnd('"')) + '[^"]*"[^>]*/>?\s*$'
        
        $lines = $content -split "`r?`n"
        $filteredLines = @()
        
        for ($i = 0; $i -lt $lines.Count; $i++) {
            if ($lines[$i] -match $pattern) {
                $modified = $true
                $fileLineCount++
                Write-Host "  Found: $($lines[$i].Trim())" -ForegroundColor Yellow
                # Skip this line (don't add to filtered)
            } else {
                $filteredLines += $lines[$i]
            }
        }
        
        $content = $filteredLines -join "`r`n"
    }

    if ($modified) {
        $filesModified++
        $linesRemoved += $fileLineCount
        
        $relativePath = $file.FullName.Replace($RootPath, "").TrimStart('\')
        Write-Host "`n[$filesModified] $relativePath" -ForegroundColor Green
        Write-Host "  Removed $fileLineCount LiteBus package reference(s)" -ForegroundColor Green
        
        if (-not $WhatIf) {
            Set-Content -Path $file.FullName -Value $content -NoNewline
            Write-Host "  File updated!" -ForegroundColor Green
        } else {
            Write-Host "  [WhatIf] Would update file" -ForegroundColor Magenta
        }
    }
}

Write-Host "`n=== Summary ===" -ForegroundColor Cyan
Write-Host "Files scanned: $($csprojFiles.Count)" -ForegroundColor White
Write-Host "Files modified: $filesModified" -ForegroundColor Green
Write-Host "Package references removed: $linesRemoved" -ForegroundColor Green

if ($WhatIf) {
    Write-Host "`n[WhatIf mode] No files were actually modified." -ForegroundColor Magenta
    Write-Host "Run without -WhatIf to apply changes." -ForegroundColor Magenta
} else {
    Write-Host "`nDone! Build solutions to verify changes." -ForegroundColor Cyan
}
