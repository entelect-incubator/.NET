<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 5 - Step 2** [![.NET - Phase 5 - Final Solution](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase5-finalsolution.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase5-finalsolution.yml)

<br/><br/><br/>

## Centralized Error Handling with GlobalExceptionHandler

**Difficulty**: ★★★★☆ (Advanced Intermediate)  
**Estimated Time**: 2-3 hours
**Prerequisites**:

- Completed Phase 5 Step 1 (Analyzers & Standards)
- Understanding of custom MediatorLite dispatcher pattern from Phase 3
- Understanding of exception handling concepts

### Learning Outcomes

After completing this step, you will:

- Implement centralized error handling using `IExceptionHandler`
- Create RFC 7231 Problem Details responses
- Integrate structured logging (Serilog) into exception handlers
- Handle different exception types appropriately
- Understand modern .NET 8+ error handling patterns

---

## Implementation

### Step 1: Create GlobalExceptionHandler

Create a new file `Api/Handlers/GlobalExceptionHandler.cs`:

```cs
namespace Api.Handlers;

using System.Net;
using System.Text.Json;
using Common.Models;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) 
	: IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		logger.LogError(exception, "An unhandled exception occurred");

		var response = httpContext.Response;
		response.ContentType = "application/json";

		var errorResponse = exception switch
		{
			ValidationException validationException => new
			{
				httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest,
				errors = validationException.Errors.Select(e => new
				{
					field = e.PropertyName,
					message = e.ErrorMessage
				})
			},
			_ => new
			{
				httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError,
				error = "An internal server error occurred"
			}
		};

		response.StatusCode = errorResponse.StatusCode;
		await response.WriteAsJsonAsync(errorResponse, cancellationToken: cancellationToken);

		return true;
	}
}
```

### Step 2: Configure in Startup

Register the handler in your `Api/Program.cs` or startup configuration:

```cs
services.AddExceptionHandler<GlobalExceptionHandler>();
services.AddProblemDetails();

// In Configure()
app.UseExceptionHandler();
```

### Step 3: Integrate Serilog for Structured Logging

Install NuGet packages:

```cs
Serilog.AspNetCore
Serilog.Sinks.Console
Serilog.Sinks.File
```

Configure in `Program.cs`:

```cs
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Information()
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.WriteTo.File("logs/app-log.txt", rollingInterval: RollingInterval.Day)
	.CreateLogger();

builder.Host.UseSerilog();

// ... rest of configuration
```

The logger injected into GlobalExceptionHandler will automatically log all exceptions with structured data.

### Step 4: Handle Validation Exceptions

When handlers use `ValidationHelper.ValidateAsync()` and it throws `ValidationException`, the GlobalExceptionHandler will catch it and return a proper 400 Bad Request response.

Example handler using validation:

```cs
public sealed class CreateCustomerCommandHandler(
	IEnumerable<IValidator<CreateCustomerCommand>> validators,
	DatabaseContext database) 
	: ICommandHandler<CreateCustomerCommand, Result<CustomerModel>>
{
	public async Task<Result<CustomerModel>> Handle(
		CreateCustomerCommand command,
		CancellationToken cancellationToken)
	{
		// Validation is explicit in the handler
		await ValidationHelper.ValidateAsync(command, validators, cancellationToken);

		// Business logic here...
		var customer = new Customer { /* ... */ };
		database.Customers.Add(customer);
		await database.SaveChangesAsync(cancellationToken);

		return Result<CustomerModel>.Success(customer.Map());
	}
}
```

## Key Points

- **GlobalExceptionHandler** replaces old middleware-based exception handling
- **ValidationHelper** in handlers makes validation explicit and testable
- **Serilog** provides structured logging for debugging and monitoring
- **RFC 7231 Problem Details** ensures consistent error responses across your API
- **Modern .NET patterns** use `IExceptionHandler` instead of try-catch middleware

## Next Steps

[Move to Phase 6 - Caching & Compression](https://github.com/entelect-incubator/.NET/tree/master/Phase%206)
