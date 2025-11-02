# &nbsp;**Pezza - Phase 3** [![.NET - Phase 3](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase3-finalsolution.yml/badge.svg?branch=master)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase3-finalsolution.yml)

![Pezza logo](./Assets/pezza-logo.png)

## Quick facts

- .NET SDK required: 10 (net10)
- Solution format: **.slnx** (modern format)
- Estimated time: 6 - 10 hours
- Difficulty: ★★★★☆ (advanced intermediate)
- Audience: developers who completed Phase 2; recommended for those learning custom CQRS patterns and understanding mediator architecture from first principles
- **Key Focus**: Build your own lightweight CQRS dispatcher and understand why frameworks like MediatR exist

## Goal

This phase teaches **CQRS (Command Query Responsibility Segregation)** through building your own custom, lightweight dispatcher pattern. Rather than using an external library like MediatR or LiteBus, you'll implement a minimal CQRS library in-house, learning:

- How mediator patterns work under the hood
- Why dispatcher abstraction matters
- How to design extensible, reusable framework code
- Trade-offs between simplicity and feature-richness

By implementing your own minimal CQRS dispatcher, you'll understand:
1. **When** to use this pattern
2. **How** it works internally
3. **Why** it's better than directly calling services
4. **What trade-offs** exist in different implementations

This is **hands-on architecture learning** — not just "use this library" — so you can make informed decisions about patterns and tools in real projects.

See Microsoft docs: [CQRS pattern](https://docs.microsoft.com/azure/architecture/patterns/cqrs)

## Architecture Overview

### **Phase 2 vs Phase 3: A Natural Progression**

| Aspect                | Phase 2 (FromServices)                           | Phase 3 (Dispatcher)                      |
| --------------------- | ------------------------------------------------ | ----------------------------------------- |
| **Pattern**           | Direct service injection                         | Centralized dispatcher                    |
| **Service Discovery** | Controller knows service type                    | Controller uses dispatcher only           |
| **Extensibility**     | Limited (each controller must know all services) | Open (dispatcher can add features)        |
| **Learning Value**    | DI fundamentals                                  | Architecture & inversion of control       |
| **Use Case**          | Simple CRUD                                      | Growing business logic                    |
| **Code Flexibility**  | Service interface changes = controller changes   | Service changes isolated from controllers |

### **Why Add a Dispatcher?**

**Phase 2 approach (direct services):**

```csharp
[HttpPost]
public async Task<Result> Create(
    [FromServices] ICreatePizzaCommand service,  // Controller knows about service
    CreatePizzaDto dto,
    CancellationToken ct)
    => await service.ExecuteAsync(dto, ct);
```

**Problems:**

- Controllers must import and know about specific services
- Adding validation, logging, or caching requires modifying every controller and service
- Hard to test because each controller-service combo is tightly coupled
- No single place to apply cross-cutting concerns

**Phase 3 approach (dispatcher):**

```csharp
[HttpPost]
public async Task<Result> Create(
    CreatePizzaCommand command,          // Just data
    [FromServices] Dispatcher dispatcher, // Single integration point
    CancellationToken ct)
    => await dispatcher.Send(command, ct); // Dispatcher routes to handler
```

**Benefits:**

- Controllers are **decoupled** from specific services
- **Single dispatcher** knows how to route and handle all commands/queries
- Easy to add cross-cutting concerns (logging, validation, transactions) **in one place**
- New handlers don't require controller changes
- Testable: mock the dispatcher, not 10 different services

## The Custom CQRS Library: MediatorLite

This phase includes `Common/CQRS/MediatorLite.cs` — a minimal CQRS implementation you'll understand completely:

```csharp
public interface ICommand<TResult> { }
public interface IQuery<TResult> { }

public interface ICommandHandler<TCommand, TResult> 
    where TCommand : ICommand<TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken ct);
}

public interface IQueryHandler<TQuery, TResult> 
    where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken ct);
}

public class Dispatcher(IServiceProvider provider)
{
    public Task<TResult> Send<TCommand, TResult>(TCommand command, CancellationToken ct = default)
        where TCommand : ICommand<TResult>
    {
        var handler = provider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return handler.Handle(command, ct);
    }

    public Task<TResult> Query<TQuery, TResult>(TQuery query, CancellationToken ct = default)
        where TQuery : IQuery<TResult>
    {
        var handler = provider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return handler.Handle(query, ct);
    }
}
```

**That's it!** ~60 lines of code contains the entire pattern.

### **How It Works**

1. **Command/Query definition** → Lightweight data object inheriting from `ICommand<T>` or `IQuery<T>`
2. **Handler registration** → Scanned and registered by Scrutor in DependencyInjection.cs
3. **Dispatcher sends** → `dispatcher.Send(command, ct)` → Retrieves matching handler from DI container
4. **Handler executes** → Business logic runs, returns result
5. **Controller receives** → Result returned to caller

```
Controller
   ↓
Dispatcher.Send(command, ct)
   ↓
DI Container: GetRequiredService<ICommandHandler<CreatePizzaCommand, PizzaModel>>()
   ↓
Handler executes business logic
   ↓
Result returned to controller
```

## Step-by-Step: Phase 3 Structure

### **Step 1: Build the Foundation**

**Directory:** `src/02. Step1`

Focus:

- Understand the Dispatcher and MediatorLite library
- Implement basic commands and queries
- See how handlers are automatically discovered
- Learn dispatcher extension methods

**Key Files:**

- `Common/CQRS/MediatorLite.cs` — The entire CQRS library (read this first!)
- `Core/Pizza/Commands/CreatePizzaCommand.cs` — Command definition + handler
- `Core/Pizza/Queries/GetPizzaQuery.cs` — Query definition + handler
- `Api/Controllers/PizzaController.cs` — Using dispatcher
- `Api/Helpers/DispatcherExtensions.cs` — Convenience methods

**Learning Goals:**

1. [ ] Read and understand all 60 lines of MediatorLite.cs
2. [ ] Understand how generic constraints work in the dispatcher
3. [ ] See how Scrutor auto-registers handlers
4. [ ] Write your first command/query pair

### **Step 2: Add Business Value**

**Directory:** `src/03. EndSolution`

Focus:

- Implement complete CRUD via commands and queries
- Add compiled EF Core queries for performance
- See how the pattern scales
- Add customer commands and queries

**Key Improvements:**

- Compiled queries in `CustomerQueries.cs` using `EF.CompileAsyncQuery` for 20-30% performance improvement
- Separate query service (`GetPizzasQuery`) for listing with better structure
- Complete CRUD: Create, Read, Update, Delete via commands

**Why Compiled Queries?**

EF Core normally compiles queries at runtime:

```csharp
// Compiled at runtime (slower)
var pizza = await db.Pizzas.FirstOrDefaultAsync(p => p.Id == id);
```

With compiled queries:

```csharp
// Compiled once at startup (faster)
private static readonly Func<DatabaseContext, int, CancellationToken, Task<Pizza?>>
    GetPizzaById = EF.CompileAsyncQuery(
        (DatabaseContext db, int id, CancellationToken ct) =>
            db.Pizzas.FirstOrDefault(p => p.Id == id));

public async Task<PizzaModel?> ExecuteAsync(int id, CancellationToken ct)
    => (await GetPizzaById(db, id, ct))?.Map();
```

**Performance benefit**: 20-30% faster for frequently-called queries.

**When to use:**

- High-traffic query handlers
- Query patterns that don't change
- Where you profiled and found hotspots

## Implementation Patterns

### **Creating a Command**

```csharp
// In Core/Pizza/Commands/CreatePizzaCommand.cs

public sealed class CreatePizzaCommand : ICommand<Result<PizzaModel>>
{
    public required CreatePizzaDto Data { get; set; }
}

public sealed class CreatePizzaCommandHandler(DatabaseContext db) 
    : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> Handle(
        CreatePizzaCommand command, 
        CancellationToken ct)
    {
        var entity = new Pizza 
        { 
            Name = command.Data.Name,
            Description = command.Data.Description,
            Price = command.Data.Price,
            DateCreated = DateTime.UtcNow
        };
        
        db.Pizzas.Add(entity);
        await db.SaveChangesAsync(ct);
        
        return Result<PizzaModel>.Success(entity.Map());
    }
}
```

### **Creating a Query**

```csharp
// In Core/Pizza/Queries/GetPizzaQuery.cs

public sealed class GetPizzaQuery : IQuery<Result<PizzaModel?>>
{
    public required int Id { get; set; }
}

public sealed class GetPizzaQueryHandler(DatabaseContext db) 
    : IQueryHandler<GetPizzaQuery, Result<PizzaModel?>>
{
    // Compiled query for performance
    private static readonly Func<DatabaseContext, int, CancellationToken, Task<Pizza?>>
        GetById = EF.CompileAsyncQuery((DatabaseContext ctx, int id, CancellationToken _) =>
            ctx.Pizzas.FirstOrDefault(p => p.Id == id));

    public async Task<Result<PizzaModel?>> Handle(GetPizzaQuery query, CancellationToken ct)
    {
        var pizza = await GetById(db, query.Id, ct);
        return pizza == null 
            ? Result<PizzaModel?>.Success(null) 
            : Result<PizzaModel?>.Success(pizza.Map());
    }
}
```

### **Using Dispatcher in Controllers**

```csharp
// In Api/Controllers/PizzaController.cs

[ApiController]
[Route("[controller]")]
public sealed class PizzaController : ApiController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        CreatePizzaCommand command,
        [FromServices] Dispatcher dispatcher,
        CancellationToken ct)
    {
        var result = await dispatcher.Send(command, ct);
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Get(
        [FromRoute] int id,
        [FromServices] Dispatcher dispatcher,
        CancellationToken ct)
    {
        var query = new GetPizzaQuery { Id = id };
        var result = await dispatcher.Query(query, ct);
        return ResponseHelper.ResponseOutcome(result, this);
    }
}
```

### **Why Extension Methods?**

Notice in the controller above: `dispatcher.Send()` and `dispatcher.Query()`.

These are convenience methods in `Api/Helpers/DispatcherExtensions.cs`:

```csharp
public static class DispatcherExtensions
{
    // Simplify: No need to specify <TCommand, TResult> — compiler infers it
    public static Task<TResult> Send<TResult>(
        this Dispatcher dispatcher, 
        ICommand<TResult> command, 
        CancellationToken ct = default)
        => dispatcher.Send<ICommand<TResult>, TResult>(command, ct);

    // Same for queries
    public static Task<TResult> Query<TQuery, TResult>(
        this Dispatcher dispatcher, 
        TQuery query, 
        CancellationToken ct = default)
        where TQuery : IQuery<TResult>
        => dispatcher.Query<TQuery, TResult>(query, ct);
}
```

**Why they matter:**

- **Without extensions**: `await dispatcher.Send<CreatePizzaCommand, Result<PizzaModel>>(cmd, ct)` — verbose
- **With extensions**: `await dispatcher.Send(cmd, ct)` — clean, generic type inference

## Dependency Injection Setup

Handlers are **auto-discovered and registered** using Scrutor:

```csharp
// In Core/DependencyInjection.cs

public static IServiceCollection AddApplication(this IServiceCollection services)
{
    services.AddScoped<Dispatcher>();

    // Auto-scan and register all ICommandHandler<,> implementations
    services.Scan(scan => scan
        .FromAssemblyOf<CreatePizzaHandler>()
        .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

    // Auto-scan and register all IQueryHandler<,> implementations
    services.Scan(scan => scan
        .FromAssemblyOf<GetPizzaHandler>()
        .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

    return services;
}
```

**How it works:**

1. Scans assembly for all classes implementing `ICommandHandler<,>`
2. Registers each as its own interface type (e.g., `ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>`)
3. When dispatcher calls `GetRequiredService<ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>>()`, Scrutor finds it automatically
4. No manual registration needed — add new handlers, they're discovered automatically

## Where This Leads

### **Next Phases Will Add**

1. **Validation Behaviors** — Decorators that wrap handlers to validate commands before execution
2. **Logging & Metrics** — Cross-cutting concerns applied to all handlers
3. **Transaction Handling** — Automatic transaction wrapping
4. **Notification/Events** — Domain events published after successful commands
5. **Caching** — Query result caching layer

### **From Here to Production CQRS**

This simple dispatcher is the **foundation** for:

- **MediatR** — Adds pipelines, behaviors, notifications (overcomplicated for most apps)
- **LiteBus** — Focuses on lightweight CQRS without overkill
- **NServiceBus / MassTransit** — Distributed CQRS with message queues

You now understand what all of these are doing under the hood.

## Common Mistakes & Solutions

### **Mistake 1: Handler Not Found**

```
InvalidOperationException: No service for type 
'ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>' has been registered.
```

**Causes:**

- Handler class doesn't implement `ICommandHandler<TCommand, TResult>`
- Handler is in a different namespace (Scrutor predicate is too narrow)
- Handler name doesn't follow pattern (typo in class name)

**Solution:**

```csharp
// ✅ Correct
public sealed class CreatePizzaCommandHandler 
    : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> Handle(...) { }
}

// ❌ Wrong (missing interface)
public sealed class CreatePizzaCommandHandler
{
    public async Task<Result<PizzaModel>> Handle(...) { }
}

// ❌ Wrong (typo in handler class name)
public sealed class CreatePizzaHanler 
    : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
```

### **Mistake 2: Generic Type Mismatch**

```
InvalidOperationException: No service registered for type 
'ICommandHandler<ICommand<Result<PizzaModel>>, Result<PizzaModel>>'
```

**Cause:** Using wrong type in dispatcher call

**Solution:**

```csharp
// ❌ Wrong
var result = await dispatcher.Send<ICommand<Result<PizzaModel>>, Result<PizzaModel>>(cmd, ct);

// ✅ Correct (let compiler infer via extensions)
var result = await dispatcher.Send(cmd, ct);

// ✅ Also correct (if using base Dispatcher.Send)
var result = await dispatcher.Send<CreatePizzaCommand, Result<PizzaModel>>(cmd, ct);
```

### **Mistake 3: Forgetting CancellationToken**

```csharp
// ❌ Lost cancellation support (request can't be cancelled)
var result = await query.ExecuteAsync(id);

// ✅ Pass through entire chain
public async Task ExecuteAsync(int id, CancellationToken ct)
    => (await GetPizzaById(db, id, ct))?.Map();
```

## Performance Considerations

### **Compiled Queries**

When to use `EF.CompileAsyncQuery`:

| Scenario                                 | Use Compiled? | Benefit                          |
| ---------------------------------------- | ------------- | -------------------------------- |
| High-traffic GET endpoint (100+ req/sec) | ✅ Yes         | 20-30% faster                    |
| Infrequently-called business logic       | ❌ No          | Negligible difference            |
| Complex query with filters/joins         | ⚠️ Maybe       | Profile first                    |
| Single entity by primary key             | ✅ Yes         | Big win                          |
| List/search with dynamic filters         | ❌ No          | Can't compile dynamic predicates |

### **Dispatcher Overhead**

The dispatcher adds minimal overhead:

- One DI lookup: ~1μs (negligible vs database I/O)
- One reflection-free generic type resolution
- No allocation in the happy path

**Don't optimize prematurely** — measure first.

## Testing Handlers

```csharp
[TestClass]
public class CreatePizzaCommandHandlerTests
{
    private DatabaseContext db = null!;
    private CreatePizzaCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        db = new DatabaseContext(new DbContextOptionsBuilder()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
        _handler = new CreatePizzaCommandHandler(db);
    }

    [TestMethod]
    public async Task Handle_WithValidCommand_ReturnsSavedPizza()
    {
        var command = new CreatePizzaCommand 
        { 
            Data = new CreatePizzaDto 
            { 
                Name = "Margherita",
                Price = 99m
            }
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Margherita", result.Data.Name);
        Assert.IsTrue(result.Data.Id > 0);
    }
}
```

**Key insights:**

- Test handlers directly — no need for dispatcher
- Use in-memory database for fast, isolated tests
- Focus on business logic, not framework plumbing

## Next Phase

**Phase 4** will build on this by:

- Adding more business logic (customer commands, order processing)
- Introducing validation behaviors
- Adding transaction handling
- Showing how to extend the dispatcher with decorators

## Resources & Further Learning

- [CQRS Pattern — Microsoft](https://docs.microsoft.com/azure/architecture/patterns/cqrs)
- [Why MediatR Exists — Jimmy Bogard](https://jimmybogard.com/mediatr-hangs-dogfood/)
- [Mediator Pattern — Design Patterns](https://refactoring.guru/design-patterns/mediator)
- [Generic Constraints in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters)

## Troubleshooting

**Q: Do I need async/await for queries?**

A: Yes. Keep everything async for scalability. Even if a query is fast, use `async` to avoid blocking threads.

**Q: Can handlers have dependencies?**

A: Absolutely! Inject anything you need:

```csharp
public sealed class CreatePizzaCommandHandler(
    DatabaseContext db,
    ILogger<CreatePizzaCommandHandler> logger,
    ICache cache) 
    : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
```

**Q: What if I need to call another command from within a handler?**

A: Inject the dispatcher and call it:

```csharp
public sealed class CreateOrderCommandHandler(
    DatabaseContext db,
    Dispatcher dispatcher) 
    : ICommandHandler<CreateOrderCommand, Result<OrderModel>>
{
    public async Task<Result<OrderModel>> Handle(CreateOrderCommand command, CancellationToken ct)
    {
        // First, create pizza if needed
        var pizzaCmd = new CreatePizzaCommand { Data = command.PizzaData };
        await dispatcher.Send(pizzaCmd, ct);
        
        // Then create order...
    }
}
```

This is considered **orchestration** — use carefully to avoid making commands too interdependent.

## Implement CQRS Pattern


## Next Phase

Phase 4 will build on this foundation by introducing:
- Command and Query handler implementation
- Validation behavior using FluentValidation
- Advanced CQRS patterns
- Customer and Order commands/queries
