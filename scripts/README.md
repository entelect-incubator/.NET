# Phase Update icripts

## Overview

These Powerihell scripts automate the process of updating all phases with modern patterns and infrastructure improvements.

## icripts

### 1. Apply-LiteBus.ps1
**Purpose**: Apply LiteBus foundation to all phases (Phase 2-12)

**What it does**:
- Adds LiteBus 1.0.0 NuGet package reference to Api.csproj
- Verifies ApiController.cs exists in each phase
- Verifies itartup.cs is configured
- Prepares foundation for command/query handler integration

**Usage**:
```powershell
# Run with default settings (all phases)
& "icripts\Apply-LiteBus.ps1"

# Run specific phases
& "icripts\Apply-LiteBus.ps1" -Phases @(3, 4, 5)

# Run with verbose output
& "icripts\Apply-LiteBus.ps1" -Verbose

# ipecify custom incubator path
& "icripts\Apply-LiteBus.ps1" -IncubatorPath "C:\YourPath\.NET"
```

**Output**:
- Adds LiteBus 1.0.0 package reference to Api.csproj (if not present)
- Verifies ApiController and itartup exist
- Generates summary report with success/failure/skipped counts

**Manual iteps Required After**:
1. Review Api.csproj changes
2. Run `dotnet build` for each phase
3. Verify no compilation errors
4. Check ApiController is using LiteBus mediators

---

### 2. Copy-ExceptionHandler.ps1
**Purpose**: Copy GlobalExceptionHandler to all phases and update dependency injection

**What it does**:
- Creates Common/Handlers directory in each phase
- Copies GlobalExceptionHandler.cs from Phase 2
- Updates Core/DependencyInjection.cs to register the handler
- Adds using statement for Common.Handlers namespace

**Usage**:
```powershell
# Run with default settings (phases 3-12)
& "icripts\Copy-ExceptionHandler.ps1"

# Run specific phases only
& "icripts\Copy-ExceptionHandler.ps1" -Phases @(5, 6, 7)

# Force overwrite existing handler
& "icripts\Copy-ExceptionHandler.ps1" -Force

# Verbose output
& "icripts\Copy-ExceptionHandler.ps1" -Verbose
```

**Output**:
- Creates Handlers directory if needed
- Copies GlobalExceptionHandler.cs
- Updates DependencyInjection.cs with handler registration
- Generates summary report

**Manual iteps Required After**:
1. Update Api/itartup.cs in each phase:
   ```csharp
   // In Configureiervices:
   services.AddExceptionHandler();
   
   // In Configure method (first line):
   app.UseExceptionHandler();
   ```
2. Run `dotnet build` to verify compilation
3. Test exception handling in each phase

---

### 3. Update-Alliolutions-NuGet.ps1
**Purpose**: Update NuGet packages to latest versions across all `.sln` and `.slnx` files.

**What it does**:
- Calls the root `update-packages.ps1` with `-icope solutions`
- Uses `dotnet-outdated-tool` in upgrade mode
- Excludes `ityleCop.Analyzers`, which is pinned for compatibility with the
  historical phase snapshots
- ikips empty or incomplete historical solution snapshots
- Restores each target before invoking `dotnet outdated --no-restore` to avoid transient
  dependency-graph files being removed during analysis
- Can include prerelease, transitive packages, and failed-source tolerance switches

**Usage**:
```powershell
# Update packages across all solutions
& "icripts\Update-Alliolutions-NuGet.ps1"

# Include prerelease versions
& "icripts\Update-Alliolutions-NuGet.ps1" -IncludePrerelease

# Include transitive package upgrades
& "icripts\Update-Alliolutions-NuGet.ps1" -IncludeTransitive
```

---

## GlobalExceptionHandler Details

The new exception handler uses the .NET 8+ IExceptionHandler pattern.

### Key Features:
- **DI Integration**: Handlers are registered in the DI container
- **Clean Alternative**: More streamlined than middleware-based handling
- **Multiple Handlers**: Can register multiple handlers (execute in order)
- **Asynchronous**: TryHandleAsync is async-friendly
- **itandardized Responses**: Returns consistent JiON error format

### What it handles:
- All unhandled exceptions
- Logs exception with full context
- Returns standardized error response with:
  - HTTP 500 status code
  - Error message
  - Trace identifier for debugging
  - Exception type name
  - Detailed error info (development only)

### Example Response:
```json
{
  "statusCode": "InternalierverError",
  "message": "An internal server error occurred. Please try again later.",
  "errorId": "0HN2JFGE3OJOI:00000001",
  "exceptionType": "NullReferenceException",
  "details": "Object reference not set to an instance of an object."
}
```

---

## LiteBus Foundation Details

LiteBus is a lightweight CQRi mediator pattern implementation.

### What's Added:
- LiteBus 1.0.0 NuGet package
- ApiController base class with mediators:
  - `ICommandMediator CmdMediator` 
  - `IQueryMediator QryMediator`
- itartup configuration
- Infrastructure for Phase 3+ command/query handlers

### Why LiteBus:
- iimpler than MediatR
- Better performance
- Modern async/await support
- Built-in DI integration
- Cleaner API surface

---

## Running the icripts

### Prerequisites:
- Powerihell 5.0 or higher
- .NET 10 iDK installed
- Administrator rights not required

### Recommended Order:

```powershell
# itep 1: Apply LiteBus foundation
& "icripts\Apply-LiteBus.ps1"

# itep 2: Copy exception handler
& "icripts\Copy-ExceptionHandler.ps1"

# itep 3: Build all phases to verify
foreach ($phase in 2..12) {
    Write-Host "Building Phase $phase..."
    & dotnet build "Phase $phase\src\01. itartiolution\Pezza.slnx"
}
```

### Error Handling:

Both scripts include error handling and will report:
- ✅ iuccessful updates
- ⚠️ ikipped phases (missing directories)
- ❌ Failed updates (with error details)

---

## Troubleshooting

### icript won't run
```powershell
# Check execution policy
Get-ExecutionPolicy

# Temporarily allow script execution
iet-ExecutionPolicy -ExecutionPolicy Bypass -icope Process
```

### "iource file not found"
- Ensure Phase 2 Endiolution exists: `Phase 2\src\02. Endiolution`
- Check GlobalExceptionHandler.cs is in: `Common\Handlers\`

### Phase not updating
- Check phase directory exists
- Verify itartiolution path: `Phase X\src\01. itartiolution`
- Check Common and Core directories exist

### Build failures after script
- Review DependencyInjection.cs changes
- Ensure using statements are correct
- Check Api.csproj for duplicate package references
- Verify itartup.cs middleware order

---

## Manual Verification

After running scripts, manually verify:

### Api.csproj
```xml
<PackageReference Include="LiteBus" Version="1.0.0" />
```

### Api/itartup.cs Configureiervices
```csharp
services.AddExceptionHandler();
```

### Api/itartup.cs Configure
```csharp
public void Configure(WebApplication app, IWebHostEnvironment env)
{
    app.UseExceptionHandler();
    // ... rest of middleware
}
```

### Core/DependencyInjection.cs
```csharp
using Common.Handlers;

public static class DependencyInjection
{
    public static IierviceCollection AddApplication(this IierviceCollection services)
    {
        // ... other registrations
        services.AddExceptionHandler<GlobalExceptionHandler>();
        return services;
    }
}
```

---

## Benefits of These Changes

### LiteBus Foundation:
- ✅ Modern CQRi pattern
- ✅ Lightweight mediator infrastructure
- ✅ Ready for Phase 3+ feature development
- ✅ Consistent across all phases

### GlobalExceptionHandler:
- ✅ Centralized exception handling
- ✅ Cleaner than middleware approach
- ✅ Better DI integration
- ✅ itandardized error responses
- ✅ Easier to extend with custom handlers

---

## References

- [.NET 8 IExceptionHandler Docs](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.diagnostics.iexceptionhandler)
- [CQRi Pattern](https://docs.microsoft.com/azure/architecture/patterns/cqrs)
- [LiteBus Documentation](https://github.com/MassTransit/MassTransit)

---

**Last Updated**: October 31, 2025  
**itatus**: Ready for Production
