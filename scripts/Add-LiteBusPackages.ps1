# Add LiteBus NuGet packages to phases 2-8
# Also removes MediatR to avoid conflicts

param(
    [int[]]$Phases = @(2,3,4,5,6,7,8),
    [switch]$DryRun
)

$baseDir = "d:\Dev\Incubator\.NET"

Write-Host "`n=== Adding LiteBus and Removing MediatR Packages ===" -ForegroundColor Cyan
if ($DryRun) {
    Write-Host "Mode: DRY-RUN`n" -ForegroundColor Yellow
} else {
    Write-Host "Mode: LIVE`n" -ForegroundColor Green
}

# LiteBus packages to add
$liteBusPackages = @(
    '<PackageReference Include="LiteBus.Commands" Version="0.8.0" />',
    '<PackageReference Include="LiteBus.Queries" Version="0.8.0" />'
)

foreach ($phaseNum in $Phases) {
    Write-Host "Phase $($phaseNum)..." -ForegroundColor Magenta
    $phaseDir = "$baseDir\Phase $phaseNum\src"
    
    if (-not (Test-Path $phaseDir)) {
        Write-Host "  Not found" -ForegroundColor Red
        continue
    }
    
    # Find all .csproj files
    $csprojFiles = Get-ChildItem -Path $phaseDir -Filter "*.csproj" -Recurse
    
    foreach ($csproj in $csprojFiles) {
        $filePath = $csproj.FullName
        $content = [System.IO.File]::ReadAllText($filePath)
        $originalContent = $content
        
        # Remove MediatR package reference
        $content = $content -replace '<PackageReference Include="MediatR" Version="[^"]*" />\s*\n', ''
        
        # Check if LiteBus packages already exist
        $hasLiteBusCommands = $content -contains "LiteBus.Commands"
        $hasLiteBusQueries = $content -contains "LiteBus.Queries"
        
        # Find the ItemGroup with PackageReferences and add LiteBus if not present
        if (-not $hasLiteBusCommands -or -not $hasLiteBusQueries) {
            # Find the first ItemGroup with PackageReference
            if ($content -match '<ItemGroup>\s*<PackageReference') {
                if (-not $hasLiteBusCommands) {
                    $content = $content -replace '(<ItemGroup>\s*<PackageReference[^>]*>)', "`$1`n`t  <PackageReference Include=`"LiteBus.Commands`" Version=`"0.8.0`" />"
                }
                if (-not $hasLiteBusQueries) {
                    $content = $content -replace '(<ItemGroup>\s*<PackageReference[^>]*>)', "`$1`n`t  <PackageReference Include=`"LiteBus.Queries`" Version=`"0.8.0`" />"
                }
            }
        }
        
        if ($content -ne $originalContent) {
            if ($DryRun) {
                Write-Host "  [DRY] $($csproj.Name)" -ForegroundColor Cyan
            } else {
                [System.IO.File]::WriteAllText($filePath, $content)
                Write-Host "  [OK] $($csproj.Name)" -ForegroundColor Green
            }
        }
    }
}

Write-Host "`nPackage updates complete`n" -ForegroundColor Green
