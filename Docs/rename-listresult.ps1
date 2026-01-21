<#
.SYNOPSIS
Renames ListResult<T> to Result<IEnumerable<T>> across all phase directories.

.DESCRIPTION
This script finds all C# files in Phase directories (Phase 4 through Phase 9) and renames:
- ListResult<T> → Result<IEnumerable<T>>
- ListResult → Result<IEnumerable<object>>

.PARAMETER DryRun
If specified, shows what would be changed without making actual changes.

.PARAMETER Verbose
Shows detailed information about each operation.

.EXAMPLE
.\rename-listresult.ps1 -DryRun
.\rename-listresult.ps1 -DryRun -Verbose
.\rename-listresult.ps1  # Actually make changes
#>

[CmdletBinding()]
param(
    [switch]$DryRun
)

$ErrorActionPreference = 'Stop'

# Get the current directory (should be .NET folder)
$dotNetRoot = Get-Location

Write-Host "[*] Starting ListResult -> Result<IEnumerable<T>> Rename" -ForegroundColor Cyan
Write-Host "[*] Root directory: $dotNetRoot" -ForegroundColor Gray

# Find all Phase directories (Phase 4 - Phase 9)
$phasePattern = 'Phase [4-9]'
$phaseDirs = @()

Get-ChildItem -Path $dotNetRoot -Directory | Where-Object { $_.Name -match $phasePattern } | ForEach-Object {
    $phaseDirs += $_.FullName
}

if ($phaseDirs.Count -eq 0) {
    Write-Host "[!] No Phase directories found matching pattern '$phasePattern'" -ForegroundColor Yellow
    exit 0
}

Write-Host "[+] Found $($phaseDirs.Count) phase directories" -ForegroundColor Green
$phaseDirs | ForEach-Object { Write-Host "    - $(Split-Path -Leaf $_)" -ForegroundColor Gray }
Write-Host ""

# Pattern to find ListResult usage
# Matches: ListResult<T>, ListResult (without brackets)
$filesModified = 0
$replacementsCount = 0

foreach ($phaseDir in $phaseDirs) {
    Write-Host "[*] Processing: $(Split-Path -Leaf $phaseDir)" -ForegroundColor Cyan
    
    # Find all C# files recursively
    $csharpFiles = @(Get-ChildItem -Path $phaseDir -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue)
    
    if ($csharpFiles.Count -eq 0) {
        Write-Host "    [!] No .cs files found" -ForegroundColor Yellow
        continue
    }

    if ($PSBoundParameters['Verbose']) {
        Write-Host "    [*] Found $($csharpFiles.Count) C# files" -ForegroundColor Gray
    }
    
    foreach ($file in $csharpFiles) {
        $content = Get-Content -Path $file.FullName -Raw -ErrorAction SilentlyContinue
        
        if (-not $content) {
            continue
        }
        
        $originalContent = $content
        $originalCount = 0
        
        # Count original matches before replacing
        $originalCount = ([regex]::Matches($originalContent, 'ListResult(?:<[^>]+>)?')).Count
        
        # Replace ListResult<T> with Result<IEnumerable<T>>
        # This regex captures the generic type parameter
        $content = [regex]::Replace($content, 'ListResult<([^>]+)>', 'Result<IEnumerable<$1>>')
        
        # Replace standalone ListResult with Result<IEnumerable<object>>
        $content = [regex]::Replace($content, '\bListResult\b', 'Result<IEnumerable<object>>')
        
        if ($content -ne $originalContent) {
            $filesModified++
            $replacementsCount += $originalCount
            
            $relativePath = $file.FullName.Replace($dotNetRoot, '').TrimStart('\')
            
            if ($DryRun) {
                Write-Host "    [~] [DRY RUN] Would modify: $relativePath ($originalCount replacements)" -ForegroundColor Yellow
                
                if ($PSBoundParameters['Verbose']) {
                    Write-Host "        Pattern matches found in content" -ForegroundColor Gray
                }
            }
            else {
                # Actually write the changes
                try {
                    Set-Content -Path $file.FullName -Value $content -NoNewline -ErrorAction Stop
                    Write-Host "    [+] Modified: $relativePath ($originalCount replacements)" -ForegroundColor Green
                }
                catch {
                    Write-Host "    [!] Error modifying $relativePath : $_" -ForegroundColor Red
                }
            }
        }
    }
}

Write-Host ""
Write-Host ("=" * 60) -ForegroundColor Cyan
if ($DryRun) {
    Write-Host "[*] DRY RUN SUMMARY" -ForegroundColor Yellow
}
else {
    Write-Host "[+] EXECUTION SUMMARY" -ForegroundColor Green
}
Write-Host ("=" * 60) -ForegroundColor Cyan
Write-Host "Files to modify:    $filesModified" -ForegroundColor Cyan
Write-Host "Total replacements: $replacementsCount" -ForegroundColor Cyan

if ($DryRun) {
    Write-Host ""
    Write-Host "Run without -DryRun to apply changes:" -ForegroundColor Yellow
    Write-Host "  .\rename-listresult.ps1" -ForegroundColor Gray
}
else {
    Write-Host ""
    Write-Host "[+] Rename operation completed!" -ForegroundColor Green
}
