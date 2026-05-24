# Comprehensively update all DependencyInjection.cs files to use LiteBus

param(
    [int[]]$Phases = @(2,3,4,5,6,7,8),
    [switch]$DryRun
)

$baseDir = "d:\Dev\Incubator\.NET"
$updated = 0

Write-Host "`n=== Updating DependencyInjection.cs files ===" -ForegroundColor Cyan
if ($DryRun) {
    Write-Host "Mode: DRY-RUN`n" -ForegroundColor Yellow
} else {
    Write-Host "Mode: LIVE`n" -ForegroundColor Green
}

# Template for LiteBus registration
$liteBusTemplate = @'
		// Manually register LiteBus command handlers
		var assembly = typeof($FIRST_COMMAND$).Assembly;
		var commandHandlerTypes = assembly.GetTypes()
			.Where(t => t.Name.EndsWith("CommandHandler") && !t.IsInterface && !t.IsAbstract)
			.ToList();

		foreach (var handlerType in commandHandlerTypes)
		{
			services.AddScoped(handlerType);
		}

		// Manually register LiteBus query handlers
		var queryHandlerTypes = assembly.GetTypes()
			.Where(t => t.Name.EndsWith("QueryHandler") && !t.IsInterface && !t.IsAbstract)
			.ToList();

		foreach (var handlerType in queryHandlerTypes)
		{
			services.AddScoped(handlerType);
		}
'@

foreach ($phaseNum in $Phases) {
    Write-Host "Phase $($phaseNum)..." -ForegroundColor Magenta
    $phaseDir = "$baseDir\Phase $phaseNum\src"
    
    Get-ChildItem -Path $phaseDir -Filter "DependencyInjection.cs" -Recurse | ForEach-Object {
        $filePath = $_.FullName
        $content = [System.IO.File]::ReadAllText($filePath)
        $originalContent = $content
        
        # Check if file already has LiteBus
        if ($content -match "LiteBus|LiteBus.Commands|LiteBus.Queries") {
            return  # Skip already updated files
        }
        
        # Check if file has AddMediatR or IPipelineBehavior
        if ($content -notmatch "AddMediatR|IPipelineBehavior") {
            return  # Skip files that don't use MediatR
        }
        
        # Add LiteBus using statements
        if ($content -notmatch "using LiteBus") {
            # Find the last using statement and add LiteBus imports after
            $content = $content -replace "(using FluentValidation;)", "`$1`nusing LiteBus.Commands.Abstractions;`nusing LiteBus.Queries.Abstractions;"
        }
        
        # Remove AddMediatR line
        $content = $content -replace "(\s*)services\.AddMediatR\([^)]*\);\r?\n", ""
        
        # Remove IPipelineBehavior registrations
        $content = $content -replace "(\s*)services\.AddTransient\(typeof\(IPipelineBehavior<,>\)[^)]*\);\r?\n", ""
        
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
