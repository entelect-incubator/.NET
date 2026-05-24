#!/usr/bin/env pwsh
param([int[]]$Phases = @(2, 3, 5, 6, 7, 8))

$ErrorActionPreference = "Stop"

foreach ($phaseNum in $Phases) {
    $phaseDir = "d:\Dev\Incubator\.NET\Phase $phaseNum"
    $startSolDir = "$phaseDir\src\01. StartSolution"
    
    if (-not (Test-Path $startSolDir)) {
        Write-Host "Phase $phaseNum StartSolution not found" -ForegroundColor Yellow
        continue
    }
    
    # Detect folder naming
    $coreDir = if (Test-Path "$startSolDir\Core") { "$startSolDir\Core" } elseif (Test-Path "$startSolDir\Pezza.Core") { "$startSolDir\Pezza.Core" } else { $null }
    $commonDir = if (Test-Path "$startSolDir\Common") { "$startSolDir\Common" } elseif (Test-Path "$startSolDir\Pezza.Common") { "$startSolDir\Pezza.Common" } else { $null }
    $apiDir = if (Test-Path "$startSolDir\Api") { "$startSolDir\Api" } elseif (Test-Path "$startSolDir\Pezza.Api") { "$startSolDir\Pezza.Api" } else { $null }
    $testDir = if (Test-Path "$startSolDir\Test") { "$startSolDir\Test" } elseif (Test-Path "$startSolDir\Pezza.Test") { "$startSolDir\Pezza.Test" } else { $null }
    
    Write-Host "Processing Phase $phaseNum..." -ForegroundColor Cyan
    
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
                # Try to add after FluentValidation if it exists, otherwise after first PackageReference
                if ($content -match 'PackageReference Include="FluentValidation') {
                    $content = $content -replace '(PackageReference Include="FluentValidation[^"]*"[^/]*/?>)', "`$1`n	  <PackageReference Include=`"LiteBus`" Version=`"1.0.0`" />"
                } else {
                    $content = $content -replace '(<ItemGroup>[\s\n]*<PackageReference)', "`$1"
                    $content = $content -replace '(<PackageReference[^>]*/>)', "`$1`n	  <PackageReference Include=`"LiteBus`" Version=`"1.0.0`" />"
                }
            }
            
            Set-Content -Path $coreCsproj -Value $content -NoNewline
            Write-Host "  Updated Core.csproj" -ForegroundColor Green
        }
    }
    
    # Update Common csproj
    if ($commonDir) {
        $commonName = Split-Path -Leaf $commonDir
        $commonCsproj = "$commonDir\$commonName.csproj"
        if (Test-Path $commonCsproj) {
            $content = Get-Content $commonCsproj -Raw
            
            # Replace old packages if they exist
            if ($content -match 'LiteBus\.(Commands|Queries)') {
                $content = $content -replace 'PackageReference Include="LiteBus\.Commands" Version="0\.8\.0"\s*/>', ''
                $content = $content -replace 'PackageReference Include="LiteBus\.Queries" Version="0\.8\.0"\s*/>', ''
            }
            
            # Ensure LiteBus 1.0.0 is present
            if ($content -notmatch 'LiteBus" Version="1\.0\.0"') {
                # Try to add after FluentValidation if it exists, otherwise after first PackageReference
                if ($content -match 'PackageReference Include="FluentValidation') {
                    $content = $content -replace '(PackageReference Include="FluentValidation[^"]*"[^/]*/?>)', "`$1`n	  <PackageReference Include=`"LiteBus`" Version=`"1.0.0`" />"
                } else {
                    $content = $content -replace '(<PackageReference[^>]*/>)', "`$1`n	  <PackageReference Include=`"LiteBus`" Version=`"1.0.0`" />"
                }
            }
            
            Set-Content -Path $commonCsproj -Value $content -NoNewline
            Write-Host "  Updated Common.csproj" -ForegroundColor Green
        }
    }
    
    # Remove Behaviour directory
    if ($commonDir) {
        $behaviourDir = "$commonDir\Behaviour"
        if (Test-Path $behaviourDir) {
            Remove-Item -Path $behaviourDir -Recurse -Force
            Write-Host "  Deleted Behaviour directory" -ForegroundColor Green
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
            Write-Host "  Created QueryMediatorExtensions.cs" -ForegroundColor Green
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
        Write-Host "  Updated test files" -ForegroundColor Green
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
                Write-Host "  Updated ApiController.cs" -ForegroundColor Green
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
        Write-Host "  Updated Controllers" -ForegroundColor Green
        
        # Update GlobalUsings
        $globalUsings = "$apiDir\GlobalUsings.cs"
        if (Test-Path $globalUsings) {
            $content = Get-Content $globalUsings -Raw
            if ($content -notmatch 'LiteBus\.Queries\.Abstractions') {
                $content = $content -replace '(global using DataAccess;)', "`$1`nglobal using LiteBus.Queries.Abstractions;`nglobal using LiteBus.Commands.Abstractions;"
                Set-Content -Path $globalUsings -Value $content -NoNewline
                Write-Host "  Updated GlobalUsings.cs" -ForegroundColor Green
            }
        }
        
        # Update Startup.cs
        $startup = "$apiDir\Startup.cs"
        if (Test-Path $startup) {
            $content = Get-Content $startup -Raw
            if ($content -match 'using Common\.Behaviour;') {
                $content = $content -replace 'using Common\.Behaviour;[\s\n]*', ''
                Set-Content -Path $startup -Value $content -NoNewline
                Write-Host "  Updated Startup.cs" -ForegroundColor Green
            }
        }
    }
    
    Write-Host "Phase $phaseNum complete!" -ForegroundColor Green
}

Write-Host "All migrations complete!" -ForegroundColor Green
