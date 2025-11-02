# Fix field naming convention - remove underscore prefix per style guide
# This script updates ICommandMediator and IQueryMediator field declarations

param(
    [int[]]$Phases = @(2,3,4,5,6,7,8),
    [switch]$DryRun
)

$baseDir = "d:\Dev\Incubator\.NET"
$totalChanges = 0

Write-Host "`n=== Fixing Field Naming Convention ===" -ForegroundColor Cyan
if ($DryRun) {
    Write-Host "Mode: DRY-RUN`n" -ForegroundColor Yellow
} else {
    Write-Host "Mode: LIVE`n" -ForegroundColor Green
}

foreach ($phaseNum in $Phases) {
    Write-Host "Phase $($phaseNum)..." -ForegroundColor Magenta
    $phaseDir = "$baseDir\Phase $phaseNum\src"
    
    if (-not (Test-Path $phaseDir)) {
        Write-Host "  Not found" -ForegroundColor Red
        continue
    }
    
    $phaseCount = 0
    
    # Find all ApiController.cs files
    Get-ChildItem -Path $phaseDir -Filter "ApiController.cs" -Recurse | ForEach-Object {
        $filePath = $_.FullName
        $content = [System.IO.File]::ReadAllText($filePath)
        $originalContent = $content
        
        # Replace private field declarations: private ICommandMediator _cmdMediator; -> private ICommandMediator cmdMediator;
        $content = $content -replace "private ICommandMediator _cmdMediator;", "private ICommandMediator cmdMediator;"
        $content = $content -replace "private IQueryMediator _qryMediator;", "private IQueryMediator qryMediator;"
        
        # Replace property initialization: _cmdMediator ??= -> cmdMediator ??=
        $content = $content -replace "_cmdMediator \?\?=", "cmdMediator ??="
        $content = $content -replace "_qryMediator \?\?=", "qryMediator ??="
        
        # Replace property access: get _cmdMediator, get _qryMediator
        $content = $content -replace "get _cmdMediator", "get cmdMediator"
        $content = $content -replace "get _qryMediator", "get qryMediator"
        
        if ($content -ne $originalContent) {
            if ($DryRun) {
                Write-Host "  [DRY] $($_.Name)" -ForegroundColor Cyan
            } else {
                [System.IO.File]::WriteAllText($filePath, $content)
                Write-Host "  [OK] $($_.Name)" -ForegroundColor Green
            }
            $phaseCount += 1
        }
    }
    
    Write-Host "  Changes: $phaseCount" -ForegroundColor Gray
    $totalChanges += $phaseCount
}

Write-Host "`nTotal changes: $totalChanges`n" -ForegroundColor Green
