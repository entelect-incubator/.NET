# Develop with AI — .NET Guide

## Overview

This guide provides best practices and standards for developing .NET applications with AI assistance (GitHub Copilot, ChatGPT, Claude, etc.). Following these guidelines ensures clean, maintainable code that aligns with the Pezza project's architecture and style.

The goal is to work _with_ AI as an intelligent assistant—not as a replacement for architecture, design decisions, and code review.

---

## 1. Before You Code: Architecture & Design

### 1.1 Understand Your Architecture

Before asking AI to generate code, understand:

- **CQRS Pattern**: Commands (write) and Queries (read) are separated
- **LiteBus for Command/Query Bus**: `ICommandMediator` and `IQueryMediator` dispatch requests to handlers
- **Dependency Injection**: Services are injected via constructor (primary constructors in .NET 10)
- **Result Pattern**: Operations return `Result<T>` or `ListResult<T>` for consistent error handling
- **Entity Mapping**: Models map from entities via extension methods (`.Map()`)

### 1.2 Design Your Request/Response

Before AI generates handler code, define:

```csharp
// Query
public class GetPizzasQuery : IQuery<ListResult<PizzaModel>>
{
    public SearchPizzaModel Data { get; set; }
}

// Command
public class CreatePizzaCommand : ICommand<Result<PizzaModel>>
{
    public CreatePizzaModel? Data { get; set; }
}
```

**AI Prompt Example:**
```
I have a GetPizzasQuery : IQuery<ListResult<PizzaModel>> with SearchPizzaModel.
Generate a GetPizzasQueryHandler with:
- Primary constructor injecting DatabaseContext and IAppCache
- Cache lookup with 12-hour expiry using CacheKey
- Filter by Name and Description from SearchPizzaModel
- Return ListResult<PizzaModel>

Follow these patterns:
- Use task-based async/await
- Check for null requests
- Map entities using .Map() extension
- Use ToList() for cached data filtering
```

### 1.3 Identify Reusable Abstractions

Ask yourself (and AI):
- Is this a cross-cutting concern? (Use middleware or behaviors)
- Should this be a shared extension method?
- Is this logic duplicated elsewhere?

**Anti-pattern:** Don't ask AI to generate the same logic twice. Create a shared utility instead.

---

## 2. Coding Standards & Style

### 2.1 Naming Conventions

| Category           | Style                        | Example                                 | Why                                  |
| ------------------ | ---------------------------- | --------------------------------------- | ------------------------------------ |
| **Namespaces**     | PascalCase, domain-organized | `Core.Pizza.Commands`                   | Mirrors folder structure             |
| **Classes**        | PascalCase                   | `CreatePizzaCommand`                    | Standard C# convention               |
| **Properties**     | PascalCase, no underscores   | `public string Name { get; set; }`      | Clean API; no private backing fields |
| **Private fields** | camelCase (rare)             | `private ICommandMediator cmdMediator;` | Only if absolutely necessary         |
| **Methods**        | PascalCase                   | `Handle`, `FilterByName`                | Standard convention                  |
| **Local vars**     | camelCase                    | `var entity = new Pizza()`              | Readable, standard                   |
| **Constants**      | PascalCase                   | `public const string CacheKey = "..."`  | Matches property style               |

**Key Rule:** No underscore prefix for properties. Use `public required string Name { get; set; }` instead of `public string _name`.

**AI Prompt:**
```
Generate property definitions for a Pizza entity using:
- public string properties with no underscore prefix
- required keyword for mandatory fields
- nullable annotations (?) for optional fields
- No private backing fields
- Format: public required|nullable type PropertyName { get; set; }
```

### 2.2 Primary Constructors (C# 12+)

Always use primary constructors for dependency injection:

```csharp
// ✅ Correct: Primary constructor with ICommandHandler
public class CreatePizzaCommandHandler(
    DatabaseContext databaseContext, 
    IAppCache cache) : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> HandleAsync(CreatePizzaCommand request, CancellationToken cancellationToken)
    {
        // databaseContext and cache are available as parameters
    }
}

// ❌ Avoid: Traditional constructor
public class CreatePizzaCommandHandler : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IAppCache _cache;
    
    public CreatePizzaCommandHandler(DatabaseContext databaseContext, IAppCache cache)
    {
        _databaseContext = databaseContext;
        _cache = cache;
    }
}
```

**AI Prompt:**
```
Generate a handler using primary constructor syntax:
- Inject DatabaseContext and ICommandMediator/IQueryMediator as constructor parameters
- Do NOT use traditional constructor or backing fields
- Use clean parameter names (lowercase, no underscore)
- Implement ICommandHandler<...> or IQueryHandler<...>
```

### 2.3 Required vs Nullable

Use nullable annotations consistently:

```csharp
public sealed class PizzaModel
{
    public required int Id { get; set; }                    // Must be set
    public required string Name { get; set; }              // Must be set
    public string? Description { get; set; }               // Optional (null allowed)
    public decimal? Price { get; set; }                    // Optional
    public DateTime? DateCreated { get; set; }             // Optional
}
```

**AI Prompt:**
```
Generate a model class with:
- required properties: Id, Name, Email
- nullable properties: PhoneNumber, Address
- Use ? annotation for nullable
- Sealed class
- No constructor, only auto-properties
```

### 2.4 Async/Await Conventions

```csharp
// ✅ Correct: Async all the way
public async Task<Result<PizzaModel>> Handle(CreatePizzaCommand request, CancellationToken cancellationToken)
{
    var entity = new Pizza { /* ... */ };
    databaseContext.Pizzas.Add(entity);
    var result = await databaseContext.SaveChangesAsync(cancellationToken);
    return result > 0 ? Result<PizzaModel>.Success(entity.Map()) : Result<PizzaModel>.Failure("Error");
}

// ❌ Avoid: .Result or .Wait()
var result = databaseContext.SaveChangesAsync().Result;
```

**AI Prompt:**
```
Generate an async handler that:
- Uses async Task<Result<T>>
- Awaits all I/O operations (SaveChangesAsync, GetAsync, etc.)
- Passes CancellationToken to async methods
- Returns Result<T>.Success() or Result<T>.Failure()
```

---

## 3. DRY (Don't Repeat Yourself)

### 3.1 Extension Methods for Common Logic

Instead of duplicating filter logic, create extensions:

```csharp
// ✅ Correct: Shared extension method
namespace Common.Extensions;

public static class PizzaExtensions
{
    public static IEnumerable<PizzaModel> FilterByName(
        this IEnumerable<PizzaModel> source, 
        string? name) 
        => string.IsNullOrEmpty(name) 
            ? source 
            : source.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
}

// Usage in multiple queries
var filtered = cachedData.FilterByName(entity.Name).FilterByDescription(entity.Description);
```

**AI Prompt:**
```
Create extension methods for PizzaModel filtering:
- FilterByName: case-insensitive substring match, handles null
- FilterByDescription: similar, handles null
- FilterByPrice: range-based (min/max)
- Chain-able (return IEnumerable<PizzaModel>)
- Guard against null input
```

### 3.2 Mapper/Mapping Extensions

Never write manual mapping twice:

```csharp
// ✅ Correct: Centralized mapping
namespace Common.Mappings;

public static class PizzaMappings
{
    public static PizzaModel Map(this Pizza entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        Price = entity.Price,
        DateCreated = entity.DateCreated
    };
    
    public static IEnumerable<PizzaModel> Map(this IEnumerable<Pizza> entities)
        => entities.Select(x => x.Map());
}

// Usage
var model = entity.Map();
var models = entities.Map();
```

**AI Prompt:**
```
Generate mapping extensions for Pizza entity to PizzaModel:
- Single entity: Pizza -> PizzaModel
- Collection: IEnumerable<Pizza> -> IEnumerable<PizzaModel>
- Use LINQ Select for batch mapping
- Handle null gracefully
- Follow pattern: public static XModel Map(this X entity)
```

### 3.3 Shared Behaviors/Validators

Use LiteBus validators for input validation (integrated with IValidator<T>):

```csharp
// ✅ Correct: Fluent Validation for commands and queries
public class CreatePizzaCommandValidator : AbstractValidator<CreatePizzaCommand>
{
    public CreatePizzaCommandValidator()
    {
        RuleFor(x => x.Data.Name)
            .NotEmpty().WithMessage("Pizza name is required")
            .MaximumLength(100).WithMessage("Pizza name must be 100 characters or less");
        
        RuleFor(x => x.Data.Description)
            .MaximumLength(500).WithMessage("Description must be 500 characters or less");
    }
}

// ✅ LiteBus automatically integrates validators during command/query execution
// No need for pipeline behaviors - validation happens automatically in HandleAsync()
```

        return failures.Any() 
            ? throw new ValidationException(failures) 
            : await next();
    }
}
```

**AI Prompt:**
```
Create a MediatR pipeline behavior for:
- Request logging (log start, end, duration)
- Exception handling
- Inject ILogger<T>
- Implement IPipelineBehavior<TRequest, TResponse>
- Call next() to continue pipeline
```

---

## 4. AI Prompting Best Practices

### 4.1 Context is King

Provide context before the request:

```
CONTEXT:
- Project: Pezza (pizza ordering system)
- Architecture: CQRS with LiteBus
- Database: Entity Framework Core with in-memory DB
- Patterns: Primary constructors, Result<T> pattern, extension methods for mapping
- .NET Version: 10.0

TASK:
Generate a query handler for retrieving active pizzas with:
- Caching (12-hour expiry)
- Filtering by price range
- Sorting by popularity
```

### 4.2 Be Specific About Patterns

```
✅ GOOD:
"Generate an IQueryHandler<GetPizzasQuery, ListResult<PizzaModel>> that:
- Injects DatabaseContext and IAppCache via primary constructor
- Caches results with CacheKey and 12-hour expiry
- Filters using extension methods (FilterByPrice, FilterByName)
- Maps using .Map() extension"

❌ VAGUE:
"Generate a handler for getting pizzas with caching"
```

### 4.3 Ask for Specific Code Patterns

```csharp
// Ask AI to generate with specific patterns:
"Generate a handler using:
- Null checks at the start: if(request.Data is null) return Result.Failure(...)
- CancellationToken passed to all async methods
- Result<T>.Success() for success, Result<T>.Failure() for errors
- databaseContext.SaveChangesAsync(cancellationToken) not .Result
- Entity-to-model mapping via .Map() extension"
```

### 4.4 Request Code Review from AI

```
"Review this code for:
- Missing null checks
- Async/await violations
- DRY principle violations
- Primary constructor usage
- Result<T> pattern misuse"
```

### 4.5 Ask for Test Cases

```
"Generate xUnit test cases for CreatePizzaCommandHandler:
- Happy path: valid pizza creation
- Failure path: null Data
- Failure path: database save fails
- Verify cache is invalidated
- Use Moq for DatabaseContext and IAppCache"
```

---

## 5. Common Patterns in Pezza

### 5.1 Result Pattern

Always return `Result<T>` or `ListResult<T>`:

```csharp
// Single result
public class Result<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    
    public static Result<T> Success(T data) => new() { Success = true, Data = data };
    public static Result<T> Failure(string message) => new() { Success = false, Message = message };
}

// List result
public class ListResult<T> : Result<IEnumerable<T>>
{
    public int Total { get; set; }
    
    public static ListResult<T> Success(IEnumerable<T> data, int total) 
        => new() { Success = true, Data = data, Total = total };
}
```

**Usage:**
```csharp
return result > 0 
    ? Result<PizzaModel>.Success(entity.Map()) 
    : Result<PizzaModel>.Failure("Failed to create pizza");
```

### 5.2 Command Query Separation

```csharp
// QUERY (Read, no side effects)
public class GetPizzasQuery : IQuery<ListResult<PizzaModel>>
{
    public SearchPizzaModel Data { get; set; }
}

// COMMAND (Write, side effects)
public class CreatePizzaCommand : ICommand<Result<PizzaModel>>
{
    public CreatePizzaModel? Data { get; set; }
}

// HANDLER (One handler per command/query)
public class CreatePizzaCommandHandler(DatabaseContext databaseContext, IAppCache cache) 
    : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> HandleAsync(
        CreatePizzaCommand request, 
        CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

### 5.3 API Controller Pattern

```csharp
// Controllers inherit ApiController (which provides Mediator)
[ApiController]
[Route("[controller]")]
public class PizzaController() : ApiController
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePizzaModel model)
    {
        var result = await this.Mediator.Send(new CreatePizzaCommand { Data = model });
        return ResponseHelper.ResponseOutcome(result, this);
    }
}
```

---

## 6. Clean Architecture Checklist

Before asking AI to generate code or pushing code, verify:

### Code Quality
- [ ] No underscore-prefixed properties (`Name` not `_name`)
- [ ] Primary constructors used for DI
- [ ] Required/nullable annotations applied
- [ ] No `.Result` or `.Wait()` calls
- [ ] `CancellationToken` passed to async methods
- [ ] All public methods documented with `///` comments

### Architecture Compliance
- [ ] Queries return `ListResult<T>`
- [ ] Commands return `Result<T>`
- [ ] No business logic in controllers
- [ ] Handlers inject dependencies via constructor
- [ ] Entity-to-model mapping via extensions
- [ ] No duplicate code (check for extension opportunities)

### Performance
- [ ] Caching used for read-heavy queries
- [ ] `AsNoTracking()` for read-only EF queries
- [ ] Batch operations where applicable
- [ ] `CancellationToken` prevents hanging requests

### Testing
- [ ] Happy path test case exists
- [ ] Null/invalid input handling tested
- [ ] Mocks for external dependencies
- [ ] Result pattern assertions (Success, Message, Data)

---

## 7. AI Tools & Workflows

### 7.1 GitHub Copilot Workflow

1. **Type the signature** (class, method, interface)
2. **Let Copilot suggest** (review carefully)
3. **Accept/reject** individual suggestions
4. **Refactor** if needed to match standards

```csharp
// Start typing:
public class GetPizzasQueryHandler(DatabaseContext databaseContext)
{
    public async Task<ListResult<PizzaModel>> Handle(
        GetPizzasQuery request, 
        CancellationToken cancellationToken)
    {
        // Copilot suggests implementation here
        // Review it matches Result pattern, null checks, async patterns
    }
}
```

### 7.2 ChatGPT/Claude Workflow

1. **Paste context** (existing similar classes)
2. **Describe the request** with specificity
3. **Request the output format** (just the handler, just tests, etc.)
4. **Review for pattern adherence** before using

### 7.3 Interactive Code Review Loop

```
YOU: "Generate a query handler for filtering customers"
AI: [generates code]
YOU: "This doesn't use primary constructor. Refactor to use primary constructor syntax."
AI: [refactors]
YOU: "Good. Now add null check for request.Data"
AI: [adds check]
```

---

## 8. Anti-Patterns to Avoid

| Anti-Pattern                        | Why It's Bad                          | Correct Approach                       |
| ----------------------------------- | ------------------------------------- | -------------------------------------- |
| `var result = async.Result`         | Causes deadlocks, blocks threads      | Use `await`                            |
| `public string _name`               | Violates naming standard, verbose     | `public string Name { get; set; }`     |
| Direct data access in controller    | Business logic in presentation layer  | Use handlers/services                  |
| Duplicate mapping code              | Violates DRY                          | Create `.Map()` extension              |
| `var x = x ?? new()` without checks | Null reference exceptions             | Validate early with `Result.Failure()` |
| No `CancellationToken` passing      | Request can't be cancelled            | Pass to all async methods              |
| Synchronous wrapper over async      | Defeats async benefits, causes issues | Keep async/await all the way           |
---

## 9. Example: End-to-End Workflow

### Step 1: Design the Request/Response
```csharp
public class UpdatePizzaCommand : ICommand<Result<PizzaModel>>
{
    public int Id { get; set; }
    public UpdatePizzaModel? Data { get; set; }
}
```

### Step 2: Prompt AI
```
Implement UpdatePizzaCommandHandler:
- Primary constructor: DatabaseContext databaseContext, IAppCache cache
- Verify request.Data is not null
- Find pizza by request.Id
- Return Failure if pizza not found
```- Update properties: Name, Description, Price
- Cache invalidation: cache.Remove(Common.Data.CacheKey)
- Save and return Result<PizzaModel>.Success(entity.Map())
```

### Step 3: Review & Refactor
- Does it use primary constructor? ✓
- Null checks present? ✓
- CancellationToken handled? Add if missing
- Cache invalidation? ✓
- Uses `.Map()` for conversion? ✓

### Step 4: Test
```csharp
[Fact]
public async Task Handle_WithValidData_UpdatesPizza()
{
    // Arrange
    var context = new MockDatabaseContext();
    var cache = new MockAppCache();
    var handler = new UpdatePizzaCommandHandler(context, cache);
    var command = new UpdatePizzaCommand { Id = 1, Data = new() { Name = "Updated" } };

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.Success);
    Assert.NotNull(result.Data);
    cache.Verify(x => x.Remove(It.IsAny<string>()), Times.Once);
}
```

---

## 10. Resources & References

- **Clean Architecture**: Robert C. Martin's "Clean Architecture"
- **LiteBus**: Open-source CQRS command/query bus (replacement for MediatR)
- **Entity Framework Core**: https://docs.microsoft.com/ef/core/
- **C# 12 Primary Constructors**: https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-12
- **Async/Await Best Practices**: https://docs.microsoft.com/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming
- **Result Pattern**: https://github.com/ardalis/Result

---

## Summary

Developing with AI is productive when you:

1. **Understand your architecture** before asking AI to code
2. **Follow naming conventions** (no underscores, PascalCase properties)
3. **Use primary constructors** for all dependency injection
4. **Apply DRY principles** (extensions, mappers, shared behaviors)
5. **Leverage the Result pattern** for consistent error handling
6. **Separate commands and queries** for clear intent
7. **Provide context** to AI (existing patterns, architecture)
8. **Review AI-generated code** against this checklist
9. **Refactor together** with AI in iterative loops

This ensures that AI-assisted development maintains the project's quality, consistency, and long-term maintainability.
