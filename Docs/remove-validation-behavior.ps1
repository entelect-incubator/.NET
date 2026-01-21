# PowerShell Script to Remove ValidationBehavior from Phases 4-9
# This removes the MediatR pipeline behavior and switches to middleware-based validation

param(
    [switch]$DryRun = $false,
    [switch]$Verbose = $false
)

$ErrorActionPreference = "Stop"
$dotnetRoot = "d:\Dev\Incubator\.NET"
$deletedCount = 0
$modifiedCount = 0

Write-Host "╔════════════════════════════════════════════════════════════╗"
Write-Host "║  Remove ValidationBehavior Pipeline (Phases 4-9)           ║"
Write-Host "║  Switching to Middleware-Based Validation                  ║"
Write-Host "╚════════════════════════════════════════════════════════════╝"
Write-Host ""

if ($DryRun) {
    Write-Host "🔍 DRY RUN MODE - No changes will be made" -ForegroundColor Yellow
    Write-Host ""
}

# Step 1: Find and delete all ValidationBehavior.cs files
Write-Host "📋 Step 1: Finding ValidationBehavior.cs files..." -ForegroundColor Cyan

$validationBehaviorFiles = Get-ChildItem -Path $dotnetRoot `
    -Filter "ValidationBehavior.cs" -Recurse `
    | Where-Object { $_.FullName -match "(Phase [4-9])" }

Write-Host "Found $($validationBehaviorFiles.Count) ValidationBehavior.cs files"
Write-Host ""

foreach ($file in $validationBehaviorFiles) {
    $phase = [regex]::Match($file.FullName, "Phase (\d+)").Groups[1].Value
    
    if ($Verbose) {
        Write-Host "  ❌ DELETE: $($file.FullName)" -ForegroundColor Red
    }
    
    if (-not $DryRun) {
        Remove-Item -Path $file.FullName -Force
    }
    
    $deletedCount++
}

Write-Host "✅ Would delete: $deletedCount files" -ForegroundColor Green
Write-Host ""

# Step 2: Remove ValidationBehavior registration from DependencyInjection.cs files
Write-Host "📋 Step 2: Removing ValidationBehavior registrations..." -ForegroundColor Cyan

$diFiles = Get-ChildItem -Path $dotnetRoot `
    -Filter "DependencyInjection.cs" -Recurse `
    | Where-Object { $_.FullName -match "(Phase [4-9])" -and $_.FullName -match "\\Core\\" }

Write-Host "Found $($diFiles.Count) DependencyInjection.cs files to check"
Write-Host ""

foreach ($file in $diFiles) {
    $phase = [regex]::Match($file.FullName, "Phase (\d+)").Groups[1].Value
    $content = Get-Content -Path $file.FullName -Raw
    $originalContent = $content
    
    # Pattern to remove ValidationBehavior registration
    $pattern = @"
\s*services\.AddTransient\(typeof\(IPipelineBehavior<,>\),\s*typeof\(ValidationBehavior<,>\)\);?\s*
"@
    
    if ($content -match "ValidationBehavior") {
        if ($Verbose) {
            Write-Host "  🔧 MODIFY: $($file.FullName)" -ForegroundColor Yellow
        }
        
        $newContent = [regex]::Replace($content, $pattern, "`n", [System.Text.RegularExpressions.RegexOptions]::Multiline)
        
        if (-not $DryRun) {
            Set-Content -Path $file.FullName -Value $newContent -Force
        }
        
        $modifiedCount++
    }
}

Write-Host "✅ Would modify: $modifiedCount files" -ForegroundColor Green
Write-Host ""

# Step 3: Summary
Write-Host "╔════════════════════════════════════════════════════════════╗"
Write-Host "║                        SUMMARY                             ║"
Write-Host "╚════════════════════════════════════════════════════════════╝"
Write-Host ""
Write-Host "📊 Statistics:"
Write-Host "  • Files to delete: $deletedCount"
Write-Host "  • Files to modify: $modifiedCount"
Write-Host ""

if ($DryRun) {
    Write-Host "✅ Dry run complete. No changes made." -ForegroundColor Green
    Write-Host ""
    Write-Host "To apply changes, run:"
    Write-Host "  .\remove-validation-behavior.ps1 -DryRun:`$false"
} else {
    Write-Host "✅ ValidationBehavior removal complete!" -ForegroundColor Green
    Write-Host ""
    Write-Host "💡 Next steps:"
    Write-Host "  1. Verify ExceptionHandlerMiddleware is in Common/Behaviour/"
    Write-Host "  2. Ensure Startup.cs calls: app.UseMiddleware(typeof(ExceptionHandlerMiddleware))"
    Write-Host "  3. Test all phases compile correctly"
}

Write-Host ""
