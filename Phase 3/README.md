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

**Phase 3** builds on Phase 2's handler pattern by introducing a **lightweight dispatcher** that centralizes command/query routing. Instead of controllers knowing about specific handler interfaces, they now send commands/queries through a single dispatcher.

### **Why Build Your Own Dispatcher?**

Rather than using MediatR or another framework, you'll implement a minimal CQRS dispatcher (~60 lines) to understand:

1. **How** mediator patterns work internally
2. **When** centralized routing beats direct injection
3. **What** trade-offs exist between simplicity and features

This is hands-on architecture—you'll know exactly how the abstraction works because you built it.

See Microsoft docs: [CQRS pattern](https://docs.microsoft.com/azure/architecture/patterns/cqrs)

## Architecture Overview

### **From Phase 2 to Phase 3: Why Add a Dispatcher?**

**Phase 2 (Explicit Injection):**

```csharp
[HttpPost]
public async Task<ActionResult> Create(
    [FromServices] ICreatePizzaCommand handler,  // Controller knows handler type
    CreatePizzaModel model,
    CancellationToken ct)
    => ResponseHelper.ResponseOutcome(await handler.ExecuteAsync(model, ct), this);
```

**Limitations:**

- Controllers must import handler interfaces
- Each action needs `[FromServices]` injection
- Adding cross-cutting concerns (logging, validation) requires changing every handler
- No single place to apply consistent behavior

**Phase 3 (Dispatcher):**

```csharp
[HttpPost]
public async Task<ActionResult> Create(
    CreatePizzaCommand command,       // Just data
    CancellationToken ct)
    => ResponseHelper.ResponseOutcome(
        await dispatcher.Send(command, ct),  // Dispatcher routes
        this);
```

**Benefits:**

- Controllers **decoupled** from handler types
- **Single point** to add logging, validation, transactions
- New handlers don't require controller changes
- Easier testing: mock the dispatcher instead of individual handlers

## The Custom Dispatcher (MediatorLite)

Located in `Common/CQRS/MediatorLite.cs`, this ~60-line dispatcher contains everything you need:

```csharp
public interface ICommand<TResult> { }
public interface IQuery<TResult> { }

public interface ICommandHandler<TCommand, TResult> 
    where TCommand : ICommand<TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken ct);
}

public class Dispatcher(IServiceProvider provider)
{
    public Task<TResult> Send<TCommand, TResult>(TCommand command, CancellationToken ct = default)
        where TCommand : ICommand<TResult>
    {
        var handler = provider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return handler.Handle(command, ct);
    }
    // ... Query method follows same pattern
}
```

**How It Works:**

1. Command/query inherits from `ICommand<TResult>` or `IQuery<TResult>`
2. Handler implements `ICommandHandler<TCommand, TResult>`
3. Scrutor scans and registers handlers automatically
4. Dispatcher resolves the right handler via generic constraints and DI
5. Business logic executes, returns `Result<T>`

## Step-by-Step: Phase 3 Structure

### **Step 1: Build the Foundation**

**Directory:** `src/02. Step1`

Focus:

- Understand the Dispatcher (MediatorLite) library
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

### ❌ **Mistake 1: Handler Not Found**

```cs
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

### ❌ **Mistake 2: Generic Type Mismatch**

```cs
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

### ❌ **Mistake 3: Forgetting CancellationToken**

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

---

## 🎓 What You Should Know By Now

After completing Phase 3, you should understand:

### **Dispatcher Pattern**
- Why centralized routing beats explicit `[FromServices]` at scale
- How generic constraints (`where TCommand : ICommand<TResult>`) enable type-safe dispatch
- The trade-off: slightly more abstraction for much better extensibility
- When to build your own vs. use a library like MediatR

### **Generic Constraints**
- How `ICommand<TResult>` constrains the dispatcher's `Send` method
- Why the compiler can infer types: `dispatcher.Send(command, ct)` → finds the right handler
- Using `GetRequiredService<ICommandHandler<TCommand, TResult>>()` for DI resolution

### **Scrutor Assembly Scanning**
- How `.FromAssemblyOf<T>()` locates types in the same assembly
- Registering implementations via `.AsImplementedInterfaces()`
- Why scanning beats manual registration for growing codebases

### **Architectural Insights**
- **Separation of concerns**: Commands carry data; handlers contain logic; controllers orchestrate
- **Single responsibility**: Each handler does one thing (create pizza, fetch pizza, etc.)
- **Open/closed**: Add new handlers without changing existing controllers
- **Cross-cutting**: Dispatcher becomes the place to add logging, validation, caching

### **Testing Benefits**
- Mock the dispatcher instead of 10+ individual handler interfaces
- Test handlers in isolation without controllers or HTTP
- In-memory DbContext keeps tests fast and repeatable

**Next**: Phase 4 adds validation behaviors, transaction handling, and demonstrates how to extend the dispatcher with decorators.

---

## Next Phase

Phase 4 will enhance the dispatcher by:

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
