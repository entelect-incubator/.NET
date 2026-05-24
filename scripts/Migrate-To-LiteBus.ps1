param(
    [int[]]$Phases = @(2,3,4,5,6,7,8),
    [switch]$DryRun,
    [switch]$Verbose
)

$baseDir = "d:\Dev\Incubator\.NET"
$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$logFile = "$baseDir\logs\migration-$timestamp.log"
$totalChanges = 0

$null = New-Item -ItemType Directory -Path "$baseDir\logs" -Force -ErrorAction SilentlyContinue

function Log {
    param([string]$Message, [string]$Level = "INFO")
    $timeStr = Get-Date -Format "HH:mm:ss"
    $logEntry = "[$timeStr] [$Level] $Message"
    Write-Host $logEntry
    Add-Content -Path $logFile -Value $logEntry
}

function Replace-InFile {
    param(
        [string]$FilePath,
        [string]$OldString,
        [string]$NewString,
        [string]$Description
    )
    
    if (-not (Test-Path $FilePath)) {
        return 0
    }
    
    $content = Get-Content -Path $FilePath -Raw
    if (-not $content.Contains($OldString)) {
        return 0
    }
    
    if ($DryRun) {
        Log "[DRY] $FilePath :: $Description" "DRY"
        return 1
    }
    
    $newContent = $content.Replace($OldString, $NewString)
    Set-Content -Path $FilePath -Value $newContent -NoNewline
    Log "OK: $FilePath :: $Description" "DONE"
    return 1
}

function Migrate-Phase {
    param([int]$PhaseNum)
    
    Log "=== Migrating Phase $PhaseNum ===" "PHASE"
    $phaseDir = "$baseDir\Phase $PhaseNum\src"
    if (-not (Test-Path $phaseDir)) {
        Log "Phase $PhaseNum not found at $phaseDir" "WARN"
        return 0
    }
    
    $phaseChanges = 0
    
    # 1. Update GlobalUsings.cs - MediatR to LiteBus
    $globalUsingsFiles = Get-ChildItem -Path $phaseDir -Filter "GlobalUsings.cs" -Recurse -ErrorAction SilentlyContinue
    foreach ($file in $globalUsingsFiles) {
        $result = Replace-InFile -FilePath $file.FullName `
            -OldString "global using MediatR;" `
            -NewString "global using LiteBus.Commands.Abstractions;`nglobal using LiteBus.Queries.Abstractions;" `
            -Description "GlobalUsings: MediatR -> LiteBus"
        $phaseChanges += $result
    }
    
    # 2. Update ApiController.cs - using statements
    $apiControllerFiles = Get-ChildItem -Path $phaseDir -Filter "ApiController.cs" -Recurse -ErrorAction SilentlyContinue
    foreach ($file in $apiControllerFiles) {
        # Remove MediatR using
        $result = Replace-InFile -FilePath $file.FullName `
            -OldString "using MediatR;" `
            -NewString "using LiteBus.Commands.Abstractions;`nusing LiteBus.Queries.Abstractions;" `
            -Description "ApiController: using MediatR -> LiteBus"
        $phaseChanges += $result
        
        # Replace IMediator properties
        $result = Replace-InFile -FilePath $file.FullName `
            -OldString "private IMediator mediator;" `
            -NewString "private ICommandMediator _cmdMediator;`n`t`tprivate IQueryMediator _qryMediator;" `
            -Description "ApiController: mediator fields"
        $phaseChanges += $result
        
        # Replace Mediator property
        $result = Replace-InFile -FilePath $file.FullName `
            -OldString "protected IMediator Mediator => this.mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();" `
            -NewString "protected ICommandMediator CmdMediator => _cmdMediator ??= HttpContext.RequestServices.GetRequiredService<ICommandMediator>();`n`n`t`tprotected IQueryMediator QryMediator => _qryMediator ??= HttpContext.RequestServices.GetRequiredService<IQueryMediator>();" `
            -Description "ApiController: Mediator property"
        $phaseChanges += $result
    }
    
    # 3. Update *Command.cs files - IRequest to ICommand
    $commandFiles = Get-ChildItem -Path $phaseDir -Filter "*Command.cs" -Recurse -ErrorAction SilentlyContinue
    foreach ($file in $commandFiles) {
        $content = Get-Content -Path $file.FullName -Raw
        # Skip CommandHandler files
        if ($content -match "Handler|Handle\(" -or $content -match "IRequestHandler") {
            continue
        }
        
        $result = Replace-InFile -FilePath $file.FullName `
            -OldString ": IRequest<" `
            -NewString ": ICommand<" `
            -Description "Command: IRequest -> ICommand"
        $phaseChanges += $result
    }
    
    # 4. Update *CommandHandler.cs files
    $commandHandlerFiles = Get-ChildItem -Path $phaseDir -Filter "*CommandHandler.cs" -Recurse -ErrorAction SilentlyContinue
    foreach ($file in $commandHandlerFiles) {
        # Replace IRequestHandler with ICommandHandler
        $result = Replace-InFile -FilePath $file.FullName `
            -OldString "IRequestHandler<" `
            -NewString "ICommandHandler<" `
            -Description "CommandHandler: IRequestHandler -> ICommandHandler"
        $phaseChanges += $result
        
        # Replace Handle with HandleAsync
        $result = Replace-InFile -FilePath $file.FullName `
            -OldString "public async Task" `
            -NewString "public async Task" `
            -Description "dummy"
        # Note: We'll do a more targeted replacement
        
        $content = Get-Content -Path $file.FullName -Raw
        if ($content -match "public async Task.+ Handle\(") {
            $newContent = $content -replace "public async Task(.+?) Handle\(", 'public async Task$1 HandleAsync('
            if ($DryRun) {
                Log "[DRY] $($file.FullName) :: CommandHandler: Handle -> HandleAsync" "DRY"
                $phaseChanges += 1
            } else {
                Set-Content -Path $file.FullName -Value $newContent -NoNewline
                Log "OK: $($file.FullName) :: CommandHandler: Handle -> HandleAsync" "DONE"
                $phaseChanges += 1
            }
        }
    }
    
    # 5. Update *Query.cs files
    $queryFiles = Get-ChildItem -Path $phaseDir -Filter "*Query.cs" -Recurse -ErrorAction SilentlyContinue
    foreach ($file in $queryFiles) {
        $content = Get-Content -Path $file.FullName -Raw
        # Skip QueryHandler files
        if ($content -match "Handler|Handle\(" -or $content -match "IRequestHandler") {
            continue
        }
        
        $result = Replace-InFile -FilePath $file.FullName `
            -OldString ": IRequest<" `
            -NewString ": IQuery<" `
            -Description "Query: IRequest -> IQuery"
        $phaseChanges += $result
    }
    
    # 6. Update *QueryHandler.cs files
    $queryHandlerFiles = Get-ChildItem -Path $phaseDir -Filter "*QueryHandler.cs" -Recurse -ErrorAction SilentlyContinue
    foreach ($file in $queryHandlerFiles) {
        # Replace IRequestHandler with IQueryHandler
        $result = Replace-InFile -FilePath $file.FullName `
            -OldString "IRequestHandler<" `
            -NewString "IQueryHandler<" `
            -Description "QueryHandler: IRequestHandler -> IQueryHandler"
        $phaseChanges += $result
        
        # Replace Handle with HandleAsync
        $content = Get-Content -Path $file.FullName -Raw
        if ($content -match "public async Task.+ Handle\(") {
            $newContent = $content -replace "public async Task(.+?) Handle\(", 'public async Task$1 HandleAsync('
            if ($DryRun) {
                Log "[DRY] $($file.FullName) :: QueryHandler: Handle -> HandleAsync" "DRY"
                $phaseChanges += 1
            } else {
                Set-Content -Path $file.FullName -Value $newContent -NoNewline
                Log "OK: $($file.FullName) :: QueryHandler: Handle -> HandleAsync" "DONE"
                $phaseChanges += 1
            }
        }
    }
    
    Log "Phase $($PhaseNum): $($phaseChanges) changes" "STAT"
    return $phaseChanges
}

# Main execution
Write-Host "`n" -ForegroundColor Green
Write-Host "╔════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║ MediatR to LiteBus Migration Tool      ║" -ForegroundColor Green
Write-Host "║ Target Phases: $($Phases -join ', ')             ║" -ForegroundColor Green
Write-Host "║ Mode: $(if ($DryRun) { 'DRY-RUN' } else { 'LIVE   ' })            ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════╝" -ForegroundColor Green
Write-Host "`n" -ForegroundColor Green

try {
    foreach ($phase in $Phases) {
        $phaseResult = Migrate-Phase -PhaseNum $phase
        $totalChanges += $phaseResult
    }
    
    Write-Host "`n" -ForegroundColor Green
    Write-Host "╔════════════════════════════════════════╗" -ForegroundColor Green
    Write-Host "║ MIGRATION COMPLETE                     ║" -ForegroundColor Green
    Write-Host "║ Total Changes: $totalChanges" -ForegroundColor Green
    if ($DryRun) {
        Write-Host "║ Mode: DRY-RUN (preview only)           ║" -ForegroundColor Cyan
    } else {
        Write-Host "║ Mode: LIVE (changes applied)           ║" -ForegroundColor Yellow
    }
    Write-Host "║ Log: $logFile" -ForegroundColor White
    Write-Host "╚════════════════════════════════════════╝" -ForegroundColor Green
    Write-Host "`n" -ForegroundColor Green
}
catch {
    Write-Host "ERROR: $_" -ForegroundColor Red
    Log "ERROR: $_" "ERROR"
    exit 1
}
