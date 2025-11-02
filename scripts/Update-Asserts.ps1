#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Batch update all Assert.IsTrue/IsFalse syntax to modern Assert.That syntax
    across all phases and documentation files.

.DESCRIPTION
    This script updates legacy NUnit assertion syntax to modern constraint-based assertions:
    - Assert.IsTrue(condition) -> Assert.That(condition, Is.True)
    - Assert.IsFalse(condition) -> Assert.That(condition, Is.False)
    - Assert.IsNull(value) -> Assert.That(value, Is.Null)
    - Assert.IsNotNull(value) -> Assert.That(value, Is.Not.Null)
    
.PARAMETER RootPath
    The root path to search for files. Defaults to current directory.

.PARAMETER FilePattern
    The file pattern to match. Defaults to '*.md' and '*.cs'

.PARAMETER WhatIf
    Show what changes would be made without making them.

.EXAMPLE
    .\Update-Asserts.ps1 -RootPath "d:\Dev\Incubator\.NET" -Verbose
    
.EXAMPLE
    .\Update-Asserts.ps1 -RootPath "d:\Dev\Incubator\.NET" -WhatIf

#>

param(
    [Parameter(Mandatory = $false)]
    [string]$RootPath = (Get-Location),
    
    [Parameter(Mandatory = $false)]
    [string[]]$FilePattern = @('*.md', '*.cs'),
    
    [Parameter(Mandatory = $false)]
    [switch]$WhatIf
)

$ErrorActionPreference = 'Stop'

# Replacement rules - ordered from most specific to most general
$replacementRules = @(
    @{
        Old = 'Assert\.IsTrue\(([^,)]+)\s*!=\s*null\)'
        New = 'Assert.That($1, Is.Not.Null)'
        Description = 'Assert.IsTrue(x != null) to Assert.That(x, Is.Not.Null)'
    },
    @{
        Old = 'Assert\.IsTrue\(\s*!([^)]+)\s*\)'
        New = 'Assert.That($1, Is.False)'
        Description = 'Assert.IsTrue(!x) to Assert.That(x, Is.False)'
    },
    @{
        Old = 'Assert\.IsTrue\(([^)]+)\s*==\s*1\)'
        New = 'Assert.That($1, Is.EqualTo(1))'
        Description = 'Assert.IsTrue(x == 1) to Assert.That(x, Is.EqualTo(1))'
    },
    @{
        Old = 'Assert\.IsTrue\(([^)]+)\.Succeeded\)'
        New = 'Assert.That($1.Succeeded, Is.True)'
        Description = 'Assert.IsTrue(x.Succeeded) to Assert.That(x.Succeeded, Is.True)'
    },
    @{
        Old = 'Assert\.IsTrue\(([^)]+)\)'
        New = 'Assert.That($1, Is.True)'
        Description = 'Assert.IsTrue(x) to Assert.That(x, Is.True)'
    },
    @{
        Old = 'Assert\.IsFalse\(([^)]+)\)'
        New = 'Assert.That($1, Is.False)'
        Description = 'Assert.IsFalse(x) to Assert.That(x, Is.False)'
    },
    @{
        Old = 'Assert\.IsNull\(([^)]+)\)'
        New = 'Assert.That($1, Is.Null)'
        Description = 'Assert.IsNull(x) to Assert.That(x, Is.Null)'
    },
    @{
        Old = 'Assert\.IsNotNull\(([^)]+)\)'
        New = 'Assert.That($1, Is.Not.Null)'
        Description = 'Assert.IsNotNull(x) to Assert.That(x, Is.Not.Null)'
    }
)

function Update-FilesInDirectory {
    param(
        [string]$Path,
        [string[]]$Patterns
    )
    
    $files = @()
    foreach ($pattern in $Patterns) {
        $files += Get-ChildItem -Path $Path -Filter $pattern -Recurse -ErrorAction SilentlyContinue
    }
    
    return $files
}

function Update-FileContent {
    param(
        [string]$FilePath,
        [object[]]$Rules
    )
    
    try {
        $content = Get-Content -Path $FilePath -Raw -Encoding UTF8 -ErrorAction Stop
    } catch {
        Write-Verbose "Skipping binary or unreadable file: $FilePath"
        return 0
    }
    
    if ([string]::IsNullOrEmpty($content)) {
        return 0
    }
    
    $originalContent = $content
    $changesCount = 0
    
    foreach ($rule in $Rules) {
        $regexMatches = [regex]::Matches($content, $rule.Old)
        if ($regexMatches.Count -gt 0) {
            Write-Verbose "Found $($regexMatches.Count) matches for: $($rule.Description)"
            $content = $content -replace $rule.Old, $rule.New
            $changesCount += $regexMatches.Count
        }
    }
    
    if ($content -ne $originalContent) {
        if ($WhatIf) {
            Write-Host "[WhatIf] Would update $FilePath ($changesCount changes)" -ForegroundColor Cyan
        } else {
            Set-Content -Path $FilePath -Value $content -Encoding UTF8 -NoNewline
            Write-Host "[OK] Updated $FilePath ($changesCount changes)" -ForegroundColor Green
        }
        return $changesCount
    }
    
    return 0
}

# Main execution
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Assert Syntax Batch Update Utility" -ForegroundColor Cyan
Write-Host "Converting Assert.IsTrue -> Assert.That" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

if ($WhatIf) {
    Write-Host "[WhatIf Mode] No changes will be made" -ForegroundColor Yellow
    Write-Host ""
}

Write-Host "Scanning directory: $RootPath" -ForegroundColor White
Write-Host "File patterns: $($FilePattern -join ', ')" -ForegroundColor White
Write-Host ""

# Get all files to process
$filesToProcess = Update-FilesInDirectory -Path $RootPath -Patterns $FilePattern

Write-Host "Found $($filesToProcess.Count) files to check" -ForegroundColor Cyan
Write-Host ""

$totalChanges = 0
$filesChanged = 0

foreach ($file in $filesToProcess) {
    $changes = Update-FileContent -FilePath $file.FullName -Rules $replacementRules
    if ($changes -gt 0) {
        $totalChanges += $changes
        $filesChanged++
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Summary" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Files checked: $($filesToProcess.Count)" -ForegroundColor White
Write-Host "Files modified: $filesChanged" -ForegroundColor Green
Write-Host "Total replacements: $totalChanges" -ForegroundColor Green
Write-Host ""

if ($WhatIf) {
    Write-Host "Run without -WhatIf to apply changes" -ForegroundColor Yellow
} else {
    Write-Host "Assert syntax updated successfully!" -ForegroundColor Green
}

Write-Host ""
Write-Host "Replacement patterns applied:" -ForegroundColor Cyan
foreach ($rule in $replacementRules) {
    Write-Host "  - $($rule.Description)" -ForegroundColor Gray
}
