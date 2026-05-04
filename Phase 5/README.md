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

## Design Patterns Used in This Phase

- **[Clean Code Principles](https://github.com/entelect-incubator/Design-Patterns/tree/main/Clean-Code)** – StyleCop Analyzers for automated code quality and consistent style
- **[Testing Patterns](https://github.com/entelect-incubator/Design-Patterns/tree/main/Testing-Patterns)** – Comprehensive unit and integration test coverage with negative paths
- **[Result Pattern](https://github.com/entelect-incubator/Design-Patterns/tree/main/Result-Pattern)** – Centralized error handling with IExceptionHandler mapping Result errors to Problem Details
- **[CQRS Pattern](https://github.com/entelect-incubator/Design-Patterns/tree/main/CQRS)** – Error handling integrated into dispatcher pipeline
- **[Feature Architecture](https://github.com/entelect-incubator/Design-Patterns/tree/main/Feature-Architecture)** – Each feature has its own tests and error handling

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

---

Teaching Thread

- From: Phase 4 added validation and data handling.
- This phase: enforce code standards, analyzers, and centralized error handling.
- Next: Phase 6 will add background jobs and events.

Libraries (why they matter)

- StyleCop / Roslyn analyzers: enforce consistent code quality and automations for learning good habits.
- Serilog (or similar): structured logging for observability.

Clean Code & SOLID (teaching notes)

- Use analyzers to enforce naming and layout; keep solutions consistent to reduce cognitive load.
- Map `Result<T>` to ProblemDetails in a single place (exception handler) rather than ad-hoc responses.

MediatR policy

- Phase 5 expects the custom dispatcher to be used in production-like flows; do not introduce MediatR without documenting the reasons.

Notes

- Link to Clean Code guide and testing patterns: ../../Design-Patterns/08-Clean-Code-Principles/

## End Solution (what to verify)

- **Why**: Demonstrates the completed, production-ready slice with analyzers fixed and global exception handling wired through the custom dispatcher (no LiteBus/MediatR).
- **What**: `Dispatch.Dispatcher` registered in DI, controllers calling `Dispatcher.Send/Query`, `GlobalExceptionHandler` enabled with `AddExceptionHandler`, StyleCop clean build, and Result-based responses mapped by `ResponseHelper`.
- **How to run**: `dotnet build "Phase 5/src/04. EndSolution/Pezza.slnx"` then `dotnet test "Phase 5/src/04. EndSolution/Pezza.slnx"`.
- **Learning check**: You should now be comfortable enforcing code standards, routing via a custom dispatcher, and returning Problem Details through centralized error handling.

## Why Phase 5 — Production Quality Code

You've built a working API with CQRS, validation, filtering, and pagination. But **production code requires more than just working**:

- **Inconsistent Style**: Without standards, every developer formats code differently
- **Hard-to-Debug Errors**: Generic 500 errors don't tell clients what went wrong
- **Silent Failures**: Exceptions get swallowed without logging
- **Maintenance Burden**: Code reviews become arguments about spacing and naming

This phase teaches you to **enforce quality through tooling** and **handle errors gracefully** so your codebase is maintainable and your API is reliable.

## What We're Building

### **Step 1: Code Standards with Analyzers**

- Configure StyleCop Analyzers for automatic style enforcement
- Set up `.editorconfig` for team-wide formatting rules
- Add Roslyn analyzers for code quality (nullability, async patterns)
- Fix analyzer warnings to meet production standards

**Before (inconsistent):**

```csharp
// Developer A
public class pizza_handler {
    private DatabaseContext _db;
    public pizza_handler(DatabaseContext db) { _db = db; }
}

// Developer B  
public class PizzaHandler(DatabaseContext databaseContext)
{
}
```

**After (enforced):**

```csharp
// Everyone follows the same standard
public sealed class PizzaHandler(DatabaseContext databaseContext) : ICommandHandler<...>
{
}
```

### **Step 2: Centralized Error Handling**

- Use `IExceptionHandler` to catch exceptions globally
- Return RFC 7231 Problem Details for consistent error responses
- Log exceptions with structured logging (Serilog)
- Map Result<T> failures to appropriate HTTP status codes

**Before (inconsistent errors):**

```csharp
// Different error formats everywhere
return StatusCode(500, exception.Message);
return BadRequest(new { error = "Invalid" });
throw new Exception("Not found");
```

**After (standardized):**

```csharp
// All errors follow Problem Details RFC 7231
{
  "type": "https://api.pezza.com/errors/validation-failed",
  "title": "Validation Failed",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "traceId": "0HMVB8A8REC1P:00000001",
  "errors": {
    "Data.Name": ["Name is required"]
  }
}
```

## How It Works

### **Analyzer Flow**

1. Developer writes code
2. StyleCop/Roslyn analyzers run during build
3. Violations show as warnings/errors in IDE
4. Build fails if severity is Error (enforcing quality gates)
5. `.editorconfig` auto-formats code on save

### **Error Handling Flow**

1. Exception occurs in handler (validation, not found, database error)
2. `GlobalExceptionHandler` catches exception
3. Logger records exception details with trace ID
4. Map exception type to HTTP status code (400, 404, 500)
5. Return Problem Details JSON with error information
6. Client receives standardized error response

## What You Should Know By Now

After completing Phase 5, you should understand:

### **Code Quality & Standards**

- Why automated code standards reduce friction in code reviews
- How StyleCop Analyzers enforce naming conventions and structure
- What `.editorconfig` does and how it works across IDEs
- How to configure analyzer severity (suggestion, warning, error)
- When to suppress warnings vs. fix them

### **Error Handling Architecture**

- Why centralized error handling reduces code duplication
- How `IExceptionHandler` middleware intercepts exceptions
- What RFC 7231 Problem Details specification defines
- When to use different HTTP status codes (400 vs 404 vs 500)
- How structured logging helps debugging production issues

### **Production Readiness**

- Code standards make onboarding new developers faster
- Consistent error responses simplify client-side error handling
- Logging with trace IDs enables request tracking across services
- Problem Details includes helpful links to error documentation
- Analyzers catch common mistakes before they reach production

### **Integration with Dispatcher**

- Error handling sits **outside** the dispatcher (middleware layer)
- Exceptions bubble up from handlers to global handler
- Result<T> failures can be converted to exceptions or HTTP responses
- Logging happens at handler level (business logic) and global level (exceptions)

### **Testing Error Scenarios**

- Write tests for validation failures (400 errors)
- Write tests for not-found scenarios (404 errors)
- Write tests for database exceptions (500 errors)
- Verify Problem Details response format in tests
- Check that logs contain expected error information

## Knowledge Check

Test your understanding with these questions:

1. **Why use StyleCop Analyzers instead of manual code reviews for style?**
   <details><summary>Answer</summary>
   Analyzers enforce standards automatically during build, catching issues immediately. Manual reviews are slow, subjective, and inconsistent. Analyzers provide instant feedback and enforce team standards without human effort.
   </details>

2. **What's the difference between returning Result<T>.Failure() and throwing an exception?**
   <details><summary>Answer</summary>
   Result<T>.Failure() represents expected business failures (validation, not found) and keeps control flow explicit. Exceptions represent unexpected errors (database down, null reference). Failures are handled by the caller; exceptions are caught by middleware.
   </details>

3. **Why use RFC 7231 Problem Details instead of custom error JSON?**
   <details><summary>Answer</summary>
   Problem Details is an industry standard that clients already understand. It includes fields for type, title, status, detail, and extensions. Standardization means better tooling support and easier client integration.
   </details>

4. **When should you use HTTP 400 vs 404 vs 500?**
   <details><summary>Answer</summary>
   - 400: Client sent invalid data (validation failure, bad request)
   - 404: Resource not found (entity doesn't exist)
   - 500: Server error (database down, unhandled exception, programming bug)
   </details>

5. **How does GlobalExceptionHandler integrate with the custom dispatcher?**
   <details><summary>Answer</summary>
   The dispatcher routes commands/queries to handlers. If a handler throws an exception, it bubbles up to ASP.NET middleware. GlobalExceptionHandler (registered as IExceptionHandler) catches it, logs it, and returns a Problem Details response. The dispatcher is unaware of exception handling.
   </details>

6. **Why log exceptions even if you return error responses to the client?**
   <details><summary>Answer</summary>
   Client error responses don't include stack traces or internal details (security). Logs capture full exception info, trace IDs, and context needed for debugging production issues. Correlation IDs link client errors to server logs.
   </details>

Move to Phase 6
[Phase 6](https://github.com/entelect-incubator/.NET/tree/master/Phase%206)

## Next Step
Move to [Phase 6](https://github.com/entelect-incubator/.NET/tree/master/Phase%206)
