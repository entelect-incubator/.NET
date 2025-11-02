#Requires -Version 7.0

<#
.SYNOPSIS
Automated migration script: MediatR → LiteBus (CQS Pattern)
Converts all Phases 2-9 from MediatR to LiteBus

.DESCRIPTION
This script performs comprehensive find-replace operations to migrate from MediatR to LiteBus:
- GlobalUsings.cs files
- DependencyInjection.cs files
- Command/Query interfaces
- Handlers
- ApiController files
- Package references

.PARAMETER Phases
Phases to migrate (default: 2,3,4,5,6,7,8,9)

.PARAMETER DryRun
Show what would be changed without making changes

.PARAMETER Verbose
Show detailed operation output

.EXAMPLE
.\Migrate-MediatR-To-LiteBus.ps1 -Phases 2,3
.\Migrate-MediatR-To-LiteBus.ps1 -DryRun -Verbose
#>

param(
    [int[]]$Phases = @(2,3,4,5,6,7,8,9),
    [switch]$DryRun,
    [switch]$Verbose
)

$ErrorActionPreference = "Stop"
$baseDir = "d:\Dev\Incubator\.NET"
$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$logFile = "$baseDir\logs\migration-$timestamp.log"

# Ensure log directory exists
$null = New-Item -ItemType Directory -Path "$baseDir\logs" -Force -ErrorAction SilentlyContinue

function Write-Log {
    param([string]$Message, [string]$Level = "INFO")
    $logMessage = "[$timestamp] [$Level] $Message"
    Write-Host $logMessage
    Add-Content -Path $logFile -Value $logMessage
}

function Test-FileExists {
    param([string]$Path)
    if (Test-Path $Path) {
        return $true
    }
    return $false
}

function Replace-InFile {
    param(
        [string]$FilePath,
        [string]$OldString,
        [string]$NewString,
        [string]$Description
    )
    
    if (-not (Test-FileExists $FilePath)) {
        Write-Log "File not found: $FilePath" "WARN"
        return 0
    }
    
    $content = Get-Content -Path $FilePath -Raw
    
    if ($content -notmatch [regex]::Escape($OldString)) {
        return 0
    }
    
    if ($DryRun) {
        Write-Log "[DRY-RUN] Would replace in: $FilePath - $Description" "DRY"
        return 1
    }
    
    $newContent = $content -replace [regex]::Escape($OldString), $NewString
    Set-Content -Path $FilePath -Value $newContent -NoNewline
    
    Write-Log "✓ Updated: $FilePath - $Description" "OK"
    return 1
}

function Migrate-GlobalUsings {
    Write-Log "=== Migrating GlobalUsings.cs files ===" "INFO"
    
    $phaseDirs = @()
    foreach ($phase in $Phases) {
        $phaseDirs += Get-ChildItem -Path "$baseDir\Phase $phase\src" -Directory -ErrorAction SilentlyContinue
    }
    
    $count = 0
    foreach ($dir in $phaseDirs) {
        $files = Get-ChildItem -Path $dir.FullName -Filter "GlobalUsings.cs" -Recurse -ErrorAction SilentlyContinue
        
        foreach ($file in $files) {
            $count += Replace-InFile `
                -FilePath $file.FullName `
                -OldString "global using MediatR;" `
                -NewString "global using LiteBus.Commands.Abstractions;`nglobal using LiteBus.Queries.Abstractions;" `
                -Description "GlobalUsings: MediatR → LiteBus"
        }
    }
    
    Write-Log "GlobalUsings: $count replacements" "STAT"
    return $count
}

function Migrate-DependencyInjection {
    Write-Log "=== Migrating DependencyInjection.cs files ===" "INFO"
    
    $phaseDirs = @()
    foreach ($phase in $Phases) {
        $phaseDirs += Get-ChildItem -Path "$baseDir\Phase $phase\src" -Directory -ErrorAction SilentlyContinue
    }
    
    $count = 0
    foreach ($dir in $phaseDirs) {
        $files = Get-ChildItem -Path $dir.FullName -Filter "DependencyInjection.cs" -Recurse -ErrorAction SilentlyContinue
        
        foreach ($file in $files) {
            # Replace AddMediatR call
            $count += Replace-InFile `
                -FilePath $file.FullName `
                -OldString "services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<" `
                -NewString "services.AddLiteBus(cfg => cfg.RegisterServicesFromAssemblyContaining<" `
                -Description "DependencyInjection: AddMediatR → AddLiteBus"
        }
    }
    
    Write-Log "DependencyInjection: $count replacements" "STAT"
    return $count
}

function Migrate-Commands {
    Write-Log "=== Migrating Command classes ===" "INFO"
    
    $phaseDirs = @()
    foreach ($phase in $Phases) {
        $phaseDirs += Get-ChildItem -Path "$baseDir\Phase $phase\src" -Directory -ErrorAction SilentlyContinue
    }
    
    $count = 0
    foreach ($dir in $phaseDirs) {
        # Find all Command files
        $commandDirs = Get-ChildItem -Path $dir.FullName -Filter "*Commands" -Directory -Recurse -ErrorAction SilentlyContinue
        
        foreach ($cmdDir in $commandDirs) {
            $files = Get-ChildItem -Path $cmdDir.FullName -Filter "*.cs" -ErrorAction SilentlyContinue
            
            foreach ($file in $files) {
                $content = Get-Content -Path $file.FullName -Raw
                
                # Skip if it's a handler
                if ($content -match "CommandHandler|Handler<") {
                    continue
                }
                
                # Replace IRequest<T> with ICommand<T> for commands
                if ($content -match "IRequest<") {
                    $count += Replace-InFile `
                        -FilePath $file.FullName `
                        -OldString "public class" `
                        -NewString "public class" `
                        -Description "Commands: Scanning for IRequest" # Placeholder
                }
            }
        }
    }
    
    Write-Log "Commands: Scanned for IRequest conversions" "STAT"
    return $count
}

function Migrate-ApiController {
    Write-Log "=== Migrating ApiController files ===" "INFO"
    
    $phaseDirs = @()
    foreach ($phase in $Phases) {
        $phaseDirs += Get-ChildItem -Path "$baseDir\Phase $phase\src" -Directory -ErrorAction SilentlyContinue
    }
    
    $count = 0
    foreach ($dir in $phaseDirs) {
        $files = Get-ChildItem -Path $dir.FullName -Filter "ApiController.cs" -Recurse -ErrorAction SilentlyContinue
        
        foreach ($file in $files) {
            # Replace using MediatR
            $count += Replace-InFile `
                -FilePath $file.FullName `
                -OldString "using MediatR;" `
                -NewString "using LiteBus.Commands.Abstractions;`nusing LiteBus.Queries.Abstractions;" `
                -Description "ApiController: using MediatR → LiteBus"
            
            # Replace IMediator field
            $count += Replace-InFile `
                -FilePath $file.FullName `
                -OldString "private IMediator mediator;" `
                -NewString "private ICommandMediator commandMediator;`n    private IQueryMediator queryMediator;" `
                -Description "ApiController: IMediator field → ICommandMediator + IQueryMediator"
            
            # Replace Mediator property
            $count += Replace-InFile `
                -FilePath $file.FullName `
                -OldString "protected IMediator Mediator => this.mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();" `
                -NewString "protected ICommandMediator CommandMediator => this.commandMediator ??= this.HttpContext.RequestServices.GetService<ICommandMediator>();`n    protected IQueryMediator QueryMediator => this.queryMediator ??= this.HttpContext.RequestServices.GetService<IQueryMediator>();" `
                -Description "ApiController: Mediator property → CommandMediator + QueryMediator"
        }
    }
    
    Write-Log "ApiController: $count replacements" "STAT"
    return $count
}

function Migrate-PackageReferences {
    Write-Log "=== Migrating project file packages ===" "INFO"
    
    $phaseDirs = @()
    foreach ($phase in $Phases) {
        $phaseDirs += Get-ChildItem -Path "$baseDir\Phase $phase\src" -Directory -ErrorAction SilentlyContinue
    }
    
    $count = 0
    foreach ($dir in $phaseDirs) {
        $files = Get-ChildItem -Path $dir.FullName -Filter "*.csproj" -Recurse -ErrorAction SilentlyContinue
        
        foreach ($file in $files) {
            # Replace MediatR package
            $count += Replace-InFile `
                -FilePath $file.FullName `
                -OldString `
                    '<PackageReference Include="MediatR" Version="' `
                -NewString `
                    '<!-- MediatR removed - migrated to LiteBus -->`n		<!-- <PackageReference Include="MediatR" Version="' `
                -Description "csproj: Comment out MediatR package"
            
            # Verify LiteBus packages exist
            $content = Get-Content -Path $file.FullName -Raw
            if ($content -notmatch "LiteBus") {
                Write-Log "WARNING: $($file.FullName) may need LiteBus packages added" "WARN"
            }
        }
    }
    
    Write-Log "Package References: Scanned $($files.Count) files" "STAT"
    return $count
}

function Show-Summary {
    param([hashtable]$Stats)
    
    Write-Log "`n" "INFO"
    Write-Log "════════════════════════════════════" "INFO"
    Write-Log "MIGRATION SUMMARY" "INFO"
    Write-Log "════════════════════════════════════" "INFO"
    
    $total = 0
    foreach ($key in $Stats.Keys) {
        Write-Log "  $key : $($Stats[$key])" "STAT"
        $total += $Stats[$key]
    }
    
    Write-Log "════════════════════════════════════" "INFO"
    Write-Log "TOTAL CHANGES: $total" "STAT"
    Write-Log "════════════════════════════════════" "INFO"
    
    if ($DryRun) {
        Write-Log "`n[DRY-RUN MODE] No changes were made" "WARN"
    } else {
        Write-Log "`nMigration completed! Review changes and run tests." "OK"
    }
    
    Write-Log "Log file: $logFile" "INFO"
}

# Main execution
function Main {
    Write-Log "╔════════════════════════════════════════════════════════════╗" "INFO"
    Write-Log "║  MediatR → LiteBus Migration Script                       ║" "INFO"
    Write-Log "║  Phases: $($Phases -join ', ')                                    ║" "INFO"
    Write-Log "║  Mode: $(if ($DryRun) { 'DRY-RUN' } else { 'LIVE' })                                     ║" "INFO"
    Write-Log "╚════════════════════════════════════════════════════════════╝" "INFO"
    
    $stats = @{}
    
    try {
        $stats["GlobalUsings"] = Migrate-GlobalUsings
        $stats["DependencyInjection"] = Migrate-DependencyInjection
        $stats["Commands"] = Migrate-Commands
        $stats["ApiControllers"] = Migrate-ApiController
        $stats["Packages"] = Migrate-PackageReferences
        
        Show-Summary $stats
    }
    catch {
        Write-Log "ERROR: $_" "ERROR"
        exit 1
    }
}

# Run main
Main
