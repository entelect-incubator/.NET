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

## Design Patterns Used in This Phase

- **[CQRS Pattern](https://github.com/entelect-incubator/Design-Patterns/tree/main/CQRS)** – Enhanced dispatcher with validation behaviors and complex query filtering
- **[Repository Pattern](https://github.com/entelect-incubator/Design-Patterns/tree/main/Repository-Pattern)** – Advanced EF Core patterns: migrations, change tracking, eager/lazy loading
- **[Result Pattern](https://github.com/entelect-incubator/Design-Patterns/tree/main/Result-Pattern)** – Validation failures returned as explicit Result<T> errors
- **[Feature Architecture](https://github.com/entelect-incubator/Design-Patterns/tree/main/Feature-Architecture)** – Each feature contains commands, queries, validators, and DTOs
- **[Clean Code Principles](https://github.com/entelect-incubator/Design-Patterns/tree/main/Clean-Code)** – FluentValidation for readable, maintainable validation rules

## Prerequisites

- Completed Phase 3 (understand the custom dispatcher pattern)
- .NET 10 SDK installed and on PATH
- Familiarity with FluentValidation or other validation libraries

## How to validate this phase locally

1. Build the start solution:

```powershell
dotnet build "Phase 4/src/01. StartSolution/Pezza.slnx"
```

2. Run tests:

```powershell
dotnet test "Phase 4/src/01. StartSolution/Pezza.slnx"
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
// ❌ Old pipeline behavior approach (used by libraries like MediatR)
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

- Requires external libraries
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

| Aspect                    | Old (Pipeline Behavior)    | New (IExceptionHandler)         |
| ------------------------- | -------------------------- | ------------------------------- |
| **Library Dependency**    | Requires external library  | Built-in .NET 8+                |
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
- [ ] Review Phase 3's custom dispatcher pattern if needed
- [ ] Check Phase 3's DispatcherExtensions for simplified API patterns

## Why Phase 4 — Building Production-Ready APIs

You've built a **custom CQRS dispatcher** in Phase 3 that routes commands and queries to their handlers. But real-world APIs need more than just routing:

- **Validation**: How do you validate incoming data before it reaches your handlers?
- **Filtering**: How do you search for customers by name or pizzas by price range?
- **Pagination**: How do you return 1,000 pizzas without overwhelming the client?
- **Error Handling**: How do you return standardized error responses when validation fails?

This phase teaches you to **layer production-ready features** onto your existing dispatcher architecture without changing the core CQRS pattern.

## What We're Building

### **Step 1: FluentValidation Integration**

- Validate commands **before** they reach handlers
- Use `IExceptionHandler` (.NET 8+) to catch validation exceptions
- Return RFC 7231 Problem Details responses for validation errors
- Keep validation rules separate from business logic

**Before (no validation):**

```csharp
// Any data passes through
await dispatcher.Send<CreatePizzaCommand, Result<PizzaModel>>(new CreatePizzaCommand 
{ 
    Data = new() { Name = "", Price = -100 }  // ❌ Invalid but accepted
});
```

**After (validation layer):**

```csharp
// Invalid data throws ValidationException, caught by IExceptionHandler
await dispatcher.Send<CreatePizzaCommand, Result<PizzaModel>>(new CreatePizzaCommand 
{ 
    Data = new() { Name = "", Price = -100 }  // ✅ ValidationException → 400 Bad Request
});
```

### **Step 2: Filtering & Pagination**

- Implement **fluent filter extensions** for EF Core queries
- Add **pagination support** with skip/take patterns
- Use **System.Linq.Dynamic.Core** for dynamic sorting
- Handle large datasets efficiently

**Before (return all data):**

```csharp
// Returns ALL 10,000 pizzas
var allPizzas = await db.Pizzas.ToListAsync();
```

**After (filtered + paginated):**

```csharp
// Returns pizzas 21-40 matching "Pepperoni", sorted by price
var query = db.Pizzas
    .FilterByName("Pepperoni")
    .FilterByPriceRange(5, 20)
    .OrderBy("Price desc")
    .ApplyPaging(new PagingArgs { Offset = 20, Limit = 20 });

var result = await query.ToListAsync();
```

## How It Works

### **Validation Flow**

1. Client sends request to API controller
2. Controller dispatches command through `dispatcher.Send()`
3. Command handler validates request using FluentValidation
4. If validation fails, throw `ValidationException`
5. `IExceptionHandler` catches exception and returns 400 with Problem Details JSON
6. If validation passes, handler executes business logic

### **Filtering & Pagination Flow**

1. Client sends search request with filter criteria and page number
2. Query handler starts with base `IQueryable<T>`
3. Apply filter extensions (`.FilterByName()`, `.FilterByPrice()`)
4. Apply sorting with `OrderBy(string)` using dynamic LINQ
5. Apply pagination with `.Skip(offset).Take(limit)`
6. Execute query with `ToListAsync()` and return paginated result

## Steps

- [ ] [Step 1 - Validation with FluentValidation](Phase%204/src/02.%20Step1)
- [ ] [Step 2 - Filtering, Searching & Pagination](Phase%204/src/03.%20Step2)

## What You Should Know By Now

After completing Phase 4, you should understand:

### **Validation Architecture**

- How FluentValidation separates validation rules from business logic
- When to validate (in handlers, before business logic executes)
- How `IExceptionHandler` provides a centralized exception handling layer
- Why throwing exceptions for validation failures is acceptable in .NET 8+
- How to return RFC 7231 Problem Details for consistent error responses

### **Advanced Query Patterns**

- How to build **fluent filter extensions** that chain together
- Why `IQueryable<T>` enables deferred execution (filters compile to SQL)
- How to use `System.Linq.Dynamic.Core` for dynamic sorting from strings
- When to use `.AsNoTracking()` for read-only queries (performance optimization)
- How pagination reduces memory usage and improves API responsiveness

### **EF Core Best Practices**

- **Migrations**: How to create and apply schema changes with `dotnet ef migrations`
- **Change Tracking**: When EF Core tracks entities vs. when to disable tracking
- **Related Data Loading**:
  - Eager loading with `.Include()`
  - Lazy loading with virtual navigation properties
  - Explicit loading with `.Entry().Collection().Load()`
- **Inheritance Patterns**: TPH (Table-Per-Hierarchy), TPC (Table-Per-Concrete), TPT (Table-Per-Type)

### **Production Readiness**

- How to add **cross-cutting concerns** (validation, logging) without changing handlers
- Why separation of concerns matters: validators, handlers, and exception handlers each have one job
- How to scale APIs with pagination instead of returning unbounded result sets
- Why standardized error responses (Problem Details) help client applications

### **Integration with Dispatcher**

- Validation happens **inside** handlers (before business logic)
- Filters and pagination happen **in query handlers** (not in controllers)
- The dispatcher remains simple—it just routes requests to handlers
- Exception handlers catch validation failures and convert them to HTTP responses

### **What's Different from Phase 3?**

- **Phase 3**: Built the dispatcher routing mechanism (commands → handlers)
- **Phase 4**: Added production features (validation, filtering, pagination) **using** the dispatcher

You now have a **production-ready CQRS architecture** with validation, filtering, pagination, and standardized error handling.

[Go to Phase 5](https://github.com/entelect-incubator/.NET/tree/master/Phase%205)

## Next Step

Move to [Phase 5](https://github.com/entelect-incubator/.NET/tree/master/Phase%205)
