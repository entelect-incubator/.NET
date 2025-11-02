# Fix Controllers - Replace anonymous objects with proper Command/Query classes
param(
    [Parameter(Mandatory=$true)]
    [string]$ProjectRoot
)

$ErrorActionPreference = "Stop"
Write-Host "Fix Controllers - Replace anonymous objects with Command/Query classes" -ForegroundColor Cyan

# Function to fix a controller file
function Fix-ControllerFile {
    param([string]$FilePath)
    
    $content = Get-Content $FilePath -Raw
    $originalContent = $content
    
    # Fix CreatePizzaCommand pattern
    $content = $content -replace 'this\.CmdMediator\.SendAsync\(new\s*\{\s*Data = model\s*\}\)', 'this.CmdMediator.SendAsync(new CreatePizzaCommand { Data = model })'
    
    # Fix UpdatePizzaCommand pattern  
    $content = $content -replace 'this\.CmdMediator\.SendAsync\(new\s*\{\s*Data = model\s*\}\)', 'this.CmdMediator.SendAsync(new UpdatePizzaCommand { Data = model })'
    
    # Fix DeletePizzaCommand pattern
    $content = $content -replace 'this\.CmdMediator\.SendAsync\(new\s*\{\s*Id = id\s*\}\)', 'this.CmdMediator.SendAsync(new DeletePizzaCommand { Id = id })'
    
    # Similar for Customer commands
    $content = $content -replace 'this\.CmdMediator\.SendAsync\(new\s*\{\s*Data = request\s*\}\)', 'this.CmdMediator.SendAsync(new CreateCustomerCommand { Data = request })'
    
    # Similar for Order commands
    $content = $content -replace 'this\.CmdMediator\.SendAsync\(new\s*\{\s*Data = model\s*\}\)', 'this.CmdMediator.SendAsync(new OrderCommand { Data = model })'
    
    if ($content -ne $originalContent) {
        Set-Content -Path $FilePath -Value $content -Encoding UTF8
        return $true
    }
    
    return $false
}

# Find all controller files
$controllerFiles = Get-ChildItem -Path $ProjectRoot -Filter "*Controller.cs" -Recurse | 
                   Where-Object { $_.FullName -like "*Api*" -and $_.FullName -notlike "*Test*" }

$fixed = 0

foreach ($file in $controllerFiles) {
    if (Fix-ControllerFile -FilePath $file.FullName) {
        Write-Host "Fixed: $($file.Name)" -ForegroundColor Green
        $fixed++
    }
}

Write-Host "`nTotal controllers fixed: $fixed" -ForegroundColor Green
