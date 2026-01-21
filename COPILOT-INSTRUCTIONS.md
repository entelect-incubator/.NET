# GitHub Copilot Instructions for .NET Pezza Development

This file provides system-level instructions for GitHub Copilot to maintain code quality and architectural consistency in the Pezza project.

## Core Architecture Principles

You are assisting developers in building the Pezza pizza ordering system using .NET 10 with the following architecture:

- **CQRS Pattern**: Commands (write operations) and Queries (read operations) are strictly separated
- **Custom Dispatcher**: All business logic flows through a custom-built Dispatcher (see [Design Patterns - CQRS](https://github.com/entelect-incubator/Design-Patterns/tree/main/03-CQRS-Pattern)) for type-safe command/query routing
- **Primary Constructors**: C# 12+ primary constructors for all dependency injection (no backing fields)
- **Result Pattern**: All operations return `Result<T>` or `Result` (non-generic) for consistent error handling (see [Result Pattern Reference](https://github.com/stianleroux/Results/blob/main/Results/Models/Result.cs) and [Design Patterns - Result Pattern](https://github.com/entelect-incubator/Design-Patterns/tree/main/02-Result-Pattern))
- **Entity Framework Core**: Database access through DbContext with async/await
- **Clean Architecture**: Controllers → Handlers → Services → Data Access (no business logic in presentation)

## Result Pattern Implementation

The project uses a clean Result pattern with only two types:

- **`Result`** - For operations without data (success/failure only)
- **`Result<T>`** - For operations that return typed data

### Result<T> Structure

```csharp
public class Result<T>
{
    public bool IsSuccess => ErrorResult == ErrorResults.None;
    public ErrorResults ErrorResult { get; set; } = ErrorResults.None;
    public List<string> Errors { get; set; } = [];
    public Dictionary<string, List<string>> ValidationErrors { get; set; } = [];
    public string? Message { get; set; }
    public T? Data { get; set; }
    public int Count { get; set; }  // For pagination scenarios
    
    // Factory methods
    public static Result<T> Success(T? data = default, int count = 0, string? message = null);
    public static Result<T> Failure(List<string>? errors = null, string? message = null);
    public static Result<T> Failure(string error, string? message = null);
    public static Result<T> Failure(Exception exception);
    public static Result<T> ValidationFailure(Dictionary<string, List<string>>? validationErrors = null, string? message = null);
    public static Result<T> NotFound(string? message = null);
    public static Result<T> Unauthorized(string? message = null);
    public static Result<T> Forbidden(string? message = null);
}
```

### Usage Examples

```csharp
// Success with data
return Result<PizzaModel>.Success(pizza);

// Success with pagination
return Result<IEnumerable<PizzaModel>>.Success(pizzas, count: total);

// Simple failure
return Result<PizzaModel>.Failure("Pizza not found");

// Validation failure
return Result<PizzaModel>.ValidationFailure(validationErrors);

// Not found
return Result<PizzaModel>.NotFound("Pizza with id 5 not found");

// Non-generic for operations without data
return Result.Success();
return Result.Failure("Operation failed");
```

**Important**: Do NOT create `ListResult<T>`, `PagedResult<T>`, or other custom result types. Use `Result<IEnumerable<T>>` with the `Count` property for lists/pagination.

## Code Standards You Must Follow

### 1. Naming Conventions
- **Properties**: PascalCase with NO underscore prefix
  - ✅ `public string Name { get; set; }`
  - ❌ `public string _name { get; set; }`
- **Classes/Methods**: PascalCase
  - ✅ `public class CreatePizzaCommand`
  - ❌ `public class create_pizza_command`
- **Local variables**: camelCase
  - ✅ `var entity = new Pizza()`
  - ❌ `var Entity = new Pizza()`
- **Namespaces**: domain-organized with hierarchy
  - ✅ `Core.Pizza.Commands`
  - ❌ `Commands`

### 2. Primary Constructors (ALWAYS)
```csharp
// ✅ CORRECT - Primary constructor, no backing fields
public class CreatePizzaCommandHandler(
    DatabaseContext databaseContext,
    IAppCache cache) : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> HandleAsync(
        CreatePizzaCommand request,
        CancellationToken cancellationToken)
    {
        // Use databaseContext and cache directly from constructor parameters
    }
}

// ❌ WRONG - Traditional constructor with backing fields
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

### 3. Required vs Nullable Properties
```csharp
// ✅ CORRECT - Explicit required/nullable annotations
public sealed class PizzaModel
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }  // Optional, can be null
    public decimal Price { get; set; }  // Non-nullable value type
}

// ❌ WRONG - Missing required keyword, unclear nullability
public class PizzaModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
```

### 4. Async/Await (Never use .Result or .Wait())
```csharp
// ✅ CORRECT - Proper async/await
public async Task<Result<PizzaModel>> Handle(
    CreatePizzaCommand request,
    CancellationToken cancellationToken)
{
    var entity = new Pizza { /* ... */ };
    databaseContext.Pizzas.Add(entity);
    
    var result = await databaseContext.SaveChangesAsync(cancellationToken);
    
    if (result > 0)
        return Result<PizzaModel>.Success(entity.Map());
    
    return Result<PizzaModel>.Failure("Failed to create pizza");
}

// ❌ WRONG - Blocking calls cause deadlocks
var result = databaseContext.SaveChangesAsync().Result;  // DEADLOCK!
var entity = databaseContext.Pizzas.FirstAsync().Result;  // DEADLOCK!
```

### 5. CancellationToken Must Be Passed
```csharp
// ✅ CORRECT - CancellationToken passed to all async methods
public async Task<Result<PizzaModel>> HandleAsync(
    CreatePizzaCommand request,
    CancellationToken cancellationToken)
{
    var entity = await databaseContext.Pizzas
        .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
    
    await databaseContext.SaveChangesAsync(cancellationToken);
}

// ❌ WRONG - Omitting CancellationToken
await databaseContext.SaveChangesAsync();  // Can't be cancelled
await databaseContext.Pizzas.FirstOrDefaultAsync(p => p.Id == id);
```

### 6. Null Checks at Entry
```csharp
// ✅ CORRECT - Validate input immediately
public async Task<Result<PizzaModel>> HandleAsync(
    CreatePizzaCommand request,
    CancellationToken cancellationToken)
{
    if (request.Data == null)
        return Result<PizzaModel>.Failure("Pizza data is required");
    
    if (string.IsNullOrEmpty(request.Data.Name))
        return Result<PizzaModel>.Failure("Name is required");
    
    // Continue with logic...
}

// ❌ WRONG - No validation, null reference exception possible
var entity = new Pizza { Name = request.Data.Name };  // NullReferenceException if request.Data is null
```

### 7. Result Pattern for All Returns
```csharp
// ✅ CORRECT - Consistent Result<T> pattern
public async Task<Result<PizzaModel>> Handle(...)
{
    // Success case
    return Result<PizzaModel>.Success(entity.Map());
    
    // Failure cases
    return Result<PizzaModel>.Failure("Entity not found");
    return Result<PizzaModel>.Failure("Unauthorized");
}

// For lists
public async Task<ListResult<PizzaModel>> Handle(...)
{
    var entities = await databaseContext.Pizzas.ToListAsync(cancellationToken);
    return ListResult<PizzaModel>.Success(entities.Map(), entities.Count);
}

// ❌ WRONG - Throwing exceptions instead of returning failures
throw new Exception("Pizza not found");
throw new UnauthorizedAccessException();
```

### 8. Entity Mapping via Extensions
```csharp
// ✅ CORRECT - Use .Map() extension for entity→model conversion
var model = entity.Map();
var models = entities.Map();

// ❌ WRONG - Manual mapping duplicated
var model = new PizzaModel 
{ 
    Id = entity.Id, 
    Name = entity.Name, 
    Description = entity.Description 
};
```

### 9. DRY: Extract Shared Logic
```csharp
// ✅ CORRECT - Create extension methods for shared operations
// In Common/Extensions/PizzaExtensions.cs
public static IEnumerable<PizzaModel> FilterByName(
    this IEnumerable<PizzaModel> source,
    string? name)
    => string.IsNullOrEmpty(name)
        ? source
        : source.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

// Usage in multiple queries
var filtered = cachedData
    .FilterByName(search.Name)
    .FilterByPrice(search.MinPrice, search.MaxPrice)
    .OrderBy(x => x.DateCreated);

// ❌ WRONG - Duplicating filter logic in every handler
if (!string.IsNullOrEmpty(search.Name))
    entities = entities.Where(x => x.Name.Contains(search.Name));
if (search.MinPrice.HasValue)
    entities = entities.Where(x => x.Price >= search.MinPrice);
```

### 10. Caching Pattern for Queries
```csharp
// ✅ CORRECT - Cache with 12-hour expiry
public async Task<ListResult<PizzaModel>> Handle(
    GetPizzasQuery request,
    CancellationToken cancellationToken)
{
    const string cacheKey = "pizzas_all";
    
    // Try cache first
    var cached = this.cache.GetOrSet(
        cacheKey,
        async (cancellationToken) =>
        {
            var entities = await databaseContext.Pizzas
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            return entities.Map().ToList();
        },
        new TimeSpan(12, 0, 0),  // 12-hour expiry
        cancellationToken);
    
    return ListResult<PizzaModel>.Success(cached, cached.Count);
}

// ❌ WRONG - No caching, repeated database hits
var entities = await databaseContext.Pizzas.ToListAsync(cancellationToken);
```

## Handler Structure Template

When generating handlers, follow this structure:

```csharp
namespace Core.{Domain}.{CommandOrQuery};

public class {Name}Handler(
    DatabaseContext databaseContext,
    IAppCache cache) : ICommandHandler<{CommandType}, {ResponseType}>  // or IQueryHandler
{
    public async Task<{ResponseType}> HandleAsync(
        {CommandType} request,
        CancellationToken cancellationToken)
    {
        // 1. Validate request
        if (request.Data == null)
            return Result<{ModelType}>.Failure("Data is required");

        // 2. Query database (with caching if read operation)
        var entity = await databaseContext.{Entities}
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Data.Id, cancellationToken);

        if (entity is null)
            return Result<{ModelType}>.Failure("Entity not found");

        // 3. Execute business logic
        entity.Property = request.Data.Property;

        // 4. Persist (for commands)
        databaseContext.{Entities}.Update(entity);
        var result = await databaseContext.SaveChangesAsync(cancellationToken);

        // 5. Invalidate cache if changed (for commands)
        if (result > 0)
            cache.Remove(Common.Data.CacheKey);

        // 6. Return result
        return result > 0
            ? Result<{ModelType}>.Success(entity.Map())
            : Result<{ModelType}>.Failure("Operation failed");
    }
}
```

## Controller Structure Template

When generating controllers, follow this pattern:

```csharp
namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class {EntityName}Controller() : ApiController
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get([FromRoute] int id)
    {
        var result = await this.Mediator.Send(new Get{EntityName}Query { Data = new() { Id = id } });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromBody] Create{EntityName}Model model)
    {
        var result = await this.Mediator.Send(new Create{EntityName}Command { Data = model });
        return ResponseHelper.ResponseOutcome(result, this, StatusCodes.Status201Created);
    }
}
```

## API Response Convention

All API responses follow this pattern:

```csharp
// Success
{
    "success": true,
    "data": { /* entity */ },
    "message": null
}

// Failure
{
    "success": false,
    "data": null,
    "message": "Error description"
}

// List success
{
    "success": true,
    "data": [ /* entities */ ],
    "total": 42,
    "message": null
}
```

## Test Structure Template

When generating tests, use this pattern:

```csharp
namespace Core.{Domain}.{CommandOrQuery}.Tests;

public class {HandlerName}Tests
{
    [Fact]
    public async Task Handle_With{Scenario}_Returns{Expected}()
    {
        // Arrange
        var mockDb = new Mock<DatabaseContext>();
        var mockCache = new Mock<IAppCache>();
        var handler = new {HandlerName}(mockDb.Object, mockCache.Object);
        
        var request = new {RequestType} 
        { 
            Data = new() { /* test data */ } 
        };

        mockDb
            .Setup(x => x.{Entities}.FindAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int id, CancellationToken ct) => new {EntityType} { Id = id });

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        mockCache.Verify(x => x.Remove(It.IsAny<string>()), Times.Once);
    }
}
```

## Common Commands to Follow

When a developer gives you a task, respond with these principles:

1. **Ask for context first**: "What's the domain? Query or Command? What should the Result contain?"
2. **Verify the pattern**: "Should this use primary constructor? What are we caching?"
3. **Default to async**: Always use `async Task<T>`, never synchronous wrappers
4. **Validate inputs**: Check `request.Data` immediately in handlers
5. **Use extensions**: For filtering, mapping, and common operations
6. **Return Result<T>**: Never throw exceptions for business logic, return failures
7. **Pass CancellationToken**: To all async method calls
8. **Cache reads**: Use 12-hour expiry for query results
9. **Invalidate on writes**: Remove relevant cache keys in command handlers
10. **Document decisions**: Use `///` XML comments for public methods

## Things to NEVER Do

- ❌ Use `.Result` or `.Wait()` on async methods
- ❌ Create backing fields with underscore prefix (`_field`)
- ❌ Duplicate mapping code (create extensions instead)
- ❌ Skip null checks on request data
- ❌ Forget `CancellationToken` in async calls
- ❌ Return exceptions instead of Result<T> failures
- ❌ Use synchronous EF Core methods
- ❌ Put business logic in controllers
- ❌ Create handlers without primary constructors
- ❌ Forget to invalidate cache after writes

## When to Ask for Human Review

- Complex business logic that spans multiple entities
- Security-sensitive operations (auth, permissions)
- Performance-critical code paths
- New architectural patterns not in this guide
- Significant refactoring or technical debt work

## Project Context

- **Framework**: .NET 10.0
- **ORM**: Entity Framework Core (in-memory for tests)
- **Message Bus**: LiteBus (CQRS commands/queries)
- **Validation**: FluentValidation
- **Testing**: xUnit with Moq
- **API Documentation**: Swagger/NSwag
- **Architecture**: Clean Architecture with CQRS

---

**Last Updated**: October 30, 2025  
**For**: Pezza Pizza Ordering System  
**Version**: .NET 10.0
