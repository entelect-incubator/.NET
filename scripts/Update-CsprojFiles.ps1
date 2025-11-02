# Update all Common.csproj files in phases 2-8 to use LiteBus instead of MediatR

param(
    [int[]]$Phases = @(2,3,4,5,6,7,8),
    [switch]$DryRun
)

$baseDir = "d:\Dev\Incubator\.NET"
$updatedFiles = 0

Write-Host "`n=== Updating .csproj Files: MediatR -> LiteBus ===" -ForegroundColor Cyan
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
    
    # Find all Common.csproj and Api.csproj files
    $csprojFiles = Get-ChildItem -Path $phaseDir -Filter "*.csproj" -Recurse | Where-Object { $_.Name -match "Common.csproj|Api.csproj" }
    
    foreach ($csproj in $csprojFiles) {
        $filePath = $csproj.FullName
        $content = [System.IO.File]::ReadAllText($filePath)
        $originalContent = $content
        
        # Check if file has MediatR
        if ($content -notmatch 'PackageReference Include="MediatR"') {
            continue
        }
        
        # Replace MediatR with LiteBus
        if ($content -match '<PackageReference Include="MediatR" Version="([^"]*)"') {
            $mediatRVersion = $matches[1]
            $oldLine = '<PackageReference Include="MediatR" Version="' + $mediatRVersion + '" />'
            $newLines = '<PackageReference Include="LiteBus.Commands" Version="0.8.0" />' + "`n`t  " + '<PackageReference Include="LiteBus.Queries" Version="0.8.0" />'
            $content = $content.Replace($oldLine, $newLines)
        }
        
        if ($content -ne $originalContent) {
            if ($DryRun) {
                Write-Host "  [DRY] $($csproj.Name) - Replaced MediatR with LiteBus" -ForegroundColor Cyan
            } else {
                [System.IO.File]::WriteAllText($filePath, $content)
                Write-Host "  [OK] $($csproj.Name) - Updated" -ForegroundColor Green
            }
            $updatedFiles += 1
        }
    }
}

Write-Host "`nTotal .csproj files updated: $updatedFiles`n" -ForegroundColor Green
