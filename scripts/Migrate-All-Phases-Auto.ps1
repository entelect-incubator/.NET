# Comprehensive Multi-Phase Migration Script
# Migrates all phases from MediatR to LiteBus with automatic fixes

param(
    [switch]$SkipBuild = $false,
    [switch]$Verbose = $false
)

$BaseRoot = "d:\Dev\Incubator\.NET"
$Phases = @(8, 7, 6, 5, 4, 3, 2, 1)

Write-Host "`n" + ("=" * 80) -ForegroundColor Cyan
Write-Host "MULTI-PHASE LITEBUS MIGRATION" -ForegroundColor Cyan
Write-Host ("=" * 80) -ForegroundColor Cyan

$TotalSuccess = 0
$TotalFailed = 0

foreach ($Phase in $Phases) {
    $PhaseDir = Join-Path $BaseRoot "Phase $Phase\src\01. StartSolution"
    $PhaseName = "PHASE $Phase"
    
    if (-not (Test-Path $PhaseDir)) {
        Write-Host "`n[SKIP] $PhaseName - Path not found" -ForegroundColor Gray
        continue
    }
    
    Write-Host "`n" + ("-" * 80) -ForegroundColor Yellow
    Write-Host "[START] $PhaseName" -ForegroundColor Yellow
    
    try {
        Push-Location $PhaseDir
        
        # Step 1: Check current status
        $LiteBusCount = (Get-ChildItem -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue | Select-String "using LiteBus" | Measure-Object).Count
        if ($LiteBusCount -gt 0) {
            Write-Host "[OK] $PhaseName - Already migrated" -ForegroundColor Green
            $TotalSuccess++
            Pop-Location
            continue
        }
        
        # Step 2: Apply global replacements
        Write-Host "  [1/4] Applying global replacements..." -ForegroundColor Cyan
        
        # Get all C# files
        $CsFiles = Get-ChildItem -Filter "*.cs" -Recurse
        $FilesProcessed = 0
        
        foreach ($File in $CsFiles) {
            $Content = Get-Content $File.FullName -Raw
            $Modified = $false
            
            # MediatR -> LiteBus replacements
            if ($Content -match "using MediatR") {
                $Content = $Content -replace "using MediatR;", ""
                $Modified = $true
            }
            
            if ($Content -match "IRequest<") {
                # This is a query or command - we need to check the file name
                if ($File.Name -match "(Query|Get)" -or $File.Directory.Name -match "Queries") {
                    $Content = $Content -replace "IRequest<", "IQuery<"
                    $Content = $Content -replace "class (\w+) : IQuery<", 'using LiteBus.Queries.Abstractions;

class $1 : IQuery<'
                    $Modified = $true
                }
                else {
                    $Content = $Content -replace "IRequest<", "ICommand<"
                    $Content = $Content -replace "class (\w+) : ICommand<", 'using LiteBus.Commands.Abstractions;

class $1 : ICommand<'
                    $Modified = $true
                }
            }
            
            if ($Content -match "IRequestHandler<") {
                if ($File.Name -match "Handler") {
                    if ($File.Directory.Name -match "Queries") {
                        $Content = $Content -replace "IRequestHandler<([^,]+),\s*([^>]+)>", 'IQueryHandler<$1, $2>'
                    }
                    else {
                        $Content = $Content -replace "IRequestHandler<([^,]+),\s*([^>]+)>", 'ICommandHandler<$1, $2>'
                    }
                    $Content = $Content -replace "public (?:async )?Task<([^>]+)> Handle\(", 'public async Task<$1> HandleAsync('
                    $Modified = $true
                }
            }
            
            # Update LiteBus imports
            if ($Modified) {
                # Remove duplicate using statements
                $Content = $Content -replace "(using [^\r\n]+;\r?\n)+", { $_.Value | Select-Object -Unique }
                Set-Content $File.FullName -Value $Content -NoNewline
                $FilesProcessed++
            }
        }
        
        Write-Host "  [1/4] Processed $FilesProcessed files" -ForegroundColor Green
        
        # Step 3: Update GlobalUsings
        Write-Host "  [2/4] Updating GlobalUsings..." -ForegroundColor Cyan
        $GlobalUsingsFile = "GlobalUsings.cs"
        if (Test-Path $GlobalUsingsFile) {
            $Content = Get-Content $GlobalUsingsFile -Raw
            if ($Content -match "using MediatR") {
                $Content = $Content -replace "global using MediatR;", ""
                $Content = $Content + "`nglobal using LiteBus.Commands.Abstractions;`nglobal using LiteBus.Queries.Abstractions;"
                Set-Content $GlobalUsingsFile -Value $Content
                Write-Host "  [2/4] GlobalUsings updated" -ForegroundColor Green
            }
        }
        
        # Step 4: Build and verify
        if (-not $SkipBuild) {
            Write-Host "  [3/4] Building..." -ForegroundColor Cyan
            $BuildOutput = dotnet build 2>&1 | Out-String
            $CSharpErrors = [Regex]::Matches($BuildOutput, "error CS").Count
            
            if ($CSharpErrors -gt 0) {
                Write-Host "  [3/4] Build has $CSharpErrors C# errors" -ForegroundColor Red
                if ($Verbose) {
                    Write-Host $BuildOutput -ForegroundColor Red
                }
                $TotalFailed++
                Pop-Location
                continue
            }
            Write-Host "  [3/4] Build succeeded" -ForegroundColor Green
        }
        
        Write-Host "[OK] $PhaseName - Migration complete" -ForegroundColor Green
        $TotalSuccess++
    }
    catch {
        Write-Host "[ERROR] $PhaseName - $_" -ForegroundColor Red
        $TotalFailed++
    }
    finally {
        Pop-Location
    }
}

Write-Host "`n" + ("=" * 80) -ForegroundColor Cyan
Write-Host "MIGRATION SUMMARY" -ForegroundColor Cyan
Write-Host ("=" * 80) -ForegroundColor Cyan
Write-Host "Successful: $TotalSuccess phases" -ForegroundColor Green
Write-Host "Failed: $TotalFailed phases" -ForegroundColor $(if ($TotalFailed -eq 0) { "Green" } else { "Red" })
