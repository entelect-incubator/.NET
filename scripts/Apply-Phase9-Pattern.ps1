<#
.SYNOPSIS
Copy Phase 9 LiteBus pattern to Phases 2-8

.DESCRIPTION
Uses Phase 9 as the reference implementation (already migrated to LiteBus)
and applies the same pattern to other phases.

.EXAMPLE
.\Apply-Phase9-Pattern-To-Phases.ps1 -DryRun
.\Apply-Phase9-Pattern-To-Phases.ps1
#>

param(
    [int[]]$Phases = @(2,3,4,5,6,7,8),
    [switch]$DryRun,
    [switch]$Verbose
)

$ErrorActionPreference = "Stop"
$baseDir = "d:\Dev\Incubator\.NET"
$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$logFile = "$baseDir\logs\phase9-pattern-$timestamp.log"
$changes = 0

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

function Replace-InFile {
    param(
        [string]$FilePath,
        [string]$OldString,
        [string]$NewString,
        [string]$Description
    )
    
    if (-not (Test-Path $FilePath)) { return 0 }
    
    $content = Get-Content -Path $FilePath -Raw
    if (-not $content.Contains($OldString)) { return 0 }
    
    if ($DryRun) {
        Log "[DRY] $FilePath - $Description" "DRY"
        return 1
    }
    
    $newContent = $content.Replace($OldString, $NewString)
    Set-Content -Path $FilePath -Value $newContent -NoNewline
    Log "✓ $FilePath - $Description" "OK"
    return 1
}

function Migrate-Phase {
    param([int]$PhaseNum)
    
    Log "═══ Phase $PhaseNum ═══" "INFO"
    $phaseDir = "$baseDir\Phase $PhaseNum\src"
    if (-not (Test-Path $phaseDir)) {
        Log "Phase $PhaseNum not found" "WARN"
        return
    }
    
    $cnt = 0
    
    # Update GlobalUsings files
    Get-ChildItem -Path $phaseDir -Filter "GlobalUsings.cs" -Recurse | ForEach-Object {
        $cnt += Replace-InFile `
            -FilePath $_.FullName `
            -OldString "global using MediatR;" `
            -NewString "global using LiteBus.Commands.Abstractions;`nglobal using LiteBus.Queries.Abstractions;" `
            -Description "GlobalUsings"
    }
    
    # Update ApiController files
    Get-ChildItem -Path $phaseDir -Filter "ApiController.cs" -Recurse | ForEach-Object {
        $cnt += Replace-InFile `
            -FilePath $_.FullName `
            -OldString "using MediatR;" `
            -NewString "using LiteBus.Commands.Abstractions;`nusing LiteBus.Queries.Abstractions;" `
            -Description "ApiController using"
        
        $cnt += Replace-InFile `
            -FilePath $_.FullName `
            -OldString "private IMediator mediator;`n`n	protected IMediator Mediator => this.mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();" `
            -NewString "private ICommandMediator _cmdMediator;`n	private IQueryMediator _qryMediator;`n`n	protected ICommandMediator CmdMediator => _cmdMediator ??= HttpContext.RequestServices.GetRequiredService<ICommandMediator>();`n	protected IQueryMediator QryMediator => _qryMediator ??= HttpContext.RequestServices.GetRequiredService<IQueryMediator>();" `
            -Description "ApiController properties"
        
        $cnt += Replace-InFile `
            -FilePath $_.FullName `
            -OldString "private IMediator mediator;`n    `n    protected IMediator Mediator`n        => this.mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();" `
            -NewString "private ICommandMediator _cmdMediator;`n	private IQueryMediator _qryMediator;`n`n	protected ICommandMediator CmdMediator => _cmdMediator ??= HttpContext.RequestServices.GetRequiredService<ICommandMediator>();`n	protected IQueryMediator QryMediator => _qryMediator ??= HttpContext.RequestServices.GetRequiredService<IQueryMediator>();" `
            -Description "ApiController properties (alt format)"
    }
    
    # Update DependencyInjection
    Get-ChildItem -Path $phaseDir -Filter "DependencyInjection.cs" -Recurse | ForEach-Object {
        $cnt += Replace-InFile `
            -FilePath $_.FullName `
            -OldString "services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<" `
            -NewString "// Migrated to LiteBus - manual handler registration`n		var assembly = typeof(" `
            -Description "DependencyInjection AddMediatR"
    }
    
    # Update Command classes
    Get-ChildItem -Path $phaseDir -Filter "*Command.cs" -Recurse | ForEach-Object {
        $content = Get-Content -Path $_.FullName -Raw
        if ($content -match "CommandHandler|Handle\(" -or $content -match "IRequestHandler") {
            return  # Skip handlers
        }
        
        $cnt += Replace-InFile `
            -FilePath $_.FullName `
            -OldString ": IRequest<" `
            -NewString ": ICommand<" `
            -Description "IRequest → ICommand"
    }
    
    # Update CommandHandlers
    Get-ChildItem -Path $phaseDir -Filter "*CommandHandler.cs" -Recurse | ForEach-Object {
        $cnt += Replace-InFile `
            -FilePath $_.FullName `
            -OldString "IRequestHandler<" `
            -NewString "ICommandHandler<" `
            -Description "IRequestHandler → ICommandHandler"
        
        $cnt += Replace-InFile `
            -FilePath $_.FullName `
            -OldString " Handle(" `
            -NewString " HandleAsync(" `
            -Description "Handle → HandleAsync"
    }
    
    # Update Query classes
    Get-ChildItem -Path $phaseDir -Filter "*Query.cs" -Recurse | ForEach-Object {
        $content = Get-Content -Path $_.FullName -Raw
        if ($content -match "QueryHandler|Handle\(" -or $content -match "IRequestHandler") {
            return  # Skip handlers
        }
        
        $cnt += Replace-InFile `
            -FilePath $_.FullName `
            -OldString ": IRequest<" `
            -NewString ": IQuery<" `
            -Description "IRequest → IQuery"
    }
    
    # Update QueryHandlers
    Get-ChildItem -Path $phaseDir -Filter "*QueryHandler.cs" -Recurse | ForEach-Object {
        $cnt += Replace-InFile `
            -FilePath $_.FullName `
            -OldString "IRequestHandler<" `
            -NewString "IQueryHandler<" `
            -Description "IRequestHandler → IQueryHandler"
        
        $cnt += Replace-InFile `
            -FilePath $_.FullName `
            -OldString " Handle(" `
            -NewString " HandleAsync(" `
            -Description "Handle → HandleAsync"
    }
    
    Log "Phase $PhaseNum: $cnt replacements" "STAT"
    $script:changes += $cnt
}

# Main
function Main {
    Log "╔════════════════════════════════════════════════╗" "INFO"
    Log "║ Applying Phase 9 LiteBus Pattern              ║" "INFO"
    Log "║ Target Phases: $($Phases -join ', ')              ║" "INFO"
    Log "║ Mode: $(if ($DryRun) { "DRY-RUN" } else { "LIVE" })                ║" "INFO"
    Log "╚════════════════════════════════════════════════╝" "INFO"
    
    try {
        foreach ($phase in $Phases) {
            Migrate-Phase -PhaseNum $phase
        }
        
        Log "`n╔════════════════════════════════════════════════╗" "INFO"
        Log "║ SUMMARY                                        ║" "INFO"
        Log "║ Total Changes: $changes                             ║" "INFO"
        if ($DryRun) {
            Log "║ Mode: DRY-RUN (no changes made)              ║" "INFO"
        }
        Log "╚════════════════════════════════════════════════╝" "INFO"
        Log "Log: $logFile" "OK"
    }
    catch {
        Log "ERROR: $_" "ERROR"
        exit 1
    }
}

Main
