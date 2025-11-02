# Simple MediatR to LiteBus Migration Script - PowerShell 5.1 Compatible
# Usage: .\Migrate-Simple.ps1 -DryRun -Phases 2,3,4

param(
    [int[]]$Phases = @(2,3,4,5,6,7,8),
    [switch]$DryRun
)

$baseDir = "d:\Dev\Incubator\.NET"
$logDir = "$baseDir\logs"
$logFile = "$logDir\migration-$(Get-Date -Format 'yyyyMMdd-HHmmss').log"

# Ensure logs directory exists
if (-not (Test-Path $logDir)) {
    $null = New-Item -ItemType Directory -Path $logDir -Force
}

$totalChanges = 0

function LogMsg {
    param([string]$Msg)
    $entry = "$(Get-Date -Format 'HH:mm:ss') - $Msg"
    Write-Host $entry
    Add-Content -Path $logFile -Value $entry
}

function ReplaceInFile {
    param(
        [string]$Path,
        [string]$Find,
        [string]$Replace,
        [string]$Desc
    )
    
    if (-not (Test-Path $Path)) {
        return 0
    }
    
    $content = [System.IO.File]::ReadAllText($Path)
    if (-not $content.Contains($Find)) {
        return 0
    }
    
    if ($DryRun) {
        LogMsg "[DRY] $Path - $Desc"
        return 1
    }
    
    $newContent = $content.Replace($Find, $Replace)
    [System.IO.File]::WriteAllText($Path, $newContent)
    LogMsg "[OK] $Path - $Desc"
    return 1
}

# Main migration logic
Write-Host "`n=== MediatR to LiteBus Migration ===" -ForegroundColor Cyan
if ($DryRun) {
    Write-Host "Mode: DRY-RUN (preview only)`n" -ForegroundColor Yellow
} else {
    Write-Host "Mode: LIVE (applying changes)`n" -ForegroundColor Green
}

foreach ($phaseNum in $Phases) {
    Write-Host "`n--- Phase $phaseNum ---" -ForegroundColor Magenta
    $phaseDir = "$baseDir\Phase $phaseNum\src"
    
    if (-not (Test-Path $phaseDir)) {
        Write-Host "Not found: $phaseDir" -ForegroundColor Red
        continue
    }
    
    $phaseCount = 0
    
    # 1. GlobalUsings.cs files
    Get-ChildItem -Path $phaseDir -Filter "GlobalUsings.cs" -Recurse | ForEach-Object {
        $count = ReplaceInFile -Path $_.FullName `
            -Find "global using MediatR;" `
            -Replace "global using LiteBus.Commands.Abstractions;`nglobal using LiteBus.Queries.Abstractions;" `
            -Desc "GlobalUsings"
        $phaseCount += $count
    }
    
    # 2. ApiController.cs files
    Get-ChildItem -Path $phaseDir -Filter "ApiController.cs" -Recurse | ForEach-Object {
        $count = ReplaceInFile -Path $_.FullName `
            -Find "using MediatR;" `
            -Replace "using LiteBus.Commands.Abstractions;`nusing LiteBus.Queries.Abstractions;" `
            -Desc "ApiController using"
        $phaseCount += $count
        
        $count = ReplaceInFile -Path $_.FullName `
            -Find "private IMediator mediator;" `
            -Replace "private ICommandMediator _cmdMediator;`n`t`tprivate IQueryMediator _qryMediator;" `
            -Desc "ApiController fields"
        $phaseCount += $count
        
        $count = ReplaceInFile -Path $_.FullName `
            -Find "protected IMediator Mediator => this.mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();" `
            -Replace "protected ICommandMediator CmdMediator => _cmdMediator ??= HttpContext.RequestServices.GetRequiredService<ICommandMediator>();`n`n`t`tprotected IQueryMediator QryMediator => _qryMediator ??= HttpContext.RequestServices.GetRequiredService<IQueryMediator>();" `
            -Desc "ApiController props"
        $phaseCount += $count
    }
    
    # 3. Command files (not handlers)
    Get-ChildItem -Path $phaseDir -Filter "*Command.cs" -Recurse | ForEach-Object {
        $fileContent = [System.IO.File]::ReadAllText($_.FullName)
        if ($fileContent -match "Handler|IRequestHandler") {
            return
        }
        
        $count = ReplaceInFile -Path $_.FullName `
            -Find ": IRequest<" `
            -Replace ": ICommand<" `
            -Desc "IRequest->ICommand"
        $phaseCount += $count
    }
    
    # 4. CommandHandler files
    Get-ChildItem -Path $phaseDir -Filter "*CommandHandler.cs" -Recurse | ForEach-Object {
        $count = ReplaceInFile -Path $_.FullName `
            -Find "IRequestHandler<" `
            -Replace "ICommandHandler<" `
            -Desc "IRequestHandler->ICommandHandler"
        $phaseCount += $count
        
        $content = [System.IO.File]::ReadAllText($_.FullName)
        if ($content -match "public async Task.* Handle\(") {
            $newContent = $content -replace "(\s+)public async Task(.+?) Handle\(", '${1}public async Task${2} HandleAsync('
            if ($DryRun) {
                LogMsg "[DRY] $_ - Handle->HandleAsync"
                $phaseCount += 1
            } else {
                [System.IO.File]::WriteAllText($_.FullName, $newContent)
                LogMsg "[OK] $_ - Handle->HandleAsync"
                $phaseCount += 1
            }
        }
    }
    
    # 5. Query files (not handlers)
    Get-ChildItem -Path $phaseDir -Filter "*Query.cs" -Recurse | ForEach-Object {
        $fileContent = [System.IO.File]::ReadAllText($_.FullName)
        if ($fileContent -match "Handler|IRequestHandler") {
            return
        }
        
        $count = ReplaceInFile -Path $_.FullName `
            -Find ": IRequest<" `
            -Replace ": IQuery<" `
            -Desc "IRequest->IQuery"
        $phaseCount += $count
    }
    
    # 6. QueryHandler files
    Get-ChildItem -Path $phaseDir -Filter "*QueryHandler.cs" -Recurse | ForEach-Object {
        $count = ReplaceInFile -Path $_.FullName `
            -Find "IRequestHandler<" `
            -Replace "IQueryHandler<" `
            -Desc "IRequestHandler->IQueryHandler"
        $phaseCount += $count
        
        $content = [System.IO.File]::ReadAllText($_.FullName)
        if ($content -match "public async Task.* Handle\(") {
            $newContent = $content -replace "(\s+)public async Task(.+?) Handle\(", '${1}public async Task${2} HandleAsync('
            if ($DryRun) {
                LogMsg "[DRY] $_ - Handle->HandleAsync"
                $phaseCount += 1
            } else {
                [System.IO.File]::WriteAllText($_.FullName, $newContent)
                LogMsg "[OK] $_ - Handle->HandleAsync"
                $phaseCount += 1
            }
        }
    }
    
    Write-Host "Phase $($phaseNum): $($phaseCount) changes" -ForegroundColor Green
    $totalChanges += $phaseCount
}

Write-Host "`n=== COMPLETE ===" -ForegroundColor Cyan
Write-Host "Total changes: $totalChanges" -ForegroundColor Green
Write-Host "Log: $logFile`n" -ForegroundColor White
