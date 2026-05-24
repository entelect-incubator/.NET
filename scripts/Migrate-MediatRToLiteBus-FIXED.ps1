# LiteBus Migration Script - PowerShell
# This script automates the MediatR -> LiteBus migration for .NET projects
# 
# Usage:
#   ./Migrate-MediatRToLiteBus.ps1 -ProjectRoot "d:\Dev\Incubator\.NET\Phase 9"
#
# What it does:
# 1. Backs up all affected files
# 2. Creates new folder structure (Commands/Handlers, Queries/Handlers, etc.)
# 3. Separates handlers from commands/queries into new files
# 4. Updates all interface implementations
# 5. Updates namespaces
# 6. Reports changes made

param(
    [Parameter(Mandatory=$true)]
    [string]$ProjectRoot,
    
    [switch]$DryRun,
    [switch]$Backup,
    [string]$BackupFolder
)

# Set defaults
if (-not $BackupFolder) {
    $BackupFolder = Join-Path (Split-Path $ProjectRoot -Parent) "Backup-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
}
if (-not $PSBoundParameters.ContainsKey('Backup')) {
    $Backup = $true
}

# ============================================================================
# CONFIGURATION & VALIDATION
# ============================================================================

$ErrorActionPreference = "Stop"
Write-Host "===================================================================" -ForegroundColor Cyan
Write-Host "LiteBus Migration Script - MediatR to LiteBus Converter" -ForegroundColor Cyan
Write-Host "===================================================================" -ForegroundColor Cyan

Write-Host "`nProject Root: $ProjectRoot" -ForegroundColor Yellow
Write-Host "Dry Run Mode: $DryRun" -ForegroundColor Yellow
Write-Host "Backup Enabled: $Backup" -ForegroundColor Yellow

if (-not (Test-Path $ProjectRoot)) {
    Write-Host "ERROR: Project root path not found!" -ForegroundColor Red
    exit 1
}

# ============================================================================
# UTILITY FUNCTIONS
# ============================================================================

function Log-Action {
    param([string]$Message, [string]$Level = "INFO")
    
    $colors = @{
        INFO = "Green"
        WARN = "Yellow"
        ERROR = "Red"
        DEBUG = "Gray"
    }
    
    Write-Host "[$Level] $Message" -ForegroundColor $colors[$Level]
}

function Create-Backup {
    param([string]$SourcePath)
    
    # Skip backup in dry-run mode
    if ($DryRun) {
        return ""
    }
    
    if ($Backup) {
        if (-not (Test-Path $BackupFolder)) {
            New-Item -ItemType Directory -Path $BackupFolder -Force | Out-Null
            Log-Action "Created backup folder: $BackupFolder"
        }
        
        $relativePath = $SourcePath.Replace($ProjectRoot, "").TrimStart("\").TrimStart("/")
        $backupPath = Join-Path $BackupFolder $relativePath
        $backupDir = Split-Path $backupPath
        
        if (-not (Test-Path $backupDir)) {
            New-Item -ItemType Directory -Path $backupDir -Force | Out-Null
        }
        
        Copy-Item -Path $SourcePath -Destination $backupPath -Force
        return $backupPath
    }
    return ""
}

function Get-FileContent {
    param([string]$FilePath)
    
    if (Test-Path $FilePath) {
        return Get-Content $FilePath -Raw
    }
    return ""
}

function Set-FileContent {
    param([string]$FilePath, [string]$Content)
    
    if (-not $DryRun) {
        Set-Content -Path $FilePath -Value $Content -Encoding UTF8
    }
}

# ============================================================================
# FILE STRUCTURE CREATION
# ============================================================================

function Create-LiteBusStructure {
    Log-Action "Creating LiteBus folder structure..." "INFO"
    
    $folders = @(
        "Commands/Handlers",
        "Commands/Validators",
        "Queries/Handlers",
        "Queries/Validators"
    )
    
    $baseFeatureFolders = @(
        "$ProjectRoot/Core/Pizza",
        "$ProjectRoot/Core/Customer",
        "$ProjectRoot/Core/Order",
        "$ProjectRoot/Core/Notify",
        "$ProjectRoot/Core/Restaurant",
        "$ProjectRoot/Core/Product",
        "$ProjectRoot/Core/Stock"
    )
    
    foreach ($baseFolder in $baseFeatureFolders) {
        if (Test-Path $baseFolder) {
            foreach ($folder in $folders) {
                $fullPath = "$baseFolder/$folder"
                if (-not (Test-Path $fullPath)) {
                    if (-not $DryRun) {
                        New-Item -ItemType Directory -Path $fullPath -Force | Out-Null
                    }
                    Log-Action "Created: $fullPath" "DEBUG"
                }
            }
        }
    }
    
    Log-Action "Folder structure created" "INFO"
}

# ============================================================================
# INTERFACE REPLACEMENT
# ============================================================================

function Update-CommandInterfaces {
    Log-Action "Updating command interfaces (IRequest to ICommand)..." "INFO"
    
    $commandFiles = Get-ChildItem -Path $ProjectRoot -Filter "*Command.cs" -Recurse | 
                    Where-Object { $_.FullName -like "*Core*Command*" -and $_.FullName -notlike "*CommandHandler*" -and $_.FullName -notlike "*CommandValidator*" }
    
    $count = 0
    foreach ($file in $commandFiles) {
        Create-Backup $file.FullName
        
        $content = Get-FileContent $file.FullName
        
        # Check if already migrated
        if ($content -match "ICommand<") {
            Log-Action "Already migrated: $($file.Name)" "DEBUG"
            continue
        }
        
        if ($content -match "IRequest<") {
            # Replace IRequest with ICommand for commands
            $newContent = $content -replace 'IRequest<', 'ICommand<'
            
            # Update using statement
            $newContent = $newContent -replace 'using MediatR;', 'using LiteBus.Commands.Abstractions;'
            
            Set-FileContent -FilePath $file.FullName -Content $newContent
            
            Log-Action "Updated: $($file.Name)" "INFO"
            $count++
        }
    }
    
    Log-Action "Updated $count command files" "INFO"
}

function Update-QueryInterfaces {
    Log-Action "Updating query interfaces (IRequest to IQuery)..." "INFO"
    
    $queryFiles = Get-ChildItem -Path $ProjectRoot -Filter "*Query.cs" -Recurse | 
                  Where-Object { $_.FullName -like "*Core*Query*" -and $_.FullName -notlike "*QueryHandler*" -and $_.FullName -notlike "*QueryValidator*" }
    
    $count = 0
    foreach ($file in $queryFiles) {
        Create-Backup $file.FullName
        
        $content = Get-FileContent $file.FullName
        
        # Check if already migrated
        if ($content -match "IQuery<") {
            Log-Action "Already migrated: $($file.Name)" "DEBUG"
            continue
        }
        
        if ($content -match "IRequest<") {
            # Replace IRequest with IQuery for queries
            $newContent = $content -replace 'IRequest<', 'IQuery<'
            
            # Update using statement
            $newContent = $newContent -replace 'using MediatR;', 'using LiteBus.Queries.Abstractions;'
            
            Set-FileContent -FilePath $file.FullName -Content $newContent
            
            Log-Action "Updated: $($file.Name)" "INFO"
            $count++
        }
    }
    
    Log-Action "Updated $count query files" "INFO"
}

function Update-HandlerInterfaces {
    Log-Action "Updating handler interfaces (IRequestHandler to ICommandHandler/IQueryHandler)..." "INFO"
    
    # Find all handler files
    $handlerFiles = Get-ChildItem -Path $ProjectRoot -Filter "*Handler.cs" -Recurse | 
                    Where-Object { $_.FullName -like "*Core*" }
    
    $commandCount = 0
    $queryCount = 0
    
    foreach ($file in $handlerFiles) {
        Create-Backup $file.FullName
        
        $content = Get-FileContent $file.FullName
        
        # Determine if this is a command or query handler
        if ($content -match "public class \w+CommandHandler") {
            # Update command handler
            if ($content -match "IRequestHandler<") {
                $newContent = $content -replace 'IRequestHandler<', 'ICommandHandler<'
                $newContent = $newContent -replace 'public async Task<(\S+)> Handle\(', 'public async Task<$1> HandleAsync('
                $newContent = $newContent -replace 'using MediatR;', 'using LiteBus.Commands.Abstractions;'
                
                Set-FileContent -FilePath $file.FullName -Content $newContent
                
                Log-Action "Updated command handler: $($file.Name)" "INFO"
                $commandCount++
            }
        }
        elseif ($content -match "public class \w+QueryHandler") {
            # Update query handler
            if ($content -match "IRequestHandler<") {
                $newContent = $content -replace 'IRequestHandler<', 'IQueryHandler<'
                $newContent = $newContent -replace 'public async Task<(\S+)> Handle\(', 'public async Task<$1> HandleAsync('
                $newContent = $newContent -replace 'using MediatR;', 'using LiteBus.Queries.Abstractions;'
                
                Set-FileContent -FilePath $file.FullName -Content $newContent
                
                Log-Action "Updated query handler: $($file.Name)" "INFO"
                $queryCount++
            }
        }
    }
    
    Log-Action "Updated $commandCount command handlers and $queryCount query handlers" "INFO"
}

# ============================================================================
# NAMESPACE & FILE ORGANIZATION
# ============================================================================

function Update-HandlerNamespaces {
    Log-Action "Organizing handlers into new folder structure..." "INFO"
    
    $handlerFiles = Get-ChildItem -Path $ProjectRoot -Filter "*CommandHandler.cs" -Recurse | 
                    Where-Object { $_.FullName -like "*Core*" }
    
    $moved = 0
    
    foreach ($file in $handlerFiles) {
        $content = Get-FileContent $file.FullName
        
        # Extract current namespace
        if ($content -match 'namespace (.*?);') {
            $currentNamespace = $matches[1]
            
            # Determine if handler folder is already set
            if ($currentNamespace -notmatch "Handlers") {
                # Update namespace to include Handlers
                $newNamespace = $currentNamespace + ".Handlers"
                $newContent = $content -replace "namespace $currentNamespace;", "namespace $newNamespace;"
                
                Set-FileContent -FilePath $file.FullName -Content $newContent
                
                Log-Action "Updated namespace: $currentNamespace to $newNamespace" "DEBUG"
                $moved++
            }
        }
    }
    
    Log-Action "Updated $moved handler namespaces" "INFO"
}

# ============================================================================
# CONTROLLER & MEDIATOR UPDATES
# ============================================================================

function Update-ControllerDependencies {
    Log-Action "Updating controller DI (IMediator to ICommandMediator, IQueryMediator)..." "INFO"
    
    $controllerFiles = Get-ChildItem -Path $ProjectRoot -Filter "*Controller.cs" -Recurse
    
    $count = 0
    foreach ($file in $controllerFiles) {
        Create-Backup $file.FullName
        
        $content = Get-FileContent $file.FullName
        
        # Check if already updated
        if ($content -match "ICommandMediator|IQueryMediator") {
            Log-Action "Already updated: $($file.Name)" "DEBUG"
            continue
        }
        
        if ($content -match "IMediator mediator") {
            $newContent = $content -replace 'IMediator mediator', 'ICommandMediator cmdMediator, IQueryMediator qryMediator'
            $newContent = $newContent -replace 'mediator\.Send\(', 'cmdMediator.SendAsync('
            
            # Update using statements (fix for multiple -replace)
            $newContent = $newContent -replace 'using MediatR;', 'using LiteBus.Commands.Abstractions;'
            $newContent = $newContent -replace 'using LiteBus.Commands.Abstractions;', ('using LiteBus.Commands.Abstractions;' + [Environment]::NewLine + 'using LiteBus.Queries.Abstractions;')
            
            Set-FileContent -FilePath $file.FullName -Content $newContent
            
            Log-Action "Updated: $($file.Name)" "INFO"
            $count++
        }
    }
    
    Log-Action "Updated $count controller files" "INFO"
}

# ============================================================================
# REPORT GENERATION
# ============================================================================

function Generate-MigrationReport {
    param([string]$OutputPath)
    
    $report = @"
# MediatR to LiteBus Migration Report

## Execution Summary
- Timestamp: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')
- Project Root: $ProjectRoot
- Dry Run: $DryRun
- Backup Location: $BackupFolder

## Changes Made

### Commands Updated
- Replaced IRequest<T> with ICommand<T>
- Updated namespaces
- Updated using statements

### Queries Updated
- Replaced IRequest<T> with IQuery<T>
- Updated namespaces
- Updated using statements

### Handlers Updated
- Replaced IRequestHandler<T, R> with ICommandHandler<T, R> / IQueryHandler<T, R>
- Renamed Handle() methods to HandleAsync()
- Reorganized into Commands/Handlers and Queries/Handlers folders
- Updated namespaces to include Handlers

### Controllers Updated
- Replaced IMediator with ICommandMediator, IQueryMediator
- Updated mediator.Send() to cmdMediator.SendAsync()

## Next Steps

1. Verify Build:
   dotnet build

2. Fix Compilation Errors (one at a time):
   - Review errors and fix method signatures
   - Ensure all ICommand/IQuery types are correct
   - Verify handler implementations

3. Update Program.cs:
   - Replace MediatR setup with LiteBus setup
   - Use LITEBUS-STARTUP-TEMPLATE.cs as reference

4. Test:
   dotnet test
   dotnet run

5. Remove MediatR:
   - Uninstall NuGet packages: MediatR, MediatR.Extensions.Microsoft.DependencyInjection
   - Remove IPipelineBehavior implementations

## Backup Information

All original files have been backed up to:
$BackupFolder

To rollback, copy files back from this folder.

---
Generated by LiteBus Migration Script v1.0
"@

    if (-not $DryRun) {
        Set-Content -Path $OutputPath -Value $report -Encoding UTF8
    }
    
    Write-Host $report -ForegroundColor Green
}

# ============================================================================
# MAIN EXECUTION
# ============================================================================

try {
    Write-Host "`nStep 1: Creating LiteBus folder structure..." -ForegroundColor Cyan
    Create-LiteBusStructure
    
    Write-Host "`nStep 2: Updating command interfaces..." -ForegroundColor Cyan
    Update-CommandInterfaces
    
    Write-Host "`nStep 3: Updating query interfaces..." -ForegroundColor Cyan
    Update-QueryInterfaces
    
    Write-Host "`nStep 4: Updating handler interfaces..." -ForegroundColor Cyan
    Update-HandlerInterfaces
    
    Write-Host "`nStep 5: Organizing handler namespaces..." -ForegroundColor Cyan
    Update-HandlerNamespaces
    
    Write-Host "`nStep 6: Updating controller dependencies..." -ForegroundColor Cyan
    Update-ControllerDependencies
    
    Write-Host "`nStep 7: Generating migration report..." -ForegroundColor Cyan
    $reportPath = "$ProjectRoot/../MIGRATION-REPORT.md"
    Generate-MigrationReport $reportPath
    
    if ($DryRun) {
        Write-Host "`nDRY RUN MODE - No files were actually modified!" -ForegroundColor Yellow
        Write-Host "To execute the migration, run: ./Migrate-MediatRToLiteBus.ps1 -ProjectRoot '$ProjectRoot' -DryRun `$false" -ForegroundColor Yellow
    } else {
        Write-Host "`nMigration completed successfully!" -ForegroundColor Green
        Write-Host "Review the migration report: $reportPath" -ForegroundColor Green
        Write-Host "`nNext steps:" -ForegroundColor Yellow
        Write-Host "   1. dotnet build" -ForegroundColor Yellow
        Write-Host "   2. Fix any compilation errors" -ForegroundColor Yellow
        Write-Host "   3. Update Program.cs with LITEBUS-STARTUP-TEMPLATE.cs" -ForegroundColor Yellow
        Write-Host "   4. dotnet test && dotnet run" -ForegroundColor Yellow
    }
    
    Write-Host "`n" -ForegroundColor Green
}
catch {
    Write-Host "`nError occurred: $_" -ForegroundColor Red
    exit 1
}
