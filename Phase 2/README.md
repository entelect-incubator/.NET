
# &nbsp;**Pezza - Phase 2** [![.NET - Phase 2](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase2-finalsolution.yml/badge.svg?branch=master)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase2-finalsolution.yml)

![Pezza logo](./Assets/pezza-logo.png)

## Quick facts

- .NET SDK required: 10 (net10)
- Solution format: **.slnx** (modern format)
- Estimated time: 4 - 8 hours
- Difficulty: ★★★☆☆ (intermediate)
- Audience: developers who completed Phase 1 or have basic CRUD/EF Core experience; recommended for those learning CQRS pattern with LiteBus mediator

## Goal

This phase teaches **CQRS fundamentals** through the simplest possible approach: **FromServices dependency injection**. Rather than using a mediator library, you'll inject command/query services directly into controllers, learning:

- How to structure business logic in service classes
- Separation of concerns (queries separate from commands)
- Clean dependency injection patterns
- How to test services in isolation
- Why this pattern is great for onboarding and clarity

This phase prioritizes **clarity and simplicity** over architectural sophistication. You'll understand the problem that CQRS solves before learning solutions like mediators (Phase 3+).

See Microsoft docs: [CQRS pattern](https://docs.microsoft.com/azure/architecture/patterns/cqrs)

## Modern .NET 10 Patterns in This Phase

This phase demonstrates several contemporary C# and .NET 10 patterns:

### **1. FromServices Pattern: Direct Dependency Injection**

**FromServices** is an ASP.NET Core feature that injects services directly into action methods, rather than controller constructors. This keeps controllers lean and follows the Single Responsibility Principle:

```csharp
// Query service interface - handles data retrieval
public interface IGetPizzaQuery
{
    Task<PizzaModel?> ExecuteAsync(int id, CancellationToken ct);
}

// Query handler - implements the query logic
public sealed class GetPizzaQueryHandler(DatabaseContext db) : IGetPizzaQuery
{
    public async Task<PizzaModel?> ExecuteAsync(int id, CancellationToken ct)
    {
        var pizza = await db.Pizzas
            .FirstOrDefaultAsync(p => p.Id == id, ct);
        return pizza?.Map();
    }
}

// Controller uses FromServices for injection
[ApiController]
[Route("[controller]")]
public sealed class PizzaController : ApiController
{
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(
        int id,
        [FromServices] IGetPizzaQuery query,  // Injected by FromServices
        CancellationToken ct)
    {
        var result = await query.ExecuteAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
```

**Why FromServices?**

| Benefit                  | Explanation                                    |
| ------------------------ | ---------------------------------------------- |
| **Clarity**              | It's obvious what each action depends on       |
| **No Constructor Bloat** | Controllers stay simple                        |
| **Testable**             | Mock the injected service easily               |
| **Scope Control**        | Services are scoped per-request automatically  |
| **Learning-Friendly**    | New developers see exactly what a method needs |

### **2. Automatic Handler Discovery with Scrutor**

Services are **auto-discovered and registered** using the Scrutor library for assembly scanning:

```csharp
// In Api/Startup.cs
public static void ConfigureServices(IServiceCollection services)
{
    // Register all IGetPizzaQuery, ICreatePizzaCommand, etc. implementations
    services.Scan(scan => scan
        .FromAssemblyOf<GetPizzaQueryHandler>()
        .AddClasses(c => 
            c.Where(t => t.Name.EndsWith("Query") || t.Name.EndsWith("Command")))
        .AsImplementedInterfaces()
        .WithScopedLifetime());
    
    // Result: All query/command handlers are registered automatically
}
```

**How it works:**

1. Scrutor scans your assemblies for classes matching predicates
2. Registers each as its interface (e.g., `GetPizzaQueryHandler` → `IGetPizzaQuery`)
3. New handlers are discovered automatically without manual registration
4. Controllers then inject via `[FromServices]`

### **3. CancellationToken in All Async Methods**
All async methods now properly support cancellation tokens for graceful shutdown and timeout handling:

```cs
// CancellationToken parameter in every async method
[HttpGet("{id}")]
public async Task<ActionResult> Get(int id, CancellationToken cancellationToken = default)
{
    var result = await pizzaCore.GetAsync(id);
    return result == null ? NotFound() : Ok(result);
}
```

#### **What are CancellationTokens?**

**CancellationTokens** are a mechanism in .NET that allows you to gracefully cancel long-running asynchronous operations. They work by propagating cancellation signals from the top-level caller down through the entire async call chain.

**Why CancellationTokens are important:**

- **Graceful Shutdown**: Allows your application to shut down cleanly without orphaning operations
- **Timeout Handling**: Cancels operations that take too long
- **Resource Cleanup**: Ensures proper disposal of resources even when operations are interrupted
- **User Responsiveness**: Users can cancel slow requests without waiting indefinitely
- **Server Load Management**: Reduces server load by stopping expensive operations early

**How CancellationTokens work:**

1. A CancellationTokenSource is created, which signals cancellation
2. A CancellationToken is derived from the source and passed down the call chain
3. Operations periodically check `cancellationToken.ThrowIfCancellationRequested()`
4. When cancellation is signaled, an `OperationCanceledException` is thrown
5. All async calls in the chain stop and resources are cleaned up

**Examples of where cancellation happens:**

- User closes their browser (HTTP connection drops)
- Request timeout (ASP.NET Core has built-in timeout)
- Server is shutting down (graceful shutdown signal)
- Admin cancels long-running operation
- Database query times out

**Usage pattern in ASP.NET Core:**

```csharp
// ASP.NET Core automatically cancels when:
// 1. Client disconnects
// 2. Request timeout occurs
// 3. Server shutdown is initiated

public async Task<ActionResult> Get(int id, CancellationToken cancellationToken = default)
{
    // Pass cancellation token to all async operations
    var pizza = await database.Pizzas
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    
    // The operation can be cancelled at any point
    // If cancelled, OperationCanceledException is thrown automatically
    return pizza is null ? NotFound() : Ok(pizza);
}
```

**Good practices:**

- Always include `CancellationToken cancellationToken = default` in async method signatures
- Pass it through the entire call chain: `await operation(cancellationToken)`
- Never ignore or suppress cancellation exceptions (they indicate intentional cancellation)
- Use it with database calls, HTTP calls, and long-running operations

### **3. EF Core Compiled Queries for Performance**

Entity Framework Core normally compiles queries at runtime, which adds overhead for frequently-used queries. **Compiled queries** are pre-compiled at startup for maximum performance:

**Without Compiled Queries (Runtime Compilation):**

```csharp
public async Task<PizzaModel?> ExecuteAsync(int id, CancellationToken ct)
{
    // ❌ Compiled every time this method is called
    var pizza = await db.Pizzas
        .FirstOrDefaultAsync(p => p.Id == id, ct);
    return pizza?.Map();
}
```

**Performance**: Slower for high-traffic endpoints (query compilation overhead)

**With Compiled Queries (Pre-Compiled at Startup):**

```csharp
public sealed class GetPizzaQueryHandler(DatabaseContext db) : IGetPizzaQuery
{
    // ✅ Compiled once at startup, reused on every call
    private static readonly Func<DatabaseContext, int, CancellationToken, Task<Pizza?>>
        GetPizzaById = EF.CompileAsyncQuery(
            (DatabaseContext ctx, int id, CancellationToken _) =>
                ctx.Pizzas.FirstOrDefault(p => p.Id == id));

    public async Task<PizzaModel?> ExecuteAsync(int id, CancellationToken ct)
    {
        var pizza = await GetPizzaById(db, id, ct);
        return pizza?.Map();
    }
}
```

**Performance**: 20-30% faster for high-traffic queries (no compilation overhead)

**When to Use Compiled Queries:**

| Scenario                                 | Use Compiled? | Benefit                          |
| ---------------------------------------- | ------------- | -------------------------------- |
| High-traffic GET endpoint (100+ req/sec) | ✅ Yes         | 20-30% faster                    |
| Single entity by ID lookup               | ✅ Yes         | Significant win                  |
| Infrequently-called business logic       | ❌ No          | Negligible difference            |
| Complex query with many conditions       | ⚠️ Maybe       | Profile first                    |
| Dynamic query with filter parameters     | ❌ No          | Can't compile dynamic predicates |

**Key Rules for Compiled Queries:**

1. Must be `static readonly` — defined at class level
2. Query predicates must be constant — no dynamic filters
3. Receives `(DatabaseContext, parameters...)` in lambda
4. Returns `Func<DatabaseContext, params..., Task<T>>` for async queries
5. The CancellationToken parameter in the lambda is ignored (but required)

**Example: CustomerQueries with Compiled Queries**

```csharp
public sealed class GetCustomersQueryHandler(DatabaseContext db) : IGetCustomersQuery
{
    // Get all customers
    private static readonly Func<DatabaseContext, CancellationToken, Task<List<Customer>>>
        GetAll = EF.CompileAsyncQuery(
            (DatabaseContext ctx, CancellationToken _) =>
                ctx.Customers.OrderBy(c => c.Name).ToList());

    // Get customer by ID
    private static readonly Func<DatabaseContext, int, CancellationToken, Task<Customer?>>
        GetById = EF.CompileAsyncQuery(
            (DatabaseContext ctx, int id, CancellationToken _) =>
                ctx.Customers.FirstOrDefault(c => c.Id == id));

    // Get customers by name prefix
    private static readonly Func<DatabaseContext, string, CancellationToken, Task<List<Customer>>>
        GetByNamePrefix = EF.CompileAsyncQuery(
            (DatabaseContext ctx, string prefix, CancellationToken _) =>
                ctx.Customers
                    .Where(c => c.Name.StartsWith(prefix))
                    .OrderBy(c => c.Name)
                    .ToList());

    public async Task<List<CustomerModel>> ExecuteAsync(CancellationToken ct)
        => (await GetAll(db, ct)).Select(c => c.Map()).ToList();

    public async Task<CustomerModel?> ExecuteAsync(int id, CancellationToken ct)
        => (await GetById(db, id, ct))?.Map();

    public async Task<List<CustomerModel>> ExecuteAsync(string namePrefix, CancellationToken ct)
        => (await GetByNamePrefix(db, namePrefix, ct))
            .Select(c => c.Map())
            .ToList();
}
```

**What to Profile:**

```csharp
// Before optimization
[Benchmark]
public async Task<PizzaModel?> GetWithoutCompiledQuery()
    => await new GetPizzaQueryHandler(db)
        .ExecuteAsync(1, CancellationToken.None);

// After optimization
[Benchmark]
public async Task<PizzaModel?> GetWithCompiledQuery()
    => await new GetPizzaCompiledQueryHandler(db)
        .ExecuteAsync(1, CancellationToken.None);

// Result: ~20-30% faster with compiled queries
```

### **4. Null Pattern Comparison: `is null` vs `== null`**

Modern C# prefers the `is null` pattern over `== null` for several reasons:

```csharp
// ❌ Old style (not recommended)
if (result.Data == null)
{
    return NotFound();
}

// ✅ Modern style (preferred in C# 9+)
if (result.Data is null)
{
    return NotFound();
}

// ✅ Even better: negated null check
if (result.Data is not null)
{
    return Ok(result.Data);
}
```

**Why `is null` is better:**

1. **Operator Overloading Vulnerability**: `== null` can be overloaded by types, leading to unexpected behavior
2. **Clarity**: `is null` is a pattern match, making intent explicit and unambiguous
3. **Consistency**: Aligns with modern C# patterns (switch expressions, record equality, etc.)
4. **Type Safety**: Compiler understands null-safety better with pattern matching
5. **Performance**: Slightly better optimized by JIT compiler
6. **Readability**: `is not null` is clearer than `!= null`

**C# 9+ Recommendation:**
- Use `is null` / `is not null` for explicit null checks
- Combine with nullable reference types for compile-time safety

### **2.6. EF Core Compiled Queries - Performance at Scale**

Compiled queries pre-compile LINQ expressions at application startup, eliminating query compilation overhead on each execution:

```cs
// Define compiled query at class level
private static readonly Func<DatabaseContext, int, Task<Pizza?>> GetPizzaByIdQuery =
    EF.CompileAsyncQuery((DatabaseContext db, int id) => 
        db.Pizzas.FirstOrDefault(c => c.Id == id));

// Use in handler
public async Task<Result<PizzaModel>> HandleAsync(UpdatePizzaCommand request, CancellationToken cancellationToken)
{
    var pizza = await GetPizzaByIdQuery(databaseContext, request.Id);
    // ... rest of logic
}
```

**Why compiled queries are good:**
1. **Reduced CPU Overhead**: Query compilation happens once, not per request
2. **Predictable Performance**: Eliminates variable compilation time in high-throughput scenarios
3. **Scale Benefit**: Massive impact with millions of queries across application lifetime
4. **Memory Efficiency**: No repeated query AST allocations and compilations
5. **Modern Alternative**: Replaces old `CompiledQuery` API with modern async pattern

**When to use:**
- High-frequency queries (called thousands of times)
- Hot paths in your application
- Query patterns that are fixed (parameters vary, structure doesn't)
- Performance-critical scenarios

**When not needed:**
- One-off queries
- Admin pages with infrequent access
- Queries with highly variable LINQ structures

### **2.7. The `required` Keyword - Enforcing Initialization**

The `required` keyword (C# 11+) enforces that a property must be set during object initialization:

```cs
public sealed class UpdatePizzaCommand : ICommand<Result<PizzaModel>>
{
    public required int Id { get; set; }
    public required UpdatePizzaModel Data { get; set; }
}

public sealed class UpdateCustomerCommand : ICommand<Result<CustomerModel>>
{
    public required int Id { get; set; }
    public required UpdateCustomerModel Data { get; set; }
}
```

**Why `required` is valuable:**
1. **Compile-Time Safety**: Missing required properties are caught by the compiler
2. **Cleaner Code**: No null checks needed for truly required data
3. **CQRS Clarity**: Commands always have the data they need
4. **API Contracts**: HTTP clients know what fields are mandatory
5. **Eliminates Validation**: Reduces need for "X is required" error messages

**Usage pattern:**
```cs
// ✅ Valid - all required properties set
var cmd = new UpdatePizzaCommand 
{ 
    Id = 1, 
    Data = new UpdatePizzaModel { Name = "New Name" } 
};

// ❌ Compilation error - missing Id
var badCmd = new UpdatePizzaCommand 
{ 
    Data = new UpdatePizzaModel { Name = "New Name" } 
};
```

**Combined with init-only:**
```cs
public sealed class UpdatePizzaCommand : ICommand<Result<PizzaModel>>
{
    public required int Id { get; init; }
    public required UpdatePizzaModel Data { get; init; }
}
```

### **2.8. Async Database Operations - CountAsync vs Count()**

Always use async counterparts for database operations to prevent thread pool starvation:

```cs
// ❌ Blocks thread pool (blocking query execution)
var count = entities.Count();

// ✅ Async - proper resource management
var count = await entities.CountAsync(cancellationToken);
```

**Why `CountAsync` is better:**
1. **Non-Blocking**: Returns control to thread pool while database works
2. **Scalability**: Prevents thread pool starvation under high load
3. **Cancellation Support**: Respects cancellation tokens
4. **Modern ASP.NET Requirement**: Essential for ASP.NET Core's scalability model
5. **Consistent Pattern**: All I/O operations should be async

**Complete async pattern:**
```cs
// Good pattern for queries
var count = await _context.Pizzas
    .AsNoTracking()           // Optimize for read-only
    .CountAsync(cancellationToken);

var items = await _context.Pizzas
    .AsNoTracking()
    .ToListAsync(cancellationToken);
```

### **2.9. AsNoTracking() - Optimizing Read-Only Queries**

`AsNoTracking()` tells Entity Framework not to track returned entities, improving performance for read-only scenarios:

```cs
// Tracking enabled (default) - for updates
var pizza = await db.Pizzas
    .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
pizza.Name = "New Name";
await db.SaveChangesAsync(cancellationToken);

// No tracking - for reads only
var pizza = await db.Pizzas
    .AsNoTracking()
    .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
// pizza is read-only, cannot be modified and tracked
```

**Why `AsNoTracking()` is good:**
1. **Reduced Memory**: Change tracker overhead eliminated
2. **CPU Efficiency**: No tracking state management
3. **Faster Queries**: Skips identity map lookups
4. **Better for Lists**: Extremely beneficial when loading many items
5. **Clear Intent**: Shows query is for read-only access

**Usage guidelines:**
- Use for search/read queries (GetAll, Search, etc.)
- Use for reporting and analytics
- Skip for Create/Update/Delete operations
- Combine with `CountAsync()` for reports

### **3. Expression-Bodied Members**

Clean, concise code using expression bodies:

```cs
// Expression body for simple implementations
public async Task<ActionResult> Search(CancellationToken cancellationToken = default)
    => Ok(await pizzaCore.GetAllAsync());

// Ternary expressions for conditional returns
return result == null ? BadRequest() : Ok(result);
```

### **3.5. Sealed Classes for Commands and Queries**

All Command and Query classes are marked with the `sealed` keyword to improve performance and express design intent:

```cs
// Sealed Command classes
public sealed class CreateCustomerCommand : ICommand<Result<CustomerModel>>
{
    public CreateCustomerModel? Data { get; set; }
}

// Sealed Query classes
public sealed class GetCustomerQuery : IQuery<Result<CustomerModel>>
{
    public int Id { get; set; }
}
```

**Why seal Command and Query classes?**

Commands and Queries in CQRS are data transfer objects that carry behavior instructions—they're not meant to be base classes or extended via inheritance. Sealing them provides three key benefits:

1. **Lower CPU Overhead (Virtual Call Elimination)**
   - Without `sealed`, .NET must assume subclasses might exist and use virtual dispatch for method calls
   - `sealed` eliminates this overhead by telling the runtime: "No subclasses possible"
   - Particularly important at scale when thousands of commands/queries are dispatched

2. **Smaller Per-Type Memory Footprint**
   - Sealing allows the JIT compiler to optimize method table layout
   - Reduces metadata overhead
   - Each CQRS class is typically instantiated many times

3. **Cleaner Semantics**
   - Explicitly communicates: "This is a leaf class, don't inherit from it"
   - Prevents accidental (and incorrect) inheritance that violates CQRS principles
   - These aren't immutable value types—they're behavior carriers meant to be used as-is
   - Reduces cognitive load for developers reading the code

**Impact:**
- All Command classes across Phases 3-12 are now sealed
- All Query classes across Phases 3-12 are now sealed
- Handler classes are also sealed (same reasoning)
- Combined with other optimizations, this contributes to Phase 2 EndSolution's improved throughput

```

All public members include proper XML documentation with parameter descriptions:

```cs
/// <summary>
/// Get Pizza by Id.
/// </summary>
/// <param name="id">Pizza Id</param>
/// <param name="cancellationToken">Cancellation token</param>
/// <returns>ActionResult with Pizza model or NotFound</returns>
[HttpGet("{id}")]
public async Task<ActionResult> Get(int id, CancellationToken cancellationToken = default)
```

### **5. ApiController Base Class Pattern**

All controllers inherit from a reusable base controller that provides access to LiteBus mediators:

```cs
public abstract class ApiController : ControllerBase
{
    private ICommandMediator? cmdMediator;
    private IQueryMediator? qryMediator;

    /// <summary>Gets the command mediator for dispatching commands.</summary>
    protected ICommandMediator CmdMediator => cmdMediator ??= HttpContext.RequestServices.GetRequiredService<ICommandMediator>();

    /// <summary>Gets the query mediator for dispatching queries.</summary>
    protected IQueryMediator QryMediator => qryMediator ??= HttpContext.RequestServices.GetRequiredService<IQueryMediator>();
}
```

This pattern:

- Provides lazy initialization of mediators (only created when first accessed)
- Uses null-coalescing assignment operator `??=` (modern C# pattern)
- Keeps mediator logic centralized in the base class
- Prepares the foundation for Phase 3+ where commands and queries will be implemented

### **6. Modern Solution Format (.slnx)**

This phase uses the modern **.slnx** solution file format instead of legacy `.sln`, providing:

- Better version control compatibility
- Improved performance
- Cleaner text format
- Better IDE support

### **7. Separation of Program.cs and Startup.cs - The Bootstrap Pattern**

This phase demonstrates the **Startup Pattern**, which separates application bootstrap logic from configuration logic:

#### **Why Separate Program.cs and Startup.cs?**

**Program.cs** (the bootstrapper):
```cs
// Program.cs - Minimal, clean entry point
var builder = WebApplication.CreateBuilder(args);
var startup = new Api.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);      // Register services
var app = builder.Build();
startup.Configure(app, builder.Environment);      // Configure middleware pipeline
app.Run();
```

**Startup.cs** (the configuration hub):
```cs
// Startup.cs - All configuration centralized here
public class Startup
{
    public IConfiguration ConfigRoot { get; }
    public Startup(IConfiguration configuration) => this.ConfigRoot = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        // Add DbContext
        services.AddDbContext<DatabaseContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString())
        );
        
        // Add Swagger
        services.AddSwaggerGen(c => { /* ... */ });
        
        // Add application services (from dependency injection)
        services.AddApplication();
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        // Configure middleware pipeline
        app.UseExceptionHandler();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API"));
        app.UseHttpsRedirection();
        app.UseRouting();
        app.MapControllers();
    }
}
```

#### **Benefits of This Pattern:**

| Aspect                     | Benefit                                                                                                 |
| -------------------------- | ------------------------------------------------------------------------------------------------------- |
| **Clarity**                | Program.cs remains minimal (10 lines) while configuration lives in Startup.cs                           |
| **Testability**            | Startup can be unit tested independently (mock IConfiguration, IServiceCollection, IWebHostEnvironment) |
| **Reusability**            | Same Startup class can be used across multiple entry points (e.g., different hosts)                     |
| **Legacy Compatibility**   | Familiar to developers from .NET Framework and .NET Core 2.x-3.x                                        |
| **Maintainability**        | All configuration in one place is easier to navigate than top-level statements                          |
| **Separation of Concerns** | Bootstrap logic (Program.cs) is separate from configuration logic (Startup.cs)                          |

#### **Program.cs vs Top-Level Statements:**

**Top-level statements** (C# 9+, minimalist approach):
```cs
// Newer approach - everything inline
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>();
var app = builder.Build();
app.UseSwagger();
app.MapControllers();
app.Run();
```

**Startup Pattern** (this phase's approach):
```cs
// Organized approach - separation of concerns
var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
var app = builder.Build();
startup.Configure(app, builder.Environment);
app.Run();
```

**Why Phase 2 uses the Startup Pattern:**
1. **Progressive Disclosure** - Configuration logic can grow without cluttering the entry point
2. **Testing Strategy** - Each method in Startup can have unit tests
3. **Phase Progression** - Sets up pattern that works better when adding middleware, policies, and advanced features in later phases
4. **Industry Standard** - This pattern has proven scalability for enterprise applications

#### **Exception Handler Registration - AddExceptionHandler Pattern:**

The pattern properly separates exception handler registration into the `DependencyInjection.cs`:

```cs
// Core/DependencyInjection.cs - where handlers are REGISTERED
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssemblyContaining<CreateCustomerCommand>());

        // Register the CONCRETE exception handler
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}

// Api/Startup.cs - where services are CONFIGURED
public void ConfigureServices(IServiceCollection services)
{
    // ... other services ...
    
    // Call the extension method from Core
    services.AddApplication();  // This registers GlobalExceptionHandler
}

// Api/Program.cs - where middleware pipeline is ACTIVATED
public void Configure(WebApplication app, IWebHostEnvironment env)
{
    // ACTIVATE the exception handler middleware
    app.UseExceptionHandler();  // Uses the registered GlobalExceptionHandler
}
```

**Key points:**
- `services.AddExceptionHandler<GlobalExceptionHandler>()` **registers** the handler type
- `app.UseExceptionHandler()` **activates** the middleware that uses registered handlers
- Never call `services.AddExceptionHandler()` without a type - it won't have a handler to use!

## Prerequisites

- Completed Phase 1 (familiarity with the solution layout and basic CRUD)
- .NET 10 SDK installed and on PATH (dotnet --version should report a 10.x SDK)
- Basic knowledge of the dotnet CLI and Visual Studio/VS Code
- Familiarity with async/await patterns

## How to validate this phase locally

1. Build the start solution:

```powershell
dotnet build "Phase 2/src/01. StartSolution/Pezza.slnx"
```

2. Run the API project:

```powershell
cd "Phase 2/src/01. StartSolution"
dotnet run --project Api/Api.csproj
```

3. Test the endpoints via Swagger at `https://localhost:7001/swagger`

## Architecture Overview

```
┌─────────────────────────────────────────┐
│   API Layer (Controllers)                │
│   - PizzaController                      │
│   - Cancellation Token Support           │
│   - Expression-bodied endpoints          │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│   LiteBus Mediator Layer                 │
│   - ICommandMediator (CmdMediator)       │
│   - IQueryMediator (QryMediator)         │
│   - Service Discovery & Routing          │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│   Core Business Logic Layer              │
│   - PizzaCore (Business Logic)           │
│   - Commands & Queries (Future)          │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│   Data Access Layer                      │
│   - Entity Framework Core                │
│   - DatabaseContext                      │
│   - In-Memory Database                   │
└─────────────────────────────────────────┘
```

## Steps (work through these in order)

- [ ] Use the Start Solution from Phase 2 to get started - **Phase 2/src/01. StartSolution**
- [ ] [Step 1 - Scaffolding](https://github.com/entelect-incubator/.NET/tree/master/Phase%202/Step%201)
- [ ] [Step 2 - Unit Tests](https://github.com/entelect-incubator/.NET/tree/master/Phase%202/Step%202)
- [ ] [Step 3 - API](https://github.com/entelect-incubator/.NET/tree/master/Phase%202/Step%203)

## Notes and learning outcomes

- Learn how to structure Commands and Queries using LiteBus (lightweight mediator pattern)
- Understand CQRS pattern: separate read and write operations
- Understand separation of concerns: keep domain logic in Core and orchestration in API/Application layers
- Master CancellationToken usage in async operations for graceful shutdown and timeout handling
- Use expression-bodied members for clean, concise code
- Implement complete XML documentation for better IDE support and Swagger generation
- Use dependency injection with LiteBus for service discovery and mediator routing
- Understand the modern .slnx solution file format and its benefits

## Key Code Changes in Phase 2

### **Controllers**
- ✅ Added `CancellationToken cancellationToken = default` parameter to all async methods
- ✅ Cleaned up controller methods to use expression bodies where appropriate
- ✅ Fixed XML documentation to include all parameters
- ✅ Removed verbose `this.` qualifiers, using modern C# style

### **ApiController Base Class**
- ✅ Created base controller inheriting from `ControllerBase`
- ✅ Added protected properties for `ICommandMediator` and `IQueryMediator`
- ✅ Implemented lazy initialization of mediators using null-coalescing operator

### **Dependency Injection (Core/DependencyInjection.cs)**
- ✅ Updated to use `AddLiteBusCommands()` and `AddLiteBusQueries()`
- ✅ Automatically discovers and registers handlers in the Core assembly
- ✅ Added XML documentation explaining configuration
- ✅ Removed old MediatR-specific code

### **Startup.cs (Api/Startup.cs)**
- ✅ Added comprehensive XML documentation
- ✅ Clarified service configuration
- ✅ Added comments explaining each configuration section

## Small checklist for reviewers

- ✅ Ensure all async methods have `CancellationToken cancellationToken = default` parameter
- ✅ Verify XML documentation includes all parameters (including cancellationToken)
- ✅ Confirm LiteBus is properly configured with `AddLiteBusCommands()` and `AddLiteBusQueries()`
- ✅ Check ApiController provides access to `CmdMediator` and `QryMediator`
- ✅ Verify expression bodies are used where appropriate
- ✅ Ensure no verbose `this.` qualifiers are used unnecessarily
- ✅ Confirm modern solution format (.slnx) is in use

## Next Phase

Phase 3 will build on this foundation by introducing:
- Command and Query handler implementation
- Validation behavior using FluentValidation
- Advanced CQRS patterns
- Customer and Order commands/queries
