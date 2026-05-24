# Add LiteBus service registration to DependencyInjection.cs files

param(
    [int[]]$Phases = @(2,3,4,5,6,7,8),
    [switch]$DryRun
)

$baseDir = "d:\Dev\Incubator\.NET"
$updated = 0

Write-Host "`n=== Adding LiteBus Service Registration ===" -ForegroundColor Cyan
if ($DryRun) {
    Write-Host "Mode: DRY-RUN`n" -ForegroundColor Yellow
} else {
    Write-Host "Mode: LIVE`n" -ForegroundColor Green
}

foreach ($phaseNum in $Phases) {
    Write-Host "Phase $($phaseNum)..." -ForegroundColor Magenta
    $phaseDir = "$baseDir\Phase $phaseNum\src"
    
    Get-ChildItem -Path $phaseDir -Filter "DependencyInjection.cs" -Recurse | ForEach-Object {
        $filePath = $_.FullName
        $content = [System.IO.File]::ReadAllText($filePath)
        $originalContent = $content
        
        # Check if already has LiteBus registration
        if ($content -match "services.AddLiteBusCommands|AddLiteBusQueries") {
            return
        }
        
        # Check if it has manual handler registration
        if ($content -notmatch "handlerType in commandHandlerTypes") {
            return
        }
        
        # Add LiteBus service registrations before handler registration
        # Find the line with "var assembly = " and insert before it
        $content = $content -replace "([\r\n]\s+)// Manually register LiteBus command handlers", "`$1// Add LiteBus services`$1services.AddLiteBusCommands();`$1services.AddLiteBusQueries();`$1`$1// Manually register LiteBus command handlers"
        
        if ($content -ne $originalContent) {
            if ($DryRun) {
                Write-Host "  [DRY] $([System.IO.Path]::GetFileName($filePath))" -ForegroundColor Cyan
            } else {
                [System.IO.File]::WriteAllText($filePath, $content)
                Write-Host "  [OK] $([System.IO.Path]::GetFileName($filePath))" -ForegroundColor Green
            }
            $updated += 1
        }
    }
}

Write-Host "`nUpdated $updated files`n" -ForegroundColor Green
