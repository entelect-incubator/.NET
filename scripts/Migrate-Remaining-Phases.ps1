#!/usr/bin/env pwsh
<#
.SYNOPSIS
Migrates Phases 2, 3, 5, 6, 7, 8 to LiteBus 1.0.0 using Phase 4 StartSolution as template
.DESCRIPTION
Applies the working Phase 4 StartSolution LiteBus pattern to remaining phases
#>

param(
    [string[]]$Phases = @(2, 3, 5, 6, 7, 8),
    [switch]$SkipBackup = $false,
    [switch]$WhatIf = $false
)

$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"

function Log {
    param([string]$Message, [ValidateSet("INFO", "SUCCESS", "WARNING", "ERROR")]$Level = "INFO")
    $prefix = switch ($Level) {
        "SUCCESS" { "[OK]" }
        "WARNING" { "[WARN]" }
        "ERROR" { "[ERR]" }
        default { "[INFO]" }
    }
    Write-Host "$prefix $Message" -ForegroundColor $(if ($Level -eq "SUCCESS") { "Green" } elseif ($Level -eq "ERROR") { "Red" } elseif ($Level -eq "WARNING") { "Yellow" } else { "Cyan" })
}

function Update-CoreCsproj {
    param([string]$PhaseDir, [int]$PhaseNum)
    
    Log "Updating Core.csproj files in Phase $PhaseNum..."
    
    Get-ChildItem -Path "$PhaseDir\src\*\Core\Core.csproj" -ErrorAction SilentlyContinue | ForEach-Object {
        $filePath = $_.FullName
        $content = Get-Content $filePath -Raw
        
        # Replace split LiteBus packages with single 1.0.0
        if ($content -match 'LiteBus\.(Commands|Queries)') {
            # Remove the old packages
            $content = $content -replace 'PackageReference Include="LiteBus\.Commands" Version="0\.8\.0"\s*/>', ''
            $content = $content -replace 'PackageReference Include="LiteBus\.Queries" Version="0\.8\.0"\s*/>', ''
            
            # Add LiteBus 1.0.0 if not present
            if ($content -notmatch 'LiteBus" Version="1\.0\.0"') {
                $content = $content -replace '(PackageReference Include="FluentValidation[^"]*"[^/]*/?>)', "`$1`n	  <PackageReference Include=`"LiteBus`" Version=`"1.0.0`" />"
            }
            
            if (-not $WhatIf) {
                Set-Content -Path $filePath -Value $content -NoNewline
                Log "Updated: $($_.Name)" "SUCCESS"
            }
        }
    }
}

function Update-CommonCsproj {
    param([string]$PhaseDir, [int]$PhaseNum)
    
    Log "Updating Common.csproj files in Phase $PhaseNum..."
    
    Get-ChildItem -Path "$PhaseDir\src\*\Common\Common.csproj" -ErrorAction SilentlyContinue | ForEach-Object {
        $filePath = $_.FullName
        $content = Get-Content $filePath -Raw
        
        # Replace split packages with single LiteBus 1.0.0
        if ($content -match 'LiteBus\.(Commands|Queries)') {
            $content = $content -replace 'PackageReference Include="LiteBus\.Commands" Version="0\.8\.0"\s*/>', ''
            $content = $content -replace 'PackageReference Include="LiteBus\.Queries" Version="0\.8\.0"\s*/>', ''
            
            if ($content -notmatch 'LiteBus" Version="1\.0\.0"') {
                $content = $content -replace '(PackageReference Include="FluentValidation[^"]*"[^/]*/?>)', "`$1`n	  <PackageReference Include=`"LiteBus`" Version=`"1.0.0`" />"
            }
            
            if (-not $WhatIf) {
                Set-Content -Path $filePath -Value $content -NoNewline
                Log "Updated: $($_.Name)" "SUCCESS"
            }
        }
    }
}

function Remove-BehaviourDirectories {
    param([string]$PhaseDir, [int]$PhaseNum)
    
    Log "Removing Behaviour directories from Phase $PhaseNum..."
    
    Get-ChildItem -Path "$PhaseDir\src\*\Common\Behaviour" -Directory -ErrorAction SilentlyContinue | ForEach-Object {
        if (-not $WhatIf) {
            Remove-Item -Path $_.FullName -Recurse -Force
            Log "Deleted: $($_.FullName)" "SUCCESS"
        }
    }
}

function Replace-HandleWithHandleAsync {
    param([string]$PhaseDir, [int]$PhaseNum)
    
    Log "Updating test files: .Handle() -> .HandleAsync() in Phase $PhaseNum..."
    
    $testFiles = Get-ChildItem -Path "$PhaseDir\src\*\Test\**\*.cs" -ErrorAction SilentlyContinue
    
    foreach ($file in $testFiles) {
        $content = Get-Content $file.FullName -Raw
        if ($content -match '\.Handle\(') {
            $content = $content -replace '\.Handle\(', '.HandleAsync('
            if (-not $WhatIf) {
                Set-Content -Path $file.FullName -Value $content -NoNewline
                Log "Updated: $($file.Name)" "SUCCESS"
            }
        }
    }
}

function Update-ApiControllers {
    param([string]$PhaseDir, [int]$PhaseNum)
    
    Log "Updating API Controllers in Phase $PhaseNum..."
    
    $controllers = Get-ChildItem -Path "$PhaseDir\src\*\Api\Controllers\*Controller.cs" -ErrorAction SilentlyContinue | Where-Object { $_.Name -ne "ApiController.cs" }
    
    foreach ($controller in $controllers) {
        $content = Get-Content $controller.FullName -Raw
        $changed = $false
        
        # Replace .Mediator.Send with appropriate method
        if ($content -match 'Query.*\}[\s\n]*\{[\s\n]*var result = await this\.Mediator\.Send') {
            # Query usage
            $content = $content -replace 'await this\.Mediator\.Send\((new Get\w+Query[^)]*)\)', 'await this.QryMediator.SendAsync($1, CancellationToken.None)'
            $changed = $true
        }
        
        if ($content -match 'Command.*\}[\s\n]*\{[\s\n]*var result = await this\.Mediator\.Send') {
            # Command usage
            $content = $content -replace 'await this\.Mediator\.Send\((new (Create|Update|Delete)\w+Command[^)]*)\)', 'await this.CmdMediator.SendAsync($1)'
            $changed = $true
        }
        
        # General replacements
        $content = $content -replace 'await this\.Mediator\.Send\(new Get', 'await this.QryMediator.SendAsync(new Get'
        $content = $content -replace 'await this\.Mediator\.Send\(new (Create|Update|Delete)', 'await this.CmdMediator.SendAsync(new $1'
        
        if ($changed -or $content -match 'Mediator\.Send') {
            if (-not $WhatIf) {
                Set-Content -Path $controller.FullName -Value $content -NoNewline
                Log "Updated: $($controller.Name)" "SUCCESS"
            }
        }
    }
}

function Update-ApiControllerBase {
    param([string]$PhaseDir, [int]$PhaseNum)
    
    Log "Updating ApiController base class in Phase $PhaseNum..."
    
    $apiControllerPath = "$PhaseDir\src\*\Api\Controllers\ApiController.cs"
    $files = Get-ChildItem -Path $apiControllerPath -ErrorAction SilentlyContinue
    
    foreach ($file in $files) {
        $content = Get-Content $file.FullName -Raw
        
        # Check if it has the old IMediator property
        if ($content -match 'protected IMediator Mediator') {
            $newContent = @"
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
"@
            if (-not $WhatIf) {
                Set-Content -Path $file.FullName -Value $newContent -NoNewline
                Log "Updated: $($file.Name)" "SUCCESS"
            }
        }
    }
}

function Add-QueryMediatorExtensions {
    param([string]$PhaseDir, [int]$PhaseNum)
    
    Log "Adding QueryMediatorExtensions.cs in Phase $PhaseNum..."
    
    $extensionContent = @"
namespace LiteBus.Queries.Abstractions
{
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Extension methods for IQueryMediator to support SendAsync pattern for consistency with MediatR
	/// </summary>
	public static class QueryMediatorExtensions
	{
		/// <summary>
		/// Send a query and get a result (wrapper around QueryAsync for API consistency)
		/// </summary>
		public static async Task<TResponse> SendAsync<TResponse>(
			this IQueryMediator mediator,
			IQuery<TResponse> query,
			CancellationToken cancellationToken = default)
		{
			// Wrapper that calls the LiteBus QueryAsync method
			return await mediator.QueryAsync(query, cancellationToken);
		}
	}
}
"@
    
    Get-ChildItem -Path "$PhaseDir\src\*\Core" -Directory -ErrorAction SilentlyContinue | foreach-object {
        $extensionPath = "$($_.FullName)\QueryMediatorExtensions.cs"
        if (-not (Test-Path $extensionPath)) {
            if (-not $WhatIf) {
                Set-Content -Path $extensionPath -Value $extensionContent -NoNewline
                Log "Created: QueryMediatorExtensions.cs" "SUCCESS"
            }
        }
    }
}

function Update-GlobalUsings {
    param([string]$PhaseDir, [int]$PhaseNum)
    
    Log "Updating GlobalUsings.cs files in Phase $PhaseNum..."
    
    $apiGlobalUsingsFiles = Get-ChildItem -Path "$PhaseDir\src\*\Api\GlobalUsings.cs" -ErrorAction SilentlyContinue
    
    foreach ($file in $apiGlobalUsingsFiles) {
        $content = Get-Content $file.FullName -Raw
        
        if ($content -notmatch 'LiteBus\.Queries\.Abstractions') {
            # Add LiteBus namespaces after DataAccess
            $content = $content -replace '(global using DataAccess;)', "`$1`nglobal using LiteBus.Queries.Abstractions;`nglobal using LiteBus.Commands.Abstractions;"
            
            if (-not $WhatIf) {
                Set-Content -Path $file.FullName -Value $content -NoNewline
                Log "Updated: GlobalUsings.cs" "SUCCESS"
            }
        }
    }
}

function Update-DependencyInjection {
    param([string]$PhaseDir, [int]$PhaseNum)
    
    Log "Updating DependencyInjection.cs files in Phase $PhaseNum..."
    
    $diFiles = Get-ChildItem -Path "$PhaseDir\src\*\Core\DependencyInjection.cs" -ErrorAction SilentlyContinue
    
    foreach ($file in $diFiles) {
        $content = Get-Content $file.FullName -Raw
        
        if ($content -match 'services\.AddMediatR|AddTransient.*IPipelineBehavior') {
            # Replace MediatR registration with LiteBus pattern
            $newDiPattern = @"
namespace Core;

using System.Reflection;
using Core.Customer.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		// Register LiteBus command and query handlers
		// Handlers will be registered through reflection scanning
		var assembly = typeof(CreateCustomerCommand).Assembly;

		// Register all command handlers
		var commandHandlerTypes = assembly.GetTypes()
			.Where(t => t.Name.EndsWith("CommandHandler") && !t.IsInterface && !t.IsAbstract)
			.ToList();

		foreach (var handlerType in commandHandlerTypes)
		{
			services.AddScoped(handlerType);
		}

		// Register all query handlers
		var queryHandlerTypes = assembly.GetTypes()
			.Where(t => t.Name.EndsWith("QueryHandler") && !t.IsInterface && !t.IsAbstract)
			.ToList();

		foreach (var handlerType in queryHandlerTypes)
		{
			services.AddScoped(handlerType);
		}

		// Register validators
		AssemblyScanner.FindValidatorsInAssembly(typeof(CreateCustomerCommand).Assembly)
		   .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		return services;
	}
}
"@
            
            if (-not $WhatIf) {
                # Try to replace just the AddMediatR part first
                if ($content -match 'services\.AddMediatR\(') {
                    $content = $content -replace '[\s\t]*services\.AddMediatR\([^;]*\);[\s\n]*', "`t// Register LiteBus command and query handlers`n`t// Handlers will be registered through reflection scanning`n`t"
                    $content = $content -replace '[\s\t]*services\.AddTransient\(typeof\(IPipelineBehavior.*?\);[\s\n]*', ''
                    
                    Set-Content -Path $file.FullName -Value $content -NoNewline
                    Log "Updated: $($file.Name)" "SUCCESS"
                }
            }
        }
    }
}

function Remove-StartupBehaviourImport {
    param([string]$PhaseDir, [int]$PhaseNum)
    
    Log "Removing Common.Behaviour imports from Startup.cs in Phase $PhaseNum..."
    
    $startupFiles = Get-ChildItem -Path "$PhaseDir\src\*\Api\Startup.cs" -ErrorAction SilentlyContinue
    
    foreach ($file in $startupFiles) {
        $content = Get-Content $file.FullName -Raw
        
        if ($content -match 'using Common\.Behaviour;') {
            $content = $content -replace 'using Common\.Behaviour;[\s\n]*', ''
            
            if (-not $WhatIf) {
                Set-Content -Path $file.FullName -Value $content -NoNewline
                Log "Updated: $($file.Name)" "SUCCESS"
            }
        }
    }
}

# Main execution
Log "================================================" "INFO"
Log "Migrating Phases to LiteBus 1.0.0 Pattern" "INFO"
Log "Phases: $($Phases -join ', ')" "INFO"
Log "================================================" "INFO"

foreach ($phase in $Phases) {
    $phaseDir = "d:\Dev\Incubator\.NET\Phase $phase"
    
    if (-not (Test-Path $phaseDir)) {
        Log "Phase $phase directory not found: $phaseDir" "WARNING"
        continue
    }
    
    Log "Processing Phase $phase..." "INFO"
    
    Update-CoreCsproj $phaseDir $phase
    Update-CommonCsproj $phaseDir $phase
    Remove-BehaviourDirectories $phaseDir $phase
    Add-QueryMediatorExtensions $phaseDir $phase
    Replace-HandleWithHandleAsync $phaseDir $phase
    Update-ApiControllerBase $phaseDir $phase
    Update-ApiControllers $phaseDir $phase
    Update-GlobalUsings $phaseDir $phase
    Update-DependencyInjection $phaseDir $phase
    Remove-StartupBehaviourImport $phaseDir $phase
    
    Log "Phase $phase migration complete!" "SUCCESS"
    Log ""
}

Log "All phases migrated successfully!" "SUCCESS"
