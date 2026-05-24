# MediatR → LiteBus Migration Guide — Big Bang Approach

**Status**: Migration Planning  
**Approach**: Rename all files/interfaces first, then fix code  
**Scope**: All Phases (1-9)  
**Date**: October 30, 2025  

---

## 📋 Complete Interface Mapping

### Commands (Write Operations)

| MediatR                                | LiteBus                              | File Pattern           | Notes                   |
| -------------------------------------- | ------------------------------------ | ---------------------- | ----------------------- |
| `IRequest`                             | `ICommand`                           | `XyzCommand.cs`        | Commands without result |
| `IRequest<T>`                          | `ICommand<T>`                        | `XyzCommand.cs`        | Commands with result    |
| `IRequestHandler<TRequest>`            | `ICommandHandler<TCommand>`          | `XyzCommandHandler.cs` | No result handler       |
| `IRequestHandler<TRequest, TResponse>` | `ICommandHandler<TCommand, TResult>` | `XyzCommandHandler.cs` | With result handler     |
| Create folder                          | Create folder                        | `Commands/Handlers/`   | Organize by type        |
| Create folder                          | Create folder                        | `Commands/Validators/` | IValidator integration  |

**Example Rename**:
```
MediatR:
  Features/Pizza/Commands/CreatePizzaCommand.cs → IRequest<PizzaModel>
  Features/Pizza/Handlers/CreatePizzaCommandHandler.cs → IRequestHandler<CreatePizzaCommand, PizzaModel>

LiteBus:
  Features/Pizza/Commands/CreatePizzaCommand.cs → ICommand<PizzaModel>
  Features/Pizza/Commands/Handlers/CreatePizzaCommandHandler.cs → ICommandHandler<CreatePizzaCommand, PizzaModel>
  Features/Pizza/Commands/Validators/CreatePizzaCommandValidator.cs → IValidator<CreatePizzaCommand>
```

### Queries (Read Operations)

| MediatR                                | LiteBus                          | File Pattern          | Notes                     |
| -------------------------------------- | -------------------------------- | --------------------- | ------------------------- |
| `IRequest<T>`                          | `IQuery<T>`                      | `XyzQuery.cs`         | Query that returns result |
| `IRequestHandler<TRequest, TResponse>` | `IQueryHandler<TQuery, TResult>` | `XyzQueryHandler.cs`  | Query handler             |
| Create folder                          | Create folder                    | `Queries/Handlers/`   | Organize by type          |
| Create folder                          | Create folder                    | `Queries/Validators/` | IValidator integration    |

**Example Rename**:
```
MediatR:
  Features/Pizza/Queries/GetPizzasQuery.cs → IRequest<ListResult<PizzaModel>>
  Features/Pizza/Handlers/GetPizzasQueryHandler.cs → IRequestHandler<GetPizzasQuery, ListResult<PizzaModel>>

LiteBus:
  Features/Pizza/Queries/GetPizzasQuery.cs → IQuery<ListResult<PizzaModel>>
  Features/Pizza/Queries/Handlers/GetPizzasQueryHandler.cs → IQueryHandler<GetPizzasQuery, ListResult<PizzaModel>>
```

### Pipeline Behaviors → Validators

| MediatR                                  | LiteBus               | File Pattern           | Notes               |
| ---------------------------------------- | --------------------- | ---------------------- | ------------------- |
| `IPipelineBehavior<TRequest, TResponse>` | `IValidator<T>`       | `XyzValidator.cs`      | Validation behavior |
| Folder: `Behaviors/`                     | Folder: `Validators/` | `Commands/Validators/` | Command validators  |
| Folder: `Behaviors/`                     | Folder: `Validators/` | `Queries/Validators/`  | Query validators    |

**Example Rename**:
```
MediatR:
  Behaviors/ValidationBehavior.cs → IPipelineBehavior
  Behaviors/LoggingBehavior.cs → IPipelineBehavior

LiteBus:
  Commands/Validators/CreatePizzaCommandValidator.cs → IValidator<CreatePizzaCommand>
  Queries/Validators/GetPizzasQueryValidator.cs → IValidator<GetPizzasQuery>
```

### Mediator Usage

| MediatR                        | LiteBus                                                       | Notes                  |
| ------------------------------ | ------------------------------------------------------------- | ---------------------- |
| `IMediator mediator`           | `ICommandMediator cmdMediator` + `IQueryMediator qryMediator` | Two separate mediators |
| `await mediator.Send(request)` | `await cmdMediator.SendAsync(command)`                        | For commands           |
| `await mediator.Send(request)` | `await qryMediator.QueryAsync(query)`                         | For queries            |

**Example**:
```csharp
// MediatR
public class PizzaController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePizzaModel model)
    {
        var result = await mediator.Send(new CreatePizzaCommand { Data = model });
    }
}

// LiteBus
public class PizzaController(ICommandMediator cmdMediator, IQueryMediator qryMediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePizzaModel model)
    {
        var result = await cmdMediator.SendAsync(new CreatePizzaCommand { Data = model });
    }
}
```

---

## 🗂️ File Reorganization Strategy

### Current MediatR Structure (Example - Pizza Feature)

```
Core/Pizza/
├── Commands/
│   ├── CreatePizzaCommand.cs
│   ├── UpdatePizzaCommand.cs
│   └── DeletePizzaCommand.cs
├── Queries/
│   ├── GetPizzasQuery.cs
│   ├── GetPizzaByIdQuery.cs
│   └── GetPizzasByCriteriaQuery.cs
├── Handlers/
│   ├── CreatePizzaCommandHandler.cs
│   ├── UpdatePizzaCommandHandler.cs
│   ├── DeletePizzaCommandHandler.cs
│   ├── GetPizzasQueryHandler.cs
│   ├── GetPizzaByIdQueryHandler.cs
│   └── GetPizzasByCriteriaQueryHandler.cs
└── Behaviors/
    └── (No specific validation behaviors in Pezza)
```

### New LiteBus Structure (Example - Pizza Feature)

```
Core/Pizza/
├── Commands/
│   ├── CreatePizzaCommand.cs           (ICommand<T>)
│   ├── UpdatePizzaCommand.cs           (ICommand<T>)
│   ├── DeletePizzaCommand.cs           (ICommand<T>)
│   ├── Handlers/
│   │   ├── CreatePizzaCommandHandler.cs    (ICommandHandler<T, R>)
│   │   ├── UpdatePizzaCommandHandler.cs    (ICommandHandler<T, R>)
│   │   └── DeletePizzaCommandHandler.cs    (ICommandHandler<T, R>)
│   └── Validators/
│       ├── CreatePizzaCommandValidator.cs  (IValidator<T>)
│       ├── UpdatePizzaCommandValidator.cs  (IValidator<T>)
│       └── DeletePizzaCommandValidator.cs  (IValidator<T>)
├── Queries/
│   ├── GetPizzasQuery.cs                   (IQuery<T>)
│   ├── GetPizzaByIdQuery.cs                (IQuery<T>)
│   └── GetPizzasByCriteriaQuery.cs         (IQuery<T>)
│   ├── Handlers/
│   │   ├── GetPizzasQueryHandler.cs        (IQueryHandler<T, R>)
│   │   ├── GetPizzaByIdQueryHandler.cs     (IQueryHandler<T, R>)
│   │   └── GetPizzasByCriteriaQueryHandler.cs (IQueryHandler<T, R>)
│   └── Validators/
│       ├── GetPizzasQueryValidator.cs      (IValidator<T>)
│       ├── GetPizzaByIdQueryValidator.cs   (IValidator<T>)
│       └── GetPizzasByCriteriaQueryValidator.cs (IValidator<T>)
```

---

## 📝 Interface Changes Summary

### 1. **Command Request → Command**

**Before (MediatR)**:
```csharp
namespace Core.Pizza.Commands;

public class CreatePizzaCommand : IRequest<Result<PizzaModel>>
{
    public CreatePizzaModel? Data { get; set; }
}
```

**After (LiteBus)**:
```csharp
namespace Core.Pizza.Commands;

public class CreatePizzaCommand : ICommand<Result<PizzaModel>>
{
    public CreatePizzaModel? Data { get; set; }
}
```

**Changes**:
- `IRequest<T>` → `ICommand<T>`
- `using MediatR;` → `using LiteBus.Commands.Abstractions;`
- Same file location

### 2. **Query Request → Query**

**Before (MediatR)**:
```csharp
namespace Core.Pizza.Queries;

public class GetPizzasQuery : IRequest<ListResult<PizzaModel>>
{
    public SearchPizzaModel Data { get; set; }
}
```

**After (LiteBus)**:
```csharp
namespace Core.Pizza.Queries;

public class GetPizzasQuery : IQuery<ListResult<PizzaModel>>
{
    public SearchPizzaModel Data { get; set; }
}
```

**Changes**:
- `IRequest<T>` → `IQuery<T>`
- `using MediatR;` → `using LiteBus.Queries.Abstractions;`
- Same file location

### 3. **Command Handler**

**Before (MediatR)**:
```csharp
namespace Core.Pizza.Commands;

public class CreatePizzaCommandHandler(DatabaseContext db, IAppCache cache) 
    : IRequestHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> Handle(
        CreatePizzaCommand request, 
        CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

**After (LiteBus)**:
```csharp
namespace Core.Pizza.Commands.Handlers;

public class CreatePizzaCommandHandler(DatabaseContext db, IAppCache cache) 
    : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> HandleAsync(
        CreatePizzaCommand command, 
        CancellationToken cancellationToken = default)
    {
        // Implementation - same code, just renamed
    }
}
```

**Changes**:
- `IRequestHandler<TRequest, TResponse>` → `ICommandHandler<TCommand, TResult>`
- Namespace: `Core.Pizza.Commands` → `Core.Pizza.Commands.Handlers`
- Method: `Handle(TRequest, CancellationToken)` → `HandleAsync(TCommand, CancellationToken = default)`
- `using MediatR;` → `using LiteBus.Commands.Abstractions;`
- Parameter name: `request` → `command` (convention)

### 4. **Query Handler**

**Before (MediatR)**:
```csharp
namespace Core.Pizza.Queries;

public class GetPizzasQueryHandler(DatabaseContext db, IAppCache cache)
    : IRequestHandler<GetPizzasQuery, ListResult<PizzaModel>>
{
    public async Task<ListResult<PizzaModel>> Handle(
        GetPizzasQuery request, 
        CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

**After (LiteBus)**:
```csharp
namespace Core.Pizza.Queries.Handlers;

public class GetPizzasQueryHandler(DatabaseContext db, IAppCache cache)
    : IQueryHandler<GetPizzasQuery, ListResult<PizzaModel>>
{
    public async Task<ListResult<PizzaModel>> HandleAsync(
        GetPizzasQuery query, 
        CancellationToken cancellationToken = default)
    {
        // Implementation - same code, just renamed
    }
}
```

**Changes**:
- `IRequestHandler<TRequest, TResponse>` → `IQueryHandler<TQuery, TResult>`
- Namespace: `Core.Pizza.Queries` → `Core.Pizza.Queries.Handlers`
- Method: `Handle(TRequest, CancellationToken)` → `HandleAsync(TQuery, CancellationToken = default)`
- `using MediatR;` → `using LiteBus.Queries.Abstractions;`
- Parameter name: `request` → `query` (convention)

### 5. **Validation - Pipeline Behavior → IValidator**

**Before (MediatR) - Global Behavior**:
```csharp
namespace Infrastructure.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
            throw new ValidationException(failures);

        return await next();
    }
}
```

**After (LiteBus) - Command Validator**:
```csharp
namespace Core.Pizza.Commands.Validators;

public class CreatePizzaCommandValidator : AbstractValidator<CreatePizzaCommand>
{
    public CreatePizzaCommandValidator()
    {
        RuleFor(c => c.Data).NotNull().WithMessage("Pizza data is required");
        RuleFor(c => c.Data.Name).NotEmpty().WithMessage("Pizza name is required");
        RuleFor(c => c.Data.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}
```

**Changes**:
- No more `IPipelineBehavior` - validation is automatic via LiteBus integration
- Use `AbstractValidator<T>` from FluentValidation directly
- Create specific validator per command/query
- LiteBus integrates FluentValidation natively

**LiteBus Registration** (in Program.cs):
```csharp
services.AddLiteBus(config =>
{
    config.AddCommandModule(builder =>
    {
        builder.RegisterFromAssembly(typeof(CreatePizzaCommand).Assembly);
    });
    config.AddQueryModule(builder =>
    {
        builder.RegisterFromAssembly(typeof(GetPizzasQuery).Assembly);
    });
});

// FluentValidation registered separately
services.AddValidatorsFromAssembly(typeof(CreatePizzaCommandValidator).Assembly);
```

---

## 🔄 Controller Changes

**Before (MediatR)**:
```csharp
namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController(IMediator mediator) : ApiController
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePizzaModel model)
    {
        var result = await mediator.Send(new CreatePizzaCommand { Data = model });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var result = await mediator.Send(new GetPizzasQuery { Data = new() });
        return ResponseHelper.ResponseOutcome(result, this);
    }
}
```

**After (LiteBus)**:
```csharp
namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController(ICommandMediator cmdMediator, IQueryMediator qryMediator) : ApiController
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePizzaModel model)
    {
        var result = await cmdMediator.SendAsync(new CreatePizzaCommand { Data = model });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var result = await qryMediator.QueryAsync(new GetPizzasQuery { Data = new() });
        return ResponseHelper.ResponseOutcome(result, this);
    }
}
```

**Changes**:
- `IMediator mediator` → `ICommandMediator cmdMediator, IQueryMediator qryMediator`
- `mediator.Send(command)` → `cmdMediator.SendAsync(command)`
- `mediator.Send(query)` → `qryMediator.QueryAsync(query)`

---

## 📦 NuGet Packages

**Remove**:
```xml
<PackageReference Include="MediatR" Version="12.x" />
<PackageReference Include="MediatR.Extensions.Microsoft.DependencyInjection" Version="12.x" />
```

**Add**:
```xml
<PackageReference Include="LiteBus" Version="1.x" />
<PackageReference Include="LiteBus.Extensions.FluentValidation" Version="1.x" />
<PackageReference Include="LiteBus.Extensions.Microsoft.Hosting" Version="1.x" />
```

---

## 🎯 File Renaming Checklist

### Phase 9 Example (Apply to all phases)

**Commands**:
- [ ] `CreatePizzaCommand.cs` - Change `IRequest<T>` → `ICommand<T>`
- [ ] `UpdatePizzaCommand.cs` - Change `IRequest<T>` → `ICommand<T>`
- [ ] `DeletePizzaCommand.cs` - Change `IRequest<T>` → `ICommand<T>`
- [ ] Move handlers to `Commands/Handlers/` subfolder
- [ ] Rename `CreatePizzaCommandHandler.cs` → namespace change only
- [ ] Create `Commands/Validators/` folder
- [ ] Create `CreatePizzaCommandValidator.cs` with `AbstractValidator<CreatePizzaCommand>`

**Queries**:
- [ ] `GetPizzasQuery.cs` - Change `IRequest<T>` → `IQuery<T>`
- [ ] `GetPizzaByIdQuery.cs` - Change `IRequest<T>` → `IQuery<T>`
- [ ] Move handlers to `Queries/Handlers/` subfolder
- [ ] Rename handler namespaces
- [ ] Create `Queries/Validators/` folder
- [ ] Create validators with `AbstractValidator<IQuery>`

**Controllers**:
- [ ] `PizzaController.cs` - Change DI from `IMediator` to `ICommandMediator, IQueryMediator`
- [ ] Update `.Send()` calls to `.SendAsync()` (commands) and `.QueryAsync()` (queries)
- [ ] All feature controllers (Pizza, Order, Crust, Sauce, etc.)

**DI Setup** (Program.cs):
- [ ] Remove MediatR registration
- [ ] Remove `AddMediatR()`
- [ ] Add LiteBus registration `AddLiteBus()`
- [ ] Register FluentValidation validators
- [ ] All projects that have handlers

---

## 📖 Summary: What to Rename

| Item              | Old                         | New                                                  | Example                                                                               |
| ----------------- | --------------------------- | ---------------------------------------------------- | ------------------------------------------------------------------------------------- |
| Command interface | `IRequest<T>`               | `ICommand<T>`                                        | `CreatePizzaCommand : ICommand<Result<PizzaModel>>`                                   |
| Query interface   | `IRequest<T>`               | `IQuery<T>`                                          | `GetPizzasQuery : IQuery<ListResult<PizzaModel>>`                                     |
| Command handler   | `IRequestHandler<Req, Res>` | `ICommandHandler<Cmd, Res>`                          | `CreatePizzaCommandHandler : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>` |
| Query handler     | `IRequestHandler<Req, Res>` | `IQueryHandler<Query, Res>`                          | `GetPizzasQueryHandler : IQueryHandler<GetPizzasQuery, ListResult<PizzaModel>>`       |
| Handler method    | `Handle()`                  | `HandleAsync()`                                      | Change method signature                                                               |
| Handler namespace | `Core.Pizza.Commands`       | `Core.Pizza.Commands.Handlers`                       | Folder structure change                                                               |
| Mediator in DI    | `IMediator`                 | `ICommandMediator, IQueryMediator`                   | Two separate mediators                                                                |
| Send method       | `mediator.Send()`           | `cmdMediator.SendAsync() / qryMediator.QueryAsync()` | Different method names                                                                |
| Validation        | `IPipelineBehavior`         | `IValidator<T>`                                      | Per-command/query validators                                                          |
| Validator folder  | `Behaviors/`                | `Commands/Validators/, Queries/Validators/`          | New folder structure                                                                  |

---

## 🚀 Implementation Order (Big Bang)

1. **All Phases - File Structure**
   - Create `Commands/Handlers/`, `Commands/Validators/`, `Queries/Handlers/`, `Queries/Validators/` folders

2. **All Phases - Move Files**
   - Move handlers to `Commands/Handlers/` and `Queries/Handlers/`

3. **All Phases - Interface Changes** (Bulk search/replace)
   - `IRequest<` → `ICommand<`  (for commands)
   - `IRequest<` → `IQuery<` (for queries)
   - `IRequestHandler<` → `ICommandHandler<`
   - `IRequestHandler<` → `IQueryHandler<`
   - `using MediatR;` → `using LiteBus.Commands.Abstractions;` (or Queries)

4. **All Phases - Method Renames**
   - `Handle(` → `HandleAsync(`
   - Parameter `request` → `command` or `query`

5. **All Phases - Create Validators**
   - Extract validation logic from existing code
   - Create `CreateXyzValidator.cs` files

6. **All Projects - DI Setup**
   - Update `Program.cs` with LiteBus registration

7. **All Controllers** 
   - Update DI: `IMediator` → `ICommandMediator, IQueryMediator`
   - Update calls: `.Send()` → `.SendAsync()` or `.QueryAsync()`

8. **Update Documentation**
   - Update DEVELOP_WITH_AI.md
   - Update COPILOT-INSTRUCTIONS.md
   - Create LiteBus migration guide

---

## 📊 File Count Estimation

Based on Pezza structure:

| Phase     | Commands | Queries | Handlers | Total | New Validators |
| --------- | -------- | ------- | -------- | ----- | -------------- |
| 1-8       | ~20      | ~20     | ~40      | ~80   | ~40            |
| 9         | ~30      | ~30     | ~60      | ~120  | ~60            |
| **Total** | ~170     | ~170    | ~340     | ~680  | ~340           |

**Scope**: ~680 files with interface changes, ~340 new validator files

---

**Ready for Phase 1 of implementation?**  
Should I start with:
1. Creating the bulk rename scripts?
2. Analyzing Phase 9 code first?
3. Creating LiteBus setup template for Program.cs?

Which would help most?
