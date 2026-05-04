# AI-Assisted Development — Quick Reference

## One-Page Cheat Sheet for .NET Developers

### 🎯 Before Coding
- [ ] Understand the architecture (CQRS, LiteBus, Result<T>)
- [ ] Design Request/Response types first
- [ ] Check if similar code exists (DRY)
- [ ] Identify abstractions (extensions, behaviors)

### 📝 Naming Rules

| What       | Format                    | Example               |
| ---------- | ------------------------- | --------------------- |
| Namespaces | Domain path               | `Core.Pizza.Commands` |
| Classes    | PascalCase                | `CreatePizzaCommand`  |
| Properties | PascalCase, NO underscore | `public string Name`  |
| Methods    | PascalCase                | `Handle`              |
| Local vars | camelCase                 | `var entity = ...`    |

**NEVER:** `public string _name` or `private string _pizza`

### 🔨 Constructor Pattern

```csharp
// ✅ DO THIS
public class Handler(DatabaseContext db, ICache cache) : IHandler { }

// ❌ DON'T DO THIS
public class Handler : IHandler
{
    private readonly DatabaseContext db;
    private readonly ICache _cache;
    public Handler(DatabaseContext db, ICache cache) { /* ... */ }
}
```

### 📦 Property Annotations

```csharp
public required int Id { get; set; }        // Must provide
public required string Name { get; set; }   // Must provide
public string? Description { get; set; }    // Optional (null OK)
public decimal? Price { get; set; }         // Optional (null OK)
```

### ⏳ Async Patterns

```csharp
// ✅ DO THIS
public async Task<Result<T>> Handle(Query request, CancellationToken ct)
{
    var result = await db.SaveChangesAsync(ct);
    return Result<T>.Success(data);
}

// ❌ DON'T DO THIS
var result = db.SaveChangesAsync().Result;  // Deadlock!
```

### 🎁 Result Pattern

```csharp
// Success
return Result<PizzaModel>.Success(entity.Map());

// Failure
return Result<PizzaModel>.Failure("Pizza not found");

// List
return ListResult<PizzaModel>.Success(data, total);
```

### 🔄 CQRS Separation

```csharp
// QUERY (Read only)
public class GetPizzasQuery : IQuery<ListResult<PizzaModel>> { }

// COMMAND (Write)
public class CreatePizzaCommand : ICommand<Result<PizzaModel>> { }

// HANDLER (One per command/query)
public class Handler(Deps d) : IQueryHandler<Query, Result> { }  // or ICommandHandler
```

### 🏗️ Extension Methods (DRY)

```csharp
// Map single
var model = entity.Map();

// Map list
var models = entities.Map();

// Filter
var filtered = list.FilterByName(name).FilterByPrice(min, max);
```

### 💬 AI Prompt Template

```
CONTEXT:
- Architecture: CQRS with LiteBus
- Pattern: Primary constructors, Result<T>, extension methods
- .NET: 10.0

TASK:
Generate [Query/Command]Handler for [feature]:
- Inject [dependencies] via primary constructor
- [specific business logic]
- Use Result<T>.Success() or .Failure()
- Cache with [duration] if applicable
- Map using .Map() extension
```

### ✅ Code Review Checklist

Before accepting AI-generated code:

- [ ] Primary constructor used? ✓
- [ ] No underscore properties? ✓
- [ ] Async/await correct? ✓
- [ ] CancellationToken passed? ✓
- [ ] Result<T> pattern used? ✓
- [ ] Null checks present? ✓
- [ ] No duplicate code? ✓
- [ ] Extension methods used? ✓
- [ ] Documented with `///`? ✓

### 🚫 Anti-Patterns to Avoid

| ❌ Bad                     | ✅ Good                           | Why                    |
| ------------------------- | -------------------------------- | ---------------------- |
| `var x = async.Result`    | `await async`                    | Prevents deadlock      |
| `public _name`            | `public Name`                    | Naming standard        |
| Logic in controller       | Logic in handler                 | Separation of concerns |
| Copy-paste mapping        | `.Map()` extension               | DRY principle          |
| `var x = y ?? z ?? throw` | `if(x == null) return Failure()` | Early returns, clarity |
| No `CancellationToken`    | Pass to all async                | Request cancellation   |

### 🎓 Common Patterns

```csharp
// Null guard
if (request.Data == null)
    return Result<T>.Failure("Invalid data");

// Save to DB
var rows = await db.SaveChangesAsync(ct);
return rows > 0 
    ? Result<T>.Success(entity.Map()) 
    : Result<T>.Failure("Save failed");

// Cache lookup
var cached = await cache.GetOrAddAsync(key, 
    () => GetData(), timespan);

// Filter chain
var result = list
    .FilterByName(search)
    .FilterByPrice(min, max)
    .OrderBy(x => x.DateCreated)
    .ToList();
```

### 📚 Files to Check First

- `Phase 9/src/01. StartSolution/Core/Pizza/Commands/CreatePizzaCommand.cs`
- `Phase 9/src/01. StartSolution/Api/Controllers/PizzaController.cs`
- `Phase 9/src/01. StartSolution/Common/Models/PizzaModel.cs`
- `Phase 9/src/01. StartSolution/Api/Controllers/ApiController.cs`

### 🤝 Workflow

1. **Type signature** → Let Copilot suggest
2. **Review** → Check against checklist
3. **Refactor** → Improve if needed
4. **Test** → Add unit tests
5. **Document** → Add XML comments

### 🎯 Remember

> AI assists architecture, design, and code review decisions. 
> You make the final call on patterns, structure, and quality.
