# Fix Nested Handlers - IRequestHandler Correction
# This fixes handlers that are nested in Command/Query files
# Since handlers are nested in the same file as commands/queries, they weren't found by the initial script

param(
    [Parameter(Mandatory=$true)]
    [string]$ProjectRoot
)

$ErrorActionPreference = "Stop"
Write-Host "===================================================================" -ForegroundColor Cyan
Write-Host "Fix Nested Handlers - IRequestHandler to ICommandHandler/IQueryHandler" -ForegroundColor Cyan
Write-Host "===================================================================" -ForegroundColor Cyan

Write-Host "`nProject Root: $ProjectRoot" -ForegroundColor Yellow

if (-not (Test-Path $ProjectRoot)) {
    Write-Host "ERROR: Project root path not found!" -ForegroundColor Red
    exit 1
}

function Fix-HandlerInterface {
    param([string]$FilePath, [string]$FileType)
    
    $content = Get-Content $FilePath -Raw
    $originalContent = $content
    
    # For command files: replace IRequestHandler with ICommandHandler
    # Pattern: IRequestHandler<CommandType, ResultType>
    if ($FileType -eq "Command") {
        $content = $content -replace 'IRequestHandler<', 'ICommandHandler<'
        # Also update using statements if needed
        $content = $content -replace 'using MediatR;', 'using LiteBus.Commands.Abstractions;'
    }
    
    # For query files: replace IRequestHandler with IQueryHandler
    if ($FileType -eq "Query") {
        $content = $content -replace 'IRequestHandler<', 'IQueryHandler<'
        # Also update using statements if needed
        $content = $content -replace 'using MediatR;', 'using LiteBus.Queries.Abstractions;'
    }
    
    if ($content -ne $originalContent) {
        Set-Content -Path $FilePath -Value $content -Encoding UTF8
        return $true
    }
    
    return $false
}

# Find all command files
$commandFiles = Get-ChildItem -Path $ProjectRoot -Filter "*Command.cs" -Recurse | 
                Where-Object { $_.FullName -like "*Core*" -and $_.FullName -notlike "*Test*" }

$queryFiles = Get-ChildItem -Path $ProjectRoot -Filter "*Query.cs" -Recurse | 
              Where-Object { $_.FullName -like "*Core*" -and $_.FullName -notlike "*Test*" }

$fixed = 0

Write-Host "`nFixing nested command handlers..." -ForegroundColor Green
foreach ($file in $commandFiles) {
    if (Fix-HandlerInterface -FilePath $file.FullName -FileType "Command") {
        Write-Host "  Fixed: $($file.Name)" -ForegroundColor Green
        $fixed++
    }
}

Write-Host "`nFixing nested query handlers..." -ForegroundColor Green
foreach ($file in $queryFiles) {
    if (Fix-HandlerInterface -FilePath $file.FullName -FileType "Query") {
        Write-Host "  Fixed: $($file.Name)" -ForegroundColor Green
        $fixed++
    }
}

Write-Host "`n================================================" -ForegroundColor Green
Write-Host "Total files fixed: $fixed" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Green
Write-Host "`nNext: Run 'dotnet build' to check for remaining errors`n" -ForegroundColor Yellow
