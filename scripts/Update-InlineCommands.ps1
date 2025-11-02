# Update inline Command/Query/Handler classes in single files

param(
    [int[]]$Phases = @(2,3,4,5,6,7,8),
    [switch]$DryRun
)

$baseDir = "d:\Dev\Incubator\.NET"
$updated = 0

Write-Host "`n=== Updating inline Command/Query Classes ===" -ForegroundColor Cyan
if ($DryRun) {
    Write-Host "Mode: DRY-RUN`n" -ForegroundColor Yellow
} else {
    Write-Host "Mode: LIVE`n" -ForegroundColor Green
}

foreach ($phaseNum in $Phases) {
    Write-Host "Phase $($phaseNum)..." -ForegroundColor Magenta
    $phaseDir = "$baseDir\Phase $phaseNum\src"
    
    Get-ChildItem -Path $phaseDir -Filter "*Command.cs" -Recurse | ForEach-Object {
        $filePath = $_.FullName
        $content = [System.IO.File]::ReadAllText($filePath)
        $originalContent = $content
        
        # Skip if already migrated
        if ($content -match ": ICommand<|: IQuery<|ICommandHandler<|IQueryHandler<") {
            return
        }
        
        # Skip if doesn't use IRequest
        if ($content -notmatch ": IRequest<") {
            return
        }
        
        # Replace IRequest with ICommand in class declarations
        $content = $content -replace ": IRequest<", ": ICommand<"
        
        # Replace IRequestHandler with ICommandHandler
        $content = $content -replace ": IRequestHandler<", ": ICommandHandler<"
        
        # Replace Handle( with HandleAsync(
        $content = $content -replace "(\s+)public async Task(.+?) Handle\(", '${1}public async Task${2} HandleAsync('
        
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
    
    Get-ChildItem -Path $phaseDir -Filter "*Query.cs" -Recurse | ForEach-Object {
        $filePath = $_.FullName
        $content = [System.IO.File]::ReadAllText($filePath)
        $originalContent = $content
        
        # Skip if already migrated
        if ($content -match ": ICommand<|: IQuery<|ICommandHandler<|IQueryHandler<") {
            return
        }
        
        # Skip if doesn't use IRequest
        if ($content -notmatch ": IRequest<") {
            return
        }
        
        # Replace IRequest with IQuery in class declarations
        $content = $content -replace ": IRequest<", ": IQuery<"
        
        # Replace IRequestHandler with IQueryHandler
        $content = $content -replace ": IRequestHandler<", ": IQueryHandler<"
        
        # Replace Handle( with HandleAsync(
        $content = $content -replace "(\s+)public async Task(.+?) Handle\(", '${1}public async Task${2} HandleAsync('
        
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
