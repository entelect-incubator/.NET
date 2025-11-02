<img align="left" width="116" height="116" src="pezza-logo.png" />

# &nbsp;**Pezza - Phase 4 — Handling Data** [![.NET - Phase 4 - Final Solution](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase4-finalsolution.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase4-finalsolution.yml)

<br/><br/>

## Quick facts

- .NET SDK required: 10 (net10)
- Estimated time: 6 - 12 hours
- Difficulty: ★★★★☆ (advanced intermediate)
- Audience: developers who completed Phase 3; ready to add validation and data handling patterns to their dispatcher-based architecture
- **Building on**: Phase 3's custom CQRS dispatcher pattern

## Goal

This phase extends the **custom CQRS dispatcher** from Phase 3 by adding **production-grade data handling**:

- **Validation**: Server-side validation using FluentValidation integrated with commands
- **Filtering & Searching**: Query handlers that support dynamic filtering and search predicates
- **Pagination**: Handling large result sets efficiently with skip/take patterns
- **Advanced EF Core**: Migrations, change tracking strategies, and related data loading (eager/lazy/explicit)

Learn to layer validation behaviors onto handlers, implement complex queries with filters, and handle real-world data scenarios.

## Prerequisites

- Completed Phase 3 (understand the dispatcher pattern and MediatorLite)
- .NET 10 SDK installed and on PATH
- Familiarity with FluentValidation or other validation libraries

## How to validate this phase locally

1. Build the start solution:

```powershell
dotnet build "Phase 4/src/01. StartSolution/Pezza.sln"
```

2. Run tests:

```powershell
dotnet test "Phase 4/src/01. StartSolution/Pezza.sln"
```

## Outcomes / learning objectives

- Understand how to add **validation to command handlers** using FluentValidation
- Implement **filtering and pagination** in query handlers
- Learn EF Core **migrations**, **change tracking**, and **related data loading strategies** (TPH/TPC/TPT inheritance patterns)
- See how the dispatcher pattern enables **cross-cutting concerns** like validation
- Build query handlers that support **complex filtering predicates** and return paginated results

## Modern Validation Pattern: IExceptionHandler + FluentValidation

This phase introduces the **modern .NET 8+ exception handling approach** for validation:

### **Old Approach (Deprecated)**

```csharp
// ❌ Old MediatR IPipelineBehavior approach
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var failures = await ValidateAsync(request);
        if (failures.Any())
            throw new ValidationException(failures);
        return await next();
    }
}
```

**Problems:**
- Requires external MediatR library
- Mixes validation logic with request handling
- Hard to test and extend
- Unclear error responses

### **New Approach (Phase 4+)**

The modern pattern separates **validation failures** from **unhandled exceptions**:

#### **Step 1: Define Validation Exception Handler**

```csharp
// In Api/Handlers/ValidationExceptionHandler.cs
public sealed class ValidationExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ValidationExceptionHandler> logger;

    public ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Only handle FluentValidation errors
        if (exception is not ValidationException validationException)
        {
            return false; // Let other handlers try
        }

        // Extract field-level errors
        var validationErrors = validationException.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray());

        // Return RFC 7231 Problem Details format
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = "application/json";

        var problemDetails = new
        {
            type = "https://api.pezza.com/docs/errors/validation-failed",
            title = "Validation Failed",
            status = 400,
            detail = "One or more validation errors occurred.",
            traceId = httpContext.TraceIdentifier,
            errors = validationErrors
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true; // Exception was handled
    }
}
```

#### **Step 2: Register in Startup.cs**

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // ... other configuration ...

    // Order matters: specific handlers first, generic handlers last
    services.AddExceptionHandler<ValidationExceptionHandler>();
    services.AddExceptionHandler<GlobalExceptionHandler>();
    services.AddProblemDetails();
}
```

#### **Step 3: Validate in Handlers**

```csharp
// In Core/Pizza/Commands/CreatePizzaCommandHandler.cs
public sealed class CreatePizzaCommandHandler(
    DatabaseContext db,
    IEnumerable<IValidator<CreatePizzaCommand>> validators)
    : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> Handle(
        CreatePizzaCommand command,
        CancellationToken ct)
    {
        // Validate before processing
        await ValidationHelper.ValidateAsync(command, validators, ct);

        // If we get here, validation passed
        var entity = new Pizza
        {
            Name = command.Data.Name,
            Description = command.Data.Description,
            Price = command.Data.Price
        };

        db.Pizzas.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<PizzaModel>.Success(entity.Map());
    }
}
```

#### **Step 4: Define Validators**

```csharp
// In Core/Pizza/Validators/CreatePizzaCommandValidator.cs
public class CreatePizzaCommandValidator : AbstractValidator<CreatePizzaCommand>
{
    public CreatePizzaCommandValidator()
    {
        RuleFor(x => x.Data.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 100).WithMessage("Name must be between 2 and 100 characters");

        RuleFor(x => x.Data.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}
```

### **Why This Pattern?**

| Aspect                    | Old (MediatR Pipeline)     | New (IExceptionHandler)         |
| ------------------------- | -------------------------- | ------------------------------- |
| **Library Dependency**    | Requires MediatR           | Built-in .NET 8+                |
| **Error Response Format** | Custom                     | RFC 7231 Problem Details        |
| **Handler Logic**         | Mixed with validation      | Explicit validation calls       |
| **Testing**               | Harder (mocks behaviors)   | Easier (test handlers directly) |
| **Performance**           | Pipeline overhead          | Direct exception handling       |
| **Extensibility**         | Limited by pipeline design | Add any exception handler       |

### **Example: Validation Error Response**

**Request:**
```json
POST /pizza
{
    "name": "",
    "price": -10
}
```

**Response (400 Bad Request):**
```json
{
    "type": "https://api.pezza.com/docs/errors/validation-failed",
    "title": "Validation Failed",
    "status": 400,
    "detail": "One or more validation errors occurred.",
    "traceId": "0HMVB8A8REC1P:00000001",
    "errors": {
        "Data.Name": [
            "Name is required"
        ],
        "Data.Price": [
            "Price must be greater than 0"
        ]
    }
}
```

## References

- FluentValidation: [https://fluentvalidation.net/](https://fluentvalidation.net/)
- EF Core migrations: [Migrations overview](https://learn.microsoft.com/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli)
- EF Core change tracking: [Change tracking overview](https://learn.microsoft.com/ef/core/change-tracking/)
- Loading related data: [Loading related data](https://learn.microsoft.com/ef/core/querying/related-data/)
- EF Core inheritance patterns: [Inheritance - Table-Per-Hierarchy (TPH)](https://learn.microsoft.com/ef/core/modeling/inheritance)
- .NET IExceptionHandler: [Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.diagnostics.iexceptionhandler)
- RFC 7231 Problem Details: [https://www.rfc-editor.org/rfc/rfc7231](https://www.rfc-editor.org/rfc/rfc7231)

## Setup

- [ ] Use the Start Solution from Phase 4: `Phase 4/src/01. StartSolution`
- [ ] Review Phase 3's dispatcher pattern (MediatorLite.cs) if needed
- [ ] Check Phase 3's DispatcherExtensions for simplified API patterns

## Steps

- [ ] [Step 1 - Validation with FluentValidation](Phase%204/src/02.%20Step1)
- [ ] [Step 2 - Filtering, Searching & Pagination](Phase%204/src/03.%20Step2)

