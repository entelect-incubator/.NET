# Phase Update Scripts

## Overview

These PowerShell scripts automate the process of updating all phases with modern patterns and infrastructure improvements.

## Scripts

### 1. Apply-LiteBus.ps1
**Purpose**: Apply LiteBus foundation to all phases (Phase 2-12)

**What it does**:
- Adds LiteBus 1.0.0 NuGet package reference to Api.csproj
- Verifies ApiController.cs exists in each phase
- Verifies Startup.cs is configured
- Prepares foundation for command/query handler integration

**Usage**:
```powershell
# Run with default settings (all phases)
& "Scripts\Apply-LiteBus.ps1"

# Run specific phases
& "Scripts\Apply-LiteBus.ps1" -Phases @(3, 4, 5)

# Run with verbose output
& "Scripts\Apply-LiteBus.ps1" -Verbose

# Specify custom incubator path
& "Scripts\Apply-LiteBus.ps1" -IncubatorPath "C:\YourPath\.NET"
```

**Output**:
- Adds LiteBus 1.0.0 package reference to Api.csproj (if not present)
- Verifies ApiController and Startup exist
- Generates summary report with success/failure/skipped counts

**Manual Steps Required After**:
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
& "Scripts\Copy-ExceptionHandler.ps1"

# Run specific phases only
& "Scripts\Copy-ExceptionHandler.ps1" -Phases @(5, 6, 7)

# Force overwrite existing handler
& "Scripts\Copy-ExceptionHandler.ps1" -Force

# Verbose output
& "Scripts\Copy-ExceptionHandler.ps1" -Verbose
```

**Output**:
- Creates Handlers directory if needed
- Copies GlobalExceptionHandler.cs
- Updates DependencyInjection.cs with handler registration
- Generates summary report

**Manual Steps Required After**:
1. Update Api/Startup.cs in each phase:
   ```csharp
   // In ConfigureServices:
   services.AddExceptionHandler();
   
   // In Configure method (first line):
   app.UseExceptionHandler();
   ```
2. Run `dotnet build` to verify compilation
3. Test exception handling in each phase

---

## GlobalExceptionHandler Details

The new exception handler uses the .NET 8+ IExceptionHandler pattern.

### Key Features:
- **DI Integration**: Handlers are registered in the DI container
- **Clean Alternative**: More streamlined than middleware-based handling
- **Multiple Handlers**: Can register multiple handlers (execute in order)
- **Asynchronous**: TryHandleAsync is async-friendly
- **Standardized Responses**: Returns consistent JSON error format

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
  "statusCode": "InternalServerError",
  "message": "An internal server error occurred. Please try again later.",
  "errorId": "0HN2JFGE3OJOI:00000001",
  "exceptionType": "NullReferenceException",
  "details": "Object reference not set to an instance of an object."
}
```

---

## LiteBus Foundation Details

LiteBus is a lightweight CQRS mediator pattern implementation.

### What's Added:
- LiteBus 1.0.0 NuGet package
- ApiController base class with mediators:
  - `ICommandMediator CmdMediator` 
  - `IQueryMediator QryMediator`
- Startup configuration
- Infrastructure for Phase 3+ command/query handlers

### Why LiteBus:
- Simpler than MediatR
- Better performance
- Modern async/await support
- Built-in DI integration
- Cleaner API surface

---

## Running the Scripts

### Prerequisites:
- PowerShell 5.0 or higher
- .NET 10 SDK installed
- Administrator rights not required

### Recommended Order:

```powershell
# Step 1: Apply LiteBus foundation
& "Scripts\Apply-LiteBus.ps1"

# Step 2: Copy exception handler
& "Scripts\Copy-ExceptionHandler.ps1"

# Step 3: Build all phases to verify
foreach ($phase in 2..12) {
    Write-Host "Building Phase $phase..."
    & dotnet build "Phase $phase\src\01. StartSolution\Pezza.slnx"
}
```

### Error Handling:

Both scripts include error handling and will report:
- ✅ Successful updates
- ⚠️ Skipped phases (missing directories)
- ❌ Failed updates (with error details)

---

## Troubleshooting

### Script won't run
```powershell
# Check execution policy
Get-ExecutionPolicy

# Temporarily allow script execution
Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
```

### "Source file not found"
- Ensure Phase 2 EndSolution exists: `Phase 2\src\02. EndSolution`
- Check GlobalExceptionHandler.cs is in: `Common\Handlers\`

### Phase not updating
- Check phase directory exists
- Verify StartSolution path: `Phase X\src\01. StartSolution`
- Check Common and Core directories exist

### Build failures after script
- Review DependencyInjection.cs changes
- Ensure using statements are correct
- Check Api.csproj for duplicate package references
- Verify Startup.cs middleware order

---

## Manual Verification

After running scripts, manually verify:

### Api.csproj
```xml
<PackageReference Include="LiteBus" Version="1.0.0" />
```

### Api/Startup.cs ConfigureServices
```csharp
services.AddExceptionHandler();
```

### Api/Startup.cs Configure
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
    public static IServiceCollection AddApplication(this IServiceCollection services)
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
- ✅ Modern CQRS pattern
- ✅ Lightweight mediator infrastructure
- ✅ Ready for Phase 3+ feature development
- ✅ Consistent across all phases

### GlobalExceptionHandler:
- ✅ Centralized exception handling
- ✅ Cleaner than middleware approach
- ✅ Better DI integration
- ✅ Standardized error responses
- ✅ Easier to extend with custom handlers

---

## References

- [.NET 8 IExceptionHandler Docs](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.diagnostics.iexceptionhandler)
- [CQRS Pattern](https://docs.microsoft.com/azure/architecture/patterns/cqrs)
- [LiteBus Documentation](https://github.com/MassTransit/MassTransit)

---

**Last Updated**: October 31, 2025  
**Status**: Ready for Production
