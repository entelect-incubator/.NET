#!/usr/bin/env pwsh
param(
    [int[]]$Phases = @(2, 3, 5, 6, 7, 8),
    [string]$TemplatePhase = 4,
    [switch]$CopySlnx = $true
)

$ErrorActionPreference = "Stop"

function LogMessage {
    param([string]$Message, [string]$Status = "INFO")
    $color = @{
        "OK" = "Green"
        "WARN" = "Yellow"
        "ERR" = "Red"
        "INFO" = "Cyan"
    }[$Status]
    
    Write-Host "[$Status] $Message" -ForegroundColor $color
}

# Get template slnx file from Phase 4
$templatePhaseDir = "d:\Dev\Incubator\.NET\Phase $TemplatePhase"
$templateStartSolDir = "$templatePhaseDir\src\01. StartSolution"
$templateSlnxFile = Get-ChildItem -Path $templateStartSolDir -File -Filter "*.slnx" | Select-Object -First 1

if (-not $templateSlnxFile) {
    LogMessage "Template SLNX file not found in Phase $TemplatePhase" "ERR"
    exit 1
}

LogMessage "Using template SLNX: $($templateSlnxFile.Name)" "INFO"
LogMessage ""

foreach ($phaseNum in $Phases) {
    $phaseDir = "d:\Dev\Incubator\.NET\Phase $phaseNum"
    $startSolDir = "$phaseDir\src\01. StartSolution"
    
    if (-not (Test-Path $startSolDir)) {
        LogMessage "Phase $phaseNum StartSolution not found" "WARN"
        continue
    }
    
    # Detect folder naming
    $coreDir = if (Test-Path "$startSolDir\Core") { "$startSolDir\Core" } elseif (Test-Path "$startSolDir\Pezza.Core") { "$startSolDir\Pezza.Core" } else { $null }
    $commonDir = if (Test-Path "$startSolDir\Common") { "$startSolDir\Common" } elseif (Test-Path "$startSolDir\Pezza.Common") { "$startSolDir\Pezza.Common" } else { $null }
    $apiDir = if (Test-Path "$startSolDir\Api") { "$startSolDir\Api" } elseif (Test-Path "$startSolDir\Pezza.Api") { "$startSolDir\Pezza.Api" } else { $null }
    $testDir = if (Test-Path "$startSolDir\Test") { "$startSolDir\Test" } elseif (Test-Path "$startSolDir\Pezza.Test") { "$startSolDir\Pezza.Test" } else { $null }
    
    if (-not $coreDir) {
        LogMessage "Phase ${phaseNum}: Could not identify folder structure" "WARN"
        continue
    }
    
    Write-Host ""
    LogMessage "Processing Phase $phaseNum..." "INFO"
    
    # Update Core csproj
    if ($coreDir) {
        $coreName = Split-Path -Leaf $coreDir
        $coreCsproj = "$coreDir\$coreName.csproj"
        if (Test-Path $coreCsproj) {
            $content = Get-Content $coreCsproj -Raw
            
            # Replace old packages if they exist
            if ($content -match 'LiteBus\.(Commands|Queries)') {
                $content = $content -replace 'PackageReference Include="LiteBus\.Commands" Version="0\.8\.0"\s*/>', ''
                $content = $content -replace 'PackageReference Include="LiteBus\.Queries" Version="0\.8\.0"\s*/>', ''
            }
            
            # Ensure LiteBus 1.0.0 is present
            if ($content -notmatch 'LiteBus" Version="1\.0\.0"') {
                if ($content -match 'PackageReference Include="FluentValidation') {
                    $content = $content -replace '(PackageReference Include="FluentValidation[^"]*"[^/]*/?>)', "`$1`n	  <PackageReference Include=`"LiteBus`" Version=`"1.0.0`" />"
                } else {
                    $content = $content -replace '(<PackageReference[^>]*/>)', "`$1`n	  <PackageReference Include=`"LiteBus`" Version=`"1.0.0`" />"
                }
            }
            
            Set-Content -Path $coreCsproj -Value $content -NoNewline
            LogMessage "  Updated $(Split-Path -Leaf $coreCsproj)" "OK"
        }
    }
    
    # Update Common csproj
    if ($commonDir) {
        $commonName = Split-Path -Leaf $commonDir
        $commonCsproj = "$commonDir\$commonName.csproj"
        if (Test-Path $commonCsproj) {
            $content = Get-Content $commonCsproj -Raw
            
            if ($content -match 'LiteBus\.(Commands|Queries)') {
                $content = $content -replace 'PackageReference Include="LiteBus\.Commands" Version="0\.8\.0"\s*/>', ''
                $content = $content -replace 'PackageReference Include="LiteBus\.Queries" Version="0\.8\.0"\s*/>', ''
            }
            
            if ($content -notmatch 'LiteBus" Version="1\.0\.0"') {
                if ($content -match 'PackageReference Include="FluentValidation') {
                    $content = $content -replace '(PackageReference Include="FluentValidation[^"]*"[^/]*/?>)', "`$1`n	  <PackageReference Include=`"LiteBus`" Version=`"1.0.0`" />"
                } else {
                    $content = $content -replace '(<PackageReference[^>]*/>)', "`$1`n	  <PackageReference Include=`"LiteBus`" Version=`"1.0.0`" />"
                }
            }
            
            Set-Content -Path $commonCsproj -Value $content -NoNewline
            LogMessage "  Updated $(Split-Path -Leaf $commonCsproj)" "OK"
        }
    }
    
    # Remove Behaviour directory
    if ($commonDir) {
        $behaviourDir = "$commonDir\Behaviour"
        if (Test-Path $behaviourDir) {
            Remove-Item -Path $behaviourDir -Recurse -Force
            LogMessage "  Deleted Behaviour directory" "OK"
        }
    }
    
    # Add QueryMediatorExtensions.cs
    if ($coreDir) {
        $extPath = "$coreDir\QueryMediatorExtensions.cs"
        if (-not (Test-Path $extPath)) {
            $ext = @'
namespace LiteBus.Queries.Abstractions
{
	using System.Threading;
	using System.Threading.Tasks;

	public static class QueryMediatorExtensions
	{
		public static async Task<TResponse> SendAsync<TResponse>(
			this IQueryMediator mediator,
			IQuery<TResponse> query,
			CancellationToken cancellationToken = default)
		{
			return await mediator.QueryAsync(query, cancellationToken);
		}
	}
}
'@
            Set-Content -Path $extPath -Value $ext
            LogMessage "  Created QueryMediatorExtensions.cs" "OK"
        }
    }
    
    # Update test files
    if ($testDir) {
        $testFiles = Get-ChildItem -Path $testDir -Include "*.cs" -Recurse -ErrorAction SilentlyContinue
        foreach ($file in $testFiles) {
            $content = Get-Content $file.FullName -Raw
            if ($content -match '\.Handle\(') {
                $content = $content -replace '\.Handle\(', '.HandleAsync('
                Set-Content -Path $file.FullName -Value $content -NoNewline
            }
        }
        LogMessage "  Updated test files" "OK"
    }
    
    # Update ApiController
    if ($apiDir) {
        $apiCtrlPath = "$apiDir\Controllers\ApiController.cs"
        if (Test-Path $apiCtrlPath) {
            $content = Get-Content $apiCtrlPath -Raw
            if ($content -match 'protected IMediator Mediator') {
                $apiCtrl = @'
namespace Api.Controllers;

using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
	private ICommandMediator cmdMediator;
	private IQueryMediator qryMediator;

	protected ICommandMediator CmdMediator => cmdMediator ??= HttpContext.RequestServices.GetRequiredService<ICommandMediator>();

	protected IQueryMediator QryMediator => qryMediator ??= HttpContext.RequestServices.GetRequiredService<IQueryMediator>();
}
'@
                Set-Content -Path $apiCtrlPath -Value $apiCtrl
                LogMessage "  Updated ApiController.cs" "OK"
            }
        }
        
        # Update other controllers
        $ctrls = Get-ChildItem -Path "$apiDir\Controllers" -Include "*Controller.cs" -Exclude "ApiController.cs" -ErrorAction SilentlyContinue
        foreach ($ctrl in $ctrls) {
            $content = Get-Content $ctrl.FullName -Raw
            $content = $content -replace 'await this\.Mediator\.Send\(new Get', 'await this.QryMediator.SendAsync(new Get'
            $content = $content -replace 'await this\.Mediator\.Send\(new (Create|Update|Delete)', 'await this.CmdMediator.SendAsync(new $1'
            Set-Content -Path $ctrl.FullName -Value $content -NoNewline
        }
        LogMessage "  Updated Controllers" "OK"
        
        # Update GlobalUsings
        $globalUsings = "$apiDir\GlobalUsings.cs"
        if (Test-Path $globalUsings) {
            $content = Get-Content $globalUsings -Raw
            if ($content -notmatch 'LiteBus\.Queries\.Abstractions') {
                $content = $content -replace '(global using DataAccess;)', "`$1`nglobal using LiteBus.Queries.Abstractions;`nglobal using LiteBus.Commands.Abstractions;"
                Set-Content -Path $globalUsings -Value $content -NoNewline
                LogMessage "  Updated GlobalUsings.cs" "OK"
            }
        }
        
        # Update Startup.cs
        $startup = "$apiDir\Startup.cs"
        if (Test-Path $startup) {
            $content = Get-Content $startup -Raw
            if ($content -match 'using Common\.Behaviour;') {
                $content = $content -replace 'using Common\.Behaviour;[\s\n]*', ''
                Set-Content -Path $startup -Value $content -NoNewline
                LogMessage "  Updated Startup.cs" "OK"
            }
        }
    }
    
    # Copy SLNX file
    if ($CopySlnx) {
        $existingSlnx = Get-ChildItem -Path $startSolDir -File -Filter "*.slnx" | Select-Object -First 1
        
        if ($existingSlnx) {
            # Replace existing slnx
            Copy-Item -Path $templateSlnxFile.FullName -Destination "$startSolDir\$($existingSlnx.Name)" -Force
            LogMessage "  Updated $($existingSlnx.Name)" "OK"
        } else {
            # Check if old .sln exists and get its name to create corresponding slnx
            $oldSln = Get-ChildItem -Path $startSolDir -File -Filter "*.sln" | Select-Object -First 1
            if ($oldSln) {
                $slnxName = $oldSln.BaseName + ".slnx"
                Copy-Item -Path $templateSlnxFile.FullName -Destination "$startSolDir\$slnxName" -Force
                LogMessage "  Created $slnxName (copied from Phase $TemplatePhase)" "OK"
            }
        }
    }
    
    LogMessage "Phase $phaseNum migration complete!" "OK"
}

Write-Host ""
LogMessage "All migrations complete!" "OK"
