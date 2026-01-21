#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Audits all phases 4-9 for old MediatR patterns and modern .NET patterns
    
.DESCRIPTION
    Scans for:
    - Old ValidationBehavior files (should be removed)
    - Old PerformanceBehaviour files using IPipelineBehavior (should use IExceptionHandler)
    - References to IPipelineBehavior in source code and READMEs
    - Missing GlobalExceptionHandler patterns
    - Outdated MediatR documentation in READMEs

.PARAMETER ReportPath
    Path where the audit report will be saved
    
.EXAMPLE
    .\audit-migration-patterns.ps1 -ReportPath ".\MIGRATION_AUDIT_$(Get-Date -Format 'yyyy-MM-dd').md"
#>

param(
    [Parameter(Mandatory=$false)]
    [string]$ReportPath = ".\MIGRATION_AUDIT_$(Get-Date -Format 'yyyy-MM-dd').md"
)

$ErrorActionPreference = "Stop"
$dotNetRoot = "d:\Dev\Incubator\.NET"
$phases = 4..9

# Initialize report
$report = @()
$report += "# Migration Audit: MediatR → Custom MediatorLite + Modern Exception Handling"
$report += ""
$report += "**Date**: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
$report += ""
$report += "## Summary"
$report += ""

# Track findings
$findings = @{
    ValidationBehaviorFiles = @()
    PerformanceBehaviourFiles = @()
    IPipelineBehaviorReferences = @()
    MissingGlobalExceptionHandler = @()
    OutdatedREADMEs = @()
}

Write-Host "🔍 Scanning phases 4-9 for old patterns..."
Write-Host ""

foreach ($phase in $phases) {
    $phasePath = Join-Path $dotNetRoot "Phase $phase"
    
    if (-not (Test-Path $phasePath)) {
        continue
    }
    
    Write-Host "Scanning Phase $phase..."
    
    # Find ValidationBehavior files
    $validationBehaviorFiles = Get-ChildItem -Path $phasePath -Recurse -Filter "ValidationBehavior.cs" -ErrorAction SilentlyContinue
    foreach ($file in $validationBehaviorFiles) {
        $findings["ValidationBehaviorFiles"] += @{
            Phase = $phase
            File = $file.FullName
            Status = "SHOULD BE REMOVED"
        }
        Write-Host "  ❌ Found ValidationBehavior.cs: $($file.FullName)"
    }
    
    # Find PerformanceBehaviour files using IPipelineBehavior
    $perfFiles = Get-ChildItem -Path $phasePath -Recurse -Filter "PerformanceBehaviour.cs" -ErrorAction SilentlyContinue
    foreach ($file in $perfFiles) {
        $content = Get-Content -Path $file.FullName -Raw
        if ($content -match "IPipelineBehavior") {
            $findings["PerformanceBehaviourFiles"] += @{
                Phase = $phase
                File = $file.FullName
                Issue = "Uses old IPipelineBehavior pattern"
            }
            Write-Host "  ⚠️  Found PerformanceBehaviour using IPipelineBehavior: $($file.FullName)"
        }
    }
    
    # Find all IPipelineBehavior references in source code
    $ipcReferences = Get-ChildItem -Path $phasePath -Recurse -Filter "*.cs" -ErrorAction SilentlyContinue |
        Select-String -Pattern "IPipelineBehavior|IRequestHandler.*MediatR|ICommandHandler.*MediatR" -ErrorAction SilentlyContinue
    
    foreach ($ref in $ipcReferences) {
        $findings["IPipelineBehaviorReferences"] += @{
            Phase = $phase
            File = $ref.Path
            Line = $ref.LineNumber
            Content = $ref.Line.Trim()
        }
        Write-Host "  ⚠️  IPipelineBehavior reference at $($ref.Path):$($ref.LineNumber)"
    }
    
    # Find API files and check for GlobalExceptionHandler
    $apiControllers = Get-ChildItem -Path (Join-Path $phasePath "src/*/Api") -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue
    $hasGlobalExceptionHandler = $false
    
    foreach ($file in $apiControllers) {
        $content = Get-Content -Path $file.FullName -Raw
        if ($content -match "GlobalExceptionHandler|IExceptionHandler") {
            $hasGlobalExceptionHandler = $true
            break
        }
    }
    
    if (-not $hasGlobalExceptionHandler) {
        $findings["MissingGlobalExceptionHandler"] += @{
            Phase = $phase
            Issue = "No GlobalExceptionHandler implementation found"
        }
        Write-Host "  ❌ Missing GlobalExceptionHandler in Phase $phase"
    }
    
    # Check READMEs for outdated patterns
    $readmeFiles = Get-ChildItem -Path $phasePath -Recurse -Filter "README.md" -ErrorAction SilentlyContinue
    foreach ($readme in $readmeFiles) {
        $content = Get-Content -Path $readme.FullName -Raw
        
        if ($content -match "IPipelineBehavior" -and $content -match "ValidationBehavior.*:.*IPipelineBehavior" -and -not ($content -match "Old.*Deprecated|❌.*Old")) {
            $findings["OutdatedREADMEs"] += @{
                Phase = $phase
                File = $readme.FullName
                Issue = "Documents old IPipelineBehavior patterns without marking as deprecated"
            }
            Write-Host "  ⚠️  Outdated README pattern: $($readme.FullName)"
        }
    }
}

Write-Host ""
Write-Host "📊 Generating audit report..."
Write-Host ""

# Generate findings section
$report += "- ValidationBehavior files found: **$($findings['ValidationBehaviorFiles'].Count)**"
$report += "- PerformanceBehaviour with IPipelineBehavior: **$($findings['PerformanceBehaviourFiles'].Count)**"
$report += "- Other IPipelineBehavior references: **$($findings['IPipelineBehaviorReferences'].Count)**"
$report += "- Phases missing GlobalExceptionHandler: **$($findings['MissingGlobalExceptionHandler'].Count)**"
$report += "- Outdated READMEs: **$($findings['OutdatedREADMEs'].Count)**"
$report += ""

# Detailed findings
$report += "---"
$report += ""

if ($findings['ValidationBehaviorFiles'].Count -gt 0) {
    $report += "## ValidationBehavior Files (Remove)"
    $report += ""
    foreach ($item in $findings['ValidationBehaviorFiles']) {
        $report += "- Phase $($item.Phase): $($item.File)"
        $report += "  Status: $($item.Status)"
        $report += "  Action: Delete this file entirely"
        $report += ""
    }
}

if ($findings['PerformanceBehaviourFiles'].Count -gt 0) {
    $report += "## ⚠️ PerformanceBehaviour Using IPipelineBehavior (Update)"
    $report += ""
    foreach ($item in $findings['PerformanceBehaviourFiles']) {
        $report += "- **Phase $($item.Phase)**: `$($item.File)`"
        $report += "  - Issue: $($item.Issue)"
        $report += "  - Action: Convert to modern IExceptionHandler or remove if not needed"
        $report += ""
    }
}

if ($findings['IPipelineBehaviorReferences'].Count -gt 0) {
    $report += "## ⚠️ IPipelineBehavior References in Source Code"
    $report += ""
    $groupedByPhase = $findings['IPipelineBehaviorReferences'] | Group-Object -Property Phase
    foreach ($group in $groupedByPhase) {
        $report += "### Phase $($group.Name)"
        $report += ""
        foreach ($ref in $group.Group) {
            $report += "- **File**: `$($ref.File)`"
            $report += "  - Line $($ref.Line): ``$($ref.Content)``"
            $report += ""
        }
    }
}

if ($findings['MissingGlobalExceptionHandler'].Count -gt 0) {
    $report += "## ❌ Missing GlobalExceptionHandler"
    $report += ""
    foreach ($item in $findings['MissingGlobalExceptionHandler']) {
        $report += "- **Phase $($item.Phase)**"
        $report += "  - Issue: $($item.Issue)"
        $report += "  - Action: Implement GlobalExceptionHandler: IExceptionHandler"
        $report += "  - Reference: Phase 5 Step 2 for implementation example"
        $report += ""
    }
}

if ($findings['OutdatedREADMEs'].Count -gt 0) {
    $report += "## ⚠️ Outdated README Documentation"
    $report += ""
    foreach ($item in $findings['OutdatedREADMEs']) {
        $report += "- **Phase $($item.Phase)**: `$($item.File)`"
        $report += "  - Issue: $($item.Issue)"
        $report += "  - Action: Update to show modern patterns or clearly mark as 'deprecated'"
        $report += ""
    }
}

# Add migration guide
$report += "---"
$report += ""
$report += "## Migration Guide"
$report += ""
$report += "### Pattern 1: Remove ValidationBehavior"
$report += ""
$report += "**Old (MediatR):**"
$report += "``````cs"
$report += "public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>"
$report += "{"
$report += "    // Handle validation in pipeline"
$report += "}"
$report += "```"
$report += ""
$report += "**New (Custom MediatorLite):**"
$report += "``````cs"
$report += "// Delete ValidationBehavior.cs entirely"
$report += "// Use ValidationHelper in individual handlers:"
$report += "await ValidationHelper.ValidateAsync(request, validators, cancellationToken);"
$report += "```"
$report += ""
$report += "### Pattern 2: Replace IPipelineBehavior with IExceptionHandler"
$report += ""
$report += "**Old (Middleware):**"
$report += "``````cs"
$report += "public class ExceptionHandlerMiddleware"
$report += "{"
$report += "    // Middleware-based exception handling"
$report += "}"
$report += "```"
$report += ""
$report += "**New (.NET 8+):**"
$report += "``````cs"
$report += "public class GlobalExceptionHandler : IExceptionHandler"
$report += "{"
$report += "    public async ValueTask<bool> TryHandleAsync(...)"
$report += "    {"
$report += "        // Modern exception handling"
$report += "    }"
$report += "}"
$report += "```"
$report += ""
$report += "### Pattern 3: Register in Startup"
$report += ""
$report += "**Remove old:**"
$report += "``````cs"
$report += "services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));"
$report += "services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));"
$report += "```"
$report += ""
$report += "**Add new:**"
$report += "``````cs"
$report += "services.AddExceptionHandler<GlobalExceptionHandler>();"
$report += "services.AddProblemDetails();"
$report += "app.UseExceptionHandler();"
$report += "```"
$report += ""

# Write report
$report | Out-File -FilePath $ReportPath -Encoding UTF8
Write-Host "✅ Audit report saved to: $ReportPath"
Write-Host ""

# Display summary
Write-Host "📈 Summary:"
Write-Host "  - ValidationBehavior files: $($findings['ValidationBehaviorFiles'].Count)"
Write-Host "  - PerformanceBehaviour issues: $($findings['PerformanceBehaviourFiles'].Count)"
Write-Host "  - IPipelineBehavior references: $($findings['IPipelineBehaviorReferences'].Count)"
Write-Host "  - Missing GlobalExceptionHandler: $($findings['MissingGlobalExceptionHandler'].Count)"
Write-Host "  - Outdated READMEs: $($findings['OutdatedREADMEs'].Count)"
