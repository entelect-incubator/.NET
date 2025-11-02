<#
.SYNOPSIS
Fixes dispatcher method names in Phase 5-9 handlers.

.DESCRIPTION
Updates all command and query handlers in Phase 5-9 to use correct dispatcher method:
- Renames HandleAsync -> Handle
- Ensures ICommandHandler<TCommand, TResult> and IQueryHandler<TQuery, TResult> use Handle method

.PARAMETER DryRun
If specified, shows what would be changed without making actual changes.

.EXAMPLE
.\fix-dispatcher-pattern.ps1 -DryRun -Verbose
.\fix-dispatcher-pattern.ps1  # Actually make changes
#>

[CmdletBinding()]
param(
    [switch]$DryRun
)

$ErrorActionPreference = 'Stop'

# Get the current directory (should be .NET folder)
$dotNetRoot = Get-Location

Write-Host "[*] Starting Dispatcher Pattern Fix (HandleAsync -> Handle)" -ForegroundColor Cyan
Write-Host "[*] Root directory: $dotNetRoot" -ForegroundColor Gray

# Find all Phase directories (Phase 5 - Phase 9)
$phasePattern = 'Phase [5-9]'
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

$filesModified = 0
$replacementsCount = 0

foreach ($phaseDir in $phaseDirs) {
    Write-Host "[*] Processing: $(Split-Path -Leaf $phaseDir)" -ForegroundColor Cyan
    
    # Find all C# files that might contain handlers (Commands, Queries, Notifications)
    $commandFiles = @(Get-ChildItem -Path "$phaseDir\*\*\Core\*\Commands\*.cs" -Recurse -ErrorAction SilentlyContinue)
    $queryFiles = @(Get-ChildItem -Path "$phaseDir\*\*\Core\*\Queries\*.cs" -Recurse -ErrorAction SilentlyContinue)
    $notificationFiles = @(Get-ChildItem -Path "$phaseDir\*\*\Core\*\Notifications\*.cs" -Recurse -ErrorAction SilentlyContinue)
    
    $allFiles = $commandFiles + $queryFiles + $notificationFiles | Select-Object -Unique
    
    if ($allFiles.Count -eq 0) {
        Write-Host "    [!] No command/query/notification files found" -ForegroundColor Yellow
        continue
    }

    if ($PSBoundParameters['Verbose']) {
        Write-Host "    [*] Found $($allFiles.Count) files to check" -ForegroundColor Gray
    }
    
    foreach ($file in $allFiles) {
        $content = Get-Content -Path $file.FullName -Raw -ErrorAction SilentlyContinue
        
        if (-not $content) {
            continue
        }
        
        $originalContent = $content
        
        # Count matches before replacement - only if file contains ICommandHandler, IQueryHandler, or INotificationHandler
        if (-not ($originalContent -match 'I(Command|Query|Notification)Handler')) {
            continue
        }
        
        $matchCount = ([regex]::Matches($originalContent, '\bHandleAsync\b')).Count
        
        if ($matchCount -eq 0) {
            continue
        }
        
        # Replace HandleAsync with Handle
        $content = [regex]::Replace($content, '\bHandleAsync\b', 'Handle')
        
        if ($content -ne $originalContent) {
            $filesModified++
            $replacementsCount += $matchCount
            
            $relativePath = $file.FullName.Replace($dotNetRoot, '').TrimStart('\')
            
            if ($DryRun) {
                Write-Host "    [~] [DRY RUN] Would modify: $relativePath ($matchCount replacements)" -ForegroundColor Yellow
                
                if ($PSBoundParameters['Verbose']) {
                    Write-Host "        Replacing HandleAsync -> Handle" -ForegroundColor Gray
                }
            }
            else {
                # Actually write the changes
                try {
                    Set-Content -Path $file.FullName -Value $content -NoNewline -ErrorAction Stop
                    Write-Host "    [+] Modified: $relativePath ($matchCount replacements)" -ForegroundColor Green
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
    Write-Host "  .\fix-dispatcher-pattern.ps1" -ForegroundColor Gray
}
else {
    Write-Host ""
    Write-Host "[+] Dispatcher pattern fix completed!" -ForegroundColor Green
}
