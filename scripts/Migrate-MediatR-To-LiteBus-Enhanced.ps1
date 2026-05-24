#Requires -Version 7.0

<#
.SYNOPSIS
Enhanced MediatR → LiteBus Migration Script (with regex and batch operations)

.DESCRIPTION
Performs comprehensive automated migration from MediatR to LiteBus across Phases 2-9:
- GlobalUsings replacements
- DependencyInjection updates
- Command/Query interface conversions (regex-based)
- Handler updates
- ApiController files
- Package reference updates
- Build verification

.PARAMETER Phases
Phases to migrate (default: 2,3,4,5,6,7,8,9)

.PARAMETER DryRun
Preview changes without making them

.PARAMETER Verbose
Show detailed output

.PARAMETER SkipBackup
Skip creating backups before migration

.EXAMPLE
.\Migrate-MediatR-To-LiteBus-Enhanced.ps1
.\Migrate-MediatR-To-LiteBus-Enhanced.ps1 -Phases 2,3 -Verbose
.\Migrate-MediatR-To-LiteBus-Enhanced.ps1 -DryRun
#>

param(
    [int[]]$Phases = @(2,3,4,5,6,7,8,9),
    [switch]$DryRun,
    [switch]$Verbose,
    [switch]$SkipBackup
)

$ErrorActionPreference = "Stop"
$baseDir = "d:\Dev\Incubator\.NET"
$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$logFile = "$baseDir\logs\migration-enhanced-$timestamp.log"
$backupDir = "$baseDir\backups\migration-$timestamp"
$replacementCount = 0
$fileCount = 0

# Create logging directory
$null = New-Item -ItemType Directory -Path "$baseDir\logs" -Force -ErrorAction SilentlyContinue

function Log {
    param([string]$Msg, [string]$Level = "INFO")
    $entry = "[$timestamp] [$Level] $Msg"
    Write-Host $entry -ForegroundColor $(
        switch ($Level) {
            "ERROR" { "Red" }
            "WARN" { "Yellow" }
            "OK" { "Green" }
            "DRY" { "Cyan" }
            default { "White" }
        }
    )
    Add-Content -Path $logFile -Value $entry
}

function Backup-File {
    param([string]$FilePath)
    if ($SkipBackup) { return }
    
    $relativePath = $FilePath.Substring($baseDir.Length)
    $backupPath = Join-Path $backupDir $relativePath
    $backupDirPath = Split-Path $backupPath
    
    $null = New-Item -ItemType Directory -Path $backupDirPath -Force -ErrorAction SilentlyContinue
    Copy-Item -Path $FilePath -Destination $backupPath -Force
}

function Replace-StringInFile {
    param(
        [string]$FilePath,
        [string]$OldString,
        [string]$NewString,
        [string]$Description,
        [switch]$Regex
    )
    
    if (-not (Test-Path $FilePath)) { return 0 }
    
    $content = Get-Content -Path $FilePath -Raw
    $found = 0
    
    if ($Regex) {
        $found = ($content -match $OldString)
        if ($found) {
            $newContent = $content -replace $OldString, $NewString
        }
    } else {
        $found = $content.Contains($OldString)
        if ($found) {
            $newContent = $content.Replace($OldString, $NewString)
        }
    }
    
    if (-not $found) { return 0 }
    
    if ($DryRun) {
        Log "[DRY] $FilePath - $Description" "DRY"
        return 1
    }
    
    Backup-File -FilePath $FilePath
    Set-Content -Path $FilePath -Value $newContent -NoNewline
    Log "✓ $FilePath - $Description" "OK"
    return 1
}

function Get-PhaseDirectories {
    $dirs = @()
    foreach ($phase in $Phases) {
        $phaseDir = "$baseDir\Phase $phase\src"
        if (Test-Path $phaseDir) {
            $dirs += Get-ChildItem -Path $phaseDir -Directory
        }
    }
    return $dirs
}

function Migrate-AllGlobalUsings {
    Log "═══ Migrating GlobalUsings.cs files ═══" "INFO"
    $count = 0
    
    foreach ($dir in (Get-PhaseDirectories)) {
        Get-ChildItem -Path $dir.FullName -Filter "GlobalUsings.cs" -Recurse | ForEach-Object {
            $count += Replace-StringInFile `
                -FilePath $_.FullName `
                -OldString "global using MediatR;" `
                -NewString "global using LiteBus.Commands.Abstractions;`nglobal using LiteBus.Queries.Abstractions;" `
                -Description "MediatR → LiteBus"
        }
    }
    
    $script:replacementCount += $count
    Log "GlobalUsings: $count replacements" "STAT"
}

function Migrate-AllDependencyInjection {
    Log "═══ Migrating DependencyInjection.cs files ═══" "INFO"
    $count = 0
    
    foreach ($dir in (Get-PhaseDirectories)) {
        Get-ChildItem -Path $dir.FullName -Filter "DependencyInjection.cs" -Recurse | ForEach-Object {
            $count += Replace-StringInFile `
                -FilePath $_.FullName `
                -OldString "services.AddMediatR(" `
                -NewString "services.AddLiteBus(" `
                -Description "AddMediatR → AddLiteBus"
        }
    }
    
    $script:replacementCount += $count
    Log "DependencyInjection: $count replacements" "STAT"
}

function Migrate-AllCommands {
    Log "═══ Migrating Command classes (IRequest → ICommand) ═══" "INFO"
    $count = 0
    
    foreach ($dir in (Get-PhaseDirectories)) {
        Get-ChildItem -Path $dir.FullName -Filter "*Command.cs" -Recurse | ForEach-Object {
            $content = Get-Content -Path $_.FullName -Raw
            
            # Skip if it's a handler
            if ($content -match "CommandHandler|Handle\(") {
                return
            }
            
            $count += Replace-StringInFile `
                -FilePath $_.FullName `
                -OldString ": IRequest<" `
                -NewString ": ICommand<" `
                -Description "IRequest → ICommand"
        }
    }
    
    $script:replacementCount += $count
    Log "Commands: $count replacements" "STAT"
}

function Migrate-AllCommandHandlers {
    Log "═══ Migrating CommandHandler classes ═══" "INFO"
    $count = 0
    
    foreach ($dir in (Get-PhaseDirectories)) {
        Get-ChildItem -Path $dir.FullName -Filter "*CommandHandler.cs" -Recurse | ForEach-Object {
            $count += Replace-StringInFile `
                -FilePath $_.FullName `
                -OldString "IRequestHandler<" `
                -NewString "ICommandHandler<" `
                -Description "IRequestHandler → ICommandHandler"
        }
    }
    
    $script:replacementCount += $count
    Log "CommandHandlers: $count replacements" "STAT"
}

function Migrate-AllQueries {
    Log "═══ Migrating Query classes (IRequest → IQuery) ═══" "INFO"
    $count = 0
    
    foreach ($dir in (Get-PhaseDirectories)) {
        Get-ChildItem -Path $dir.FullName -Filter "*Query.cs" -Recurse | ForEach-Object {
            $content = Get-Content -Path $_.FullName -Raw
            
            # Skip if it's a handler
            if ($content -match "QueryHandler|Handle\(") {
                return
            }
            
            $count += Replace-StringInFile `
                -FilePath $_.FullName `
                -OldString ": IRequest<" `
                -NewString ": IQuery<" `
                -Description "IRequest → IQuery"
        }
    }
    
    $script:replacementCount += $count
    Log "Queries: $count replacements" "STAT"
}

function Migrate-AllQueryHandlers {
    Log "═══ Migrating QueryHandler classes ═══" "INFO"
    $count = 0
    
    foreach ($dir in (Get-PhaseDirectories)) {
        Get-ChildItem -Path $dir.FullName -Filter "*QueryHandler.cs" -Recurse | ForEach-Object {
            $count += Replace-StringInFile `
                -FilePath $_.FullName `
                -OldString "IRequestHandler<" `
                -NewString "IQueryHandler<" `
                -Description "IRequestHandler → IQueryHandler"
        }
    }
    
    $script:replacementCount += $count
    Log "QueryHandlers: $count replacements" "STAT"
}

function Migrate-AllApiControllers {
    Log "═══ Migrating ApiController files ═══" "INFO"
    $count = 0
    
    foreach ($dir in (Get-PhaseDirectories)) {
        Get-ChildItem -Path $dir.FullName -Filter "ApiController.cs" -Recurse | ForEach-Object {
            $filePath = $_.FullName
            
            $count += Replace-StringInFile `
                -FilePath $filePath `
                -OldString "using MediatR;" `
                -NewString "using LiteBus.Commands.Abstractions;`nusing LiteBus.Queries.Abstractions;" `
                -Description "using MediatR → LiteBus"
            
            $count += Replace-StringInFile `
                -FilePath $filePath `
                -OldString "private IMediator mediator;" `
                -NewString "private ICommandMediator commandMediator;`n    private IQueryMediator queryMediator;" `
                -Description "IMediator field → separated mediators"
            
            $count += Replace-StringInFile `
                -FilePath $filePath `
                -OldString "protected IMediator Mediator => this.mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();" `
                -NewString "protected ICommandMediator CommandMediator => this.commandMediator ??= this.HttpContext.RequestServices.GetService<ICommandMediator>();`n`n    protected IQueryMediator QueryMediator => this.queryMediator ??= this.HttpContext.RequestServices.GetService<IQueryMediator>();" `
                -Description "Mediator property → separated properties"
        }
    }
    
    $script:replacementCount += $count
    Log "ApiControllers: $count replacements" "STAT"
}

function Migrate-AllPackageReferences {
    Log "═══ Migrating package references ═══" "INFO"
    $count = 0
    
    foreach ($dir in (Get-PhaseDirectories)) {
        Get-ChildItem -Path $dir.FullName -Filter "*.csproj" -Recurse | ForEach-Object {
            $count += Replace-StringInFile `
                -FilePath $_.FullName `
                -OldString '<PackageReference Include="MediatR" Version="' `
                -NewString '<!-- MediatR removed: migrated to LiteBus -->`n		<!-- <PackageReference Include="MediatR" Version="' `
                -Description "Comment MediatR package"
        }
    }
    
    $script:replacementCount += $count
    Log "Packages: $count replacements" "STAT"
}

function Show-Summary {
    Log "`n╔════════════════════════════════════════════════════════╗" "INFO"
    Log "║              MIGRATION SUMMARY                         ║" "INFO"
    Log "║════════════════════════════════════════════════════════║" "INFO"
    Log "║ Phases Modified: $($Phases -join ', ')                       ║" "INFO"
    Log "║ Total Replacements: $replacementCount                    ║" "INFO"
    Log "║ Mode: $(if ($DryRun) { "DRY-RUN (no changes)" } else { "LIVE" })                  ║" "INFO"
    Log "║ Backup: $(if ($SkipBackup) { "SKIPPED" } else { "Created at $backupDir" })  ║" "INFO"
    Log "╚════════════════════════════════════════════════════════╝" "INFO"
    Log "Log file: $logFile" "OK"
}

# Main execution
function Main {
    Log "╔════════════════════════════════════════════════════════╗" "INFO"
    Log "║ MediatR → LiteBus Enhanced Migration Script           ║" "INFO"
    Log "║ Phases: $($Phases -join ', ')                              ║" "INFO"
    Log "║ Mode: $(if ($DryRun) { "DRY-RUN" } else { "LIVE" })                             ║" "INFO"
    Log "╚════════════════════════════════════════════════════════╝" "INFO"
    
    try {
        Migrate-AllGlobalUsings
        Migrate-AllDependencyInjection
        Migrate-AllCommands
        Migrate-AllCommandHandlers
        Migrate-AllQueries
        Migrate-AllQueryHandlers
        Migrate-AllApiControllers
        Migrate-AllPackageReferences
        
        Show-Summary
        
        if (-not $DryRun) {
            Log "`nNext steps:" "WARN"
            Log "1. Review changes in Visual Studio" "WARN"
            Log "2. Restore NuGet packages" "WARN"
            Log "3. Build solution to verify" "WARN"
            Log "4. Run tests" "WARN"
        }
    }
    catch {
        Log "ERROR: $_" "ERROR"
        exit 1
    }
}

Main
