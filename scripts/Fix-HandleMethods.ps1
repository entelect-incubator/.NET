# Fix Handler Method Names - Handle() to HandleAsync()
# This script updates handlers to use the correct method names

param(
    [Parameter(Mandatory=$true)]
    [string]$ProjectRoot
)

$ErrorActionPreference = "Stop"
Write-Host "===================================================================" -ForegroundColor Cyan
Write-Host "Fix Handler Methods - Handle() to HandleAsync()" -ForegroundColor Cyan
Write-Host "===================================================================" -ForegroundColor Cyan

Write-Host "`nProject Root: $ProjectRoot" -ForegroundColor Yellow

if (-not (Test-Path $ProjectRoot)) {
    Write-Host "ERROR: Project root path not found!" -ForegroundColor Red
    exit 1
}

function Fix-HandleMethods {
    param([string]$FilePath)
    
    $content = Get-Content $FilePath -Raw
    $originalContent = $content
    
    # Replace Handle method with HandleAsync
    # This needs to be careful to handle different method signatures
    $content = $content -replace 'public async Task<(.+?)> Handle\(', 'public async Task<$1> HandleAsync('
    
    # Also handle non-async versions (though unlikely in this codebase)
    $content = $content -replace 'public Task<(.+?)> Handle\(', 'public Task<$1> HandleAsync('
    
    if ($content -ne $originalContent) {
        Set-Content -Path $FilePath -Value $content -Encoding UTF8
        return $true
    }
    
    return $false
}

# Find all handler files (they contain "Handler" in name)
$handlerFiles = Get-ChildItem -Path $ProjectRoot -Filter "*Handler.cs" -Recurse | 
                Where-Object { $_.FullName -like "*Core*" -and $_.FullName -notlike "*Test*" }

# Also find command/query files that might have nested handlers
$commandFiles = Get-ChildItem -Path $ProjectRoot -Filter "*Command.cs" -Recurse | 
                Where-Object { $_.FullName -like "*Core*" -and $_.FullName -notlike "*Test*" }

$queryFiles = Get-ChildItem -Path $ProjectRoot -Filter "*Query.cs" -Recurse | 
              Where-Object { $_.FullName -like "*Core*" -and $_.FullName -notlike "*Test*" }

$allFiles = @($handlerFiles) + @($commandFiles) + @($queryFiles)

$fixed = 0

Write-Host "`nFixing Handle() methods to HandleAsync()..." -ForegroundColor Green
foreach ($file in $allFiles) {
    if (Fix-HandleMethods -FilePath $file.FullName) {
        Write-Host "  Fixed: $($file.Name)" -ForegroundColor Green
        $fixed++
    }
}

Write-Host "`n================================================" -ForegroundColor Green
Write-Host "Total files fixed: $fixed" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Green
Write-Host "`nNext: Run 'dotnet build' to verify fixes`n" -ForegroundColor Yellow
