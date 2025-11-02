<img align="left" width="116" height="116" src="pezza-logo.png" />

# &nbsp;**Pezza - Phase 5 — Standards & Error Handling** [![.NET - Phase 5 - Final Solution](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase5-finalsolution.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase5-finalsolution.yml)

<br/><br/>

## Quick facts

- .NET SDK required: 10 (net10)
- Estimated time: 4 - 8 hours
- Difficulty: ★★★★☆ (advanced intermediate)
- Audience: developers who completed Phase 4; ready to enforce coding standards and implement robust error handling
- **Building on**: Phase 4's validation patterns

## Goal

This phase adds **production-ready standards and error handling** to your dispatcher-based architecture:

- **Code Standards**: Configure analyzers (StyleCop, etc.) for consistent code style and quality rules
- **Centralized Error Handling**: Leverage .NET 8+ `IExceptionHandler` with `AddProblemDetails()`
- **Consistent Error Responses**: Shape error responses for APIs using RFC 7231 Problem Details format
- **Structured Logging**: Integrate logging into exception handlers and handlers

Learn to enforce quality through tooling, handle errors gracefully, and return predictable error shapes to API consumers.

## Prerequisites

- Completed Phase 4 (understand dispatcher, validation, and data handling)
- .NET 10 SDK installed and on PATH
- Familiarity with structured logging and exception handling concepts

## How to validate this phase locally

1. Build the start solution:

```powershell
dotnet build "Phase 5/src/01. StartSolution/Pezza.sln"
```

2. Run tests:

```powershell
dotnet test "Phase 5/src/01. StartSolution/Pezza.sln"
```

## Topics / learning outcomes

- Configure **StyleCop Analyzers** and other .NET analyzers for automatic style enforcement
- Understand **RFC 7231 Problem Details** format for standardized error responses
- Implement **centralized error handling** using `IExceptionHandler` middleware
- Learn how to structure **error responses** consistently across your API
- Use **structured logging** (Serilog, etc.) in error scenarios

## Key Pattern: GlobalExceptionHandler

This phase uses the **GlobalExceptionHandler** you've seen throughout:

```csharp
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) 
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, 
        Exception exception, 
        CancellationToken ct)
    {
        logger.LogError(exception, "An error occurred");

        context.Response.StatusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        await context.Response.WriteAsJsonAsync(
            new ErrorResponse { Message = exception.Message },
            cancellationToken: ct);

        return true;
    }
}
```

Registered in `Startup.cs`:
```csharp
services.AddExceptionHandler<GlobalExceptionHandler>();
services.AddProblemDetails();
```

## References

- StyleCop Analyzers: [https://github.com/DotNetAnalyzers/StyleCopAnalyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)
- RFC 7231 Problem Details: [https://www.rfc-editor.org/rfc/rfc7231](https://www.rfc-editor.org/rfc/rfc7231)
- .NET Exception Handling Middleware: [Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.diagnostics.iexceptionhandler)
- Structured Logging with Serilog: [https://serilog.net/](https://serilog.net/)

## Steps

- [ ] [Step 1 - Standards & Analyzers](Phase%205/src/02.%20Step%201)
- [ ] [Step 2 - Centralized Error Handling](Phase%205/src/03.%20Step%202)
