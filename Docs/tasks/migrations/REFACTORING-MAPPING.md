# Complete Refactoring Mapping - MediatR → LiteBus

**Document Version**: 1.0  
**Date**: October 30, 2025  
**Scope**: All Phases (1-9)  
**Status**: Ready for Execution  

---

## 📋 Table of Contents

1. [Global Interface Mapping](#global-interface-mapping)
2. [File Organization Patterns](#file-organization-patterns)
3. [Namespace Transformation Rules](#namespace-transformation-rules)
4. [Code Pattern Replacements](#code-pattern-replacements)
5. [Using Statement Updates](#using-statement-updates)
6. [Phase-by-Phase Inventory](#phase-by-phase-inventory)
7. [DI Configuration Mapping](#di-configuration-mapping)
8. [Execution Order](#execution-order)

---

## Global Interface Mapping

### 1. Request Interfaces

| Pattern                     | MediatR                  | LiteBus                 | Use Case                      |
| --------------------------- | ------------------------ | ----------------------- | ----------------------------- |
| Write operation             | `IRequest<TResponse>`    | `ICommand<TResult>`     | Commands that return a result |
| Write operation (no result) | `IRequest` (Unit return) | `ICommand`              | Fire-and-forget commands      |
| Read operation              | `IRequest<TResponse>`    | `IQuery<TResult>`       | Queries that return data      |
| Stream operation            | N/A                      | `IStreamQuery<TResult>` | Streaming/batch queries       |

### 2. Handler Interfaces

| Pattern             | MediatR                                | LiteBus                                | File Location        |
| ------------------- | -------------------------------------- | -------------------------------------- | -------------------- |
| Command with result | `IRequestHandler<TRequest, TResponse>` | `ICommandHandler<TCommand, TResult>`   | `Commands/Handlers/` |
| Command no result   | `IRequestHandler<TRequest>`            | `ICommandHandler<TCommand>`            | `Commands/Handlers/` |
| Query               | `IRequestHandler<TRequest, TResponse>` | `IQueryHandler<TQuery, TResult>`       | `Queries/Handlers/`  |
| Stream query        | N/A                                    | `IStreamQueryHandler<TQuery, TResult>` | `Queries/Handlers/`  |

### 3. Behavior/Handler Pipeline

| Pattern         | MediatR                   | LiteBus                   | Purpose             |
| --------------- | ------------------------- | ------------------------- | ------------------- |
| Pre-processing  | `IPipelineBehavior<T, R>` | `ICommandPreHandler<T>`   | Runs before command |
| Post-processing | `IPipelineBehavior<T, R>` | `ICommandPostHandler<T>`  | Runs after command  |
| Error handling  | `IPipelineBehavior<T, R>` | `ICommandErrorHandler<T>` | Runs on error       |
| Query pre       | `IPipelineBehavior<T, R>` | `IQueryPreHandler<T>`     | Runs before query   |
| Query post      | `IPipelineBehavior<T, R>` | `IQueryPostHandler<T>`    | Runs after query    |
| Validation      | `IPipelineBehavior<T, R>` | `IValidator<T>`           | Validates input     |

### 4. Mediator

| Operation    | MediatR                   | LiteBus                                   | Note           |
| ------------ | ------------------------- | ----------------------------------------- | -------------- |
| Send command | `IMediator.Send(command)` | `ICommandMediator.SendAsync(command)`     | Async only     |
| Query data   | `IMediator.Send(query)`   | `IQueryMediator.QueryAsync(query)`        | Async only     |
| Stream data  | N/A                       | `IQueryMediator.StreamAsync(query)`       | New capability |
| Register     | `AddMediatR(assembly)`    | `AddLiteBus(cfg => cfg.Add...(assembly))` | In DI setup    |

---

## File Organization Patterns

### Current MediatR Structure

```
Feature/
├── Commands/
│   ├── CreateXyzCommand.cs           ← IRequest<T>
│   ├── UpdateXyzCommand.cs           ← IRequest<T>
│   ├── DeleteXyzCommand.cs           ← IRequest<T>
│   ├── CreateXyzCommandValidator.cs  ← AbstractValidator<T>
│   └── (handlers nested in command files)
├── Queries/
│   ├── GetXyzQuery.cs                ← IRequest<T>
│   ├── GetXyzesQuery.cs              ← IRequest<T>
│   ├── GetXyzByIdQuery.cs            ← IRequest<T>
│   └── (handlers nested in query files)
└── Behaviors/
    ├── ValidationBehavior.cs         ← IPipelineBehavior (REMOVED)
    └── PerformanceBehaviour.cs       ← IPipelineBehavior (CONVERT)
```

### Target LiteBus Structure

```
Feature/
├── Commands/
│   ├── CreateXyzCommand.cs           ← ICommand<T> (ONLY command, no handler)
│   ├── UpdateXyzCommand.cs           ← ICommand<T>
│   ├── DeleteXyzCommand.cs           ← ICommand<T>
│   ├── Handlers/                     ← NEW FOLDER
│   │   ├── CreateXyzCommandHandler.cs ← ICommandHandler<T, R>
│   │   ├── UpdateXyzCommandHandler.cs ← ICommandHandler<T, R>
│   │   └── DeleteXyzCommandHandler.cs ← ICommandHandler<T, R>
│   └── Validators/                   ← NEW/REORGANIZED
│       ├── CreateXyzCommandValidator.cs ← AbstractValidator<T>
│       ├── UpdateXyzCommandValidator.cs
│       └── DeleteXyzCommandValidator.cs
├── Queries/
│   ├── GetXyzQuery.cs                ← IQuery<T> (ONLY query, no handler)
│   ├── GetXyzesQuery.cs              ← IQuery<T>
│   ├── GetXyzByIdQuery.cs            ← IQuery<T>
│   ├── Handlers/                     ← NEW FOLDER
│   │   ├── GetXyzQueryHandler.cs     ← IQueryHandler<T, R>
│   │   ├── GetXyzesQueryHandler.cs   ← IQueryHandler<T, R>
│   │   └── GetXyzByIdQueryHandler.cs ← IQueryHandler<T, R>
│   └── Validators/                   ← NEW/OPTIONAL
│       ├── GetXyzQueryValidator.cs   ← AbstractValidator<T>
│       └── GetXyzesQueryValidator.cs
└── PrePostHandlers/                  ← NEW (if needed)
    ├── XyzCommandLoggingHandler.cs   ← ICommandPreHandler<T>
    └── XyzCommandAuditingHandler.cs  ← ICommandPostHandler<T>
```

---

## Namespace Transformation Rules

### Pattern 1: Command Namespaces

```
BEFORE:
namespace Core.Feature.Commands;
public class CreateXyzCommand : IRequest<Result<Xyz>>
public class CreateXyzCommandHandler : IRequestHandler<CreateXyzCommand, Result<Xyz>>

AFTER:
# Command File
namespace Core.Feature.Commands;
public class CreateXyzCommand : ICommand<Result<Xyz>>

# Handler File (NEW location)
namespace Core.Feature.Commands.Handlers;
public class CreateXyzCommandHandler : ICommandHandler<CreateXyzCommand, Result<Xyz>>
```

### Pattern 2: Query Namespaces

```
BEFORE:
namespace Core.Feature.Queries;
public class GetXyzQuery : IRequest<Xyz>
public class GetXyzQueryHandler : IRequestHandler<GetXyzQuery, Xyz>

AFTER:
# Query File
namespace Core.Feature.Queries;
public class GetXyzQuery : IQuery<Xyz>

# Handler File (NEW location)
namespace Core.Feature.Queries.Handlers;
public class GetXyzQueryHandler : IQueryHandler<GetXyzQuery, Xyz>
```

### Pattern 3: Validator Namespaces

```
BEFORE:
namespace Core.Feature.Commands;
public class CreateXyzCommandValidator : AbstractValidator<CreateXyzCommand>

AFTER:
namespace Core.Feature.Commands.Validators;
public class CreateXyzCommandValidator : AbstractValidator<CreateXyzCommand>
```

---

## Code Pattern Replacements

### Type 1: Command Definition

```csharp
// ✗ BEFORE (MediatR)
using MediatR;

namespace Core.Pizza.Commands;

public class CreatePizzaCommand : IRequest<Result<PizzaModel>>
{
    public CreatePizzaModel? Data { get; set; }
}

public class CreatePizzaCommandHandler(DatabaseContext db) 
    : IRequestHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> Handle(
        CreatePizzaCommand request, 
        CancellationToken cancellationToken)
    {
        // Implementation
    }
}

// ✓ AFTER (LiteBus) - Command file ONLY
using LiteBus.Commands.Abstractions;

namespace Core.Pizza.Commands;

public class CreatePizzaCommand : ICommand<Result<PizzaModel>>
{
    public CreatePizzaModel? Data { get; set; }
}

// ✓ AFTER (LiteBus) - Handler file SEPARATE
using LiteBus.Commands.Abstractions;

namespace Core.Pizza.Commands.Handlers;

public class CreatePizzaCommandHandler(DatabaseContext db) 
    : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> HandleAsync(
        CreatePizzaCommand command, 
        CancellationToken cancellationToken = default)
    {
        // Implementation
    }
}
```

### Type 2: Query Definition

```csharp
// ✗ BEFORE (MediatR)
using MediatR;

namespace Core.Pizza.Queries;

public class GetPizzasQuery : IRequest<ListResult<PizzaModel>>
{
    public SearchPizzaModel Data { get; set; }
}

public class GetPizzasQueryHandler(DatabaseContext db) 
    : IRequestHandler<GetPizzasQuery, ListResult<PizzaModel>>
{
    public async Task<ListResult<PizzaModel>> Handle(
        GetPizzasQuery request, 
        CancellationToken cancellationToken)
    {
        // Implementation
    }
}

// ✓ AFTER (LiteBus) - Query file ONLY
using LiteBus.Queries.Abstractions;

namespace Core.Pizza.Queries;

public class GetPizzasQuery : IQuery<ListResult<PizzaModel>>
{
    public SearchPizzaModel Data { get; set; }
}

// ✓ AFTER (LiteBus) - Handler file SEPARATE
using LiteBus.Queries.Abstractions;

namespace Core.Pizza.Queries.Handlers;

public class GetPizzasQueryHandler(DatabaseContext db) 
    : IQueryHandler<GetPizzasQuery, ListResult<PizzaModel>>
{
    public async Task<ListResult<PizzaModel>> HandleAsync(
        GetPizzasQuery query, 
        CancellationToken cancellationToken = default)
    {
        // Implementation
    }
}
```

### Type 3: Validator

```csharp
// BEFORE (MediatR)
namespace Core.Pizza.Commands;

using FluentValidation;

public class CreatePizzaCommandValidator : AbstractValidator<CreatePizzaCommand>
{
    public CreatePizzaCommandValidator()
    {
        RuleFor(x => x.Data).NotNull();
        RuleFor(x => x.Data.Name).NotEmpty().MaximumLength(100);
    }
}

// AFTER (LiteBus) - SAME CODE, just reorganized namespace
namespace Core.Pizza.Commands.Validators;

using FluentValidation;

public class CreatePizzaCommandValidator : AbstractValidator<CreatePizzaCommand>
{
    public CreatePizzaCommandValidator()
    {
        RuleFor(x => x.Data).NotNull();
        RuleFor(x => x.Data.Name).NotEmpty().MaximumLength(100);
    }
}
```

### Type 4: Controller Updates

```csharp
// ✗ BEFORE (MediatR)
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PizzaController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePizzaModel model)
    {
        var result = await mediator.Send(
            new CreatePizzaCommand { Data = model });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var result = await mediator.Send(
            new GetPizzasQuery { Data = new() });
        return ResponseHelper.ResponseOutcome(result, this);
    }
}

// ✓ AFTER (LiteBus)
using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PizzaController(
    ICommandMediator cmdMediator,
    IQueryMediator qryMediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePizzaModel model)
    {
        var result = await cmdMediator.SendAsync(
            new CreatePizzaCommand { Data = model });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var result = await qryMediator.QueryAsync(
            new GetPizzasQuery { Data = new() });
        return ResponseHelper.ResponseOutcome(result, this);
    }
}
```

---

## Using Statement Updates

### Global Replacements

| Old              | New                                                                        | Applies To              |
| ---------------- | -------------------------------------------------------------------------- | ----------------------- |
| `using MediatR;` | `using LiteBus.Commands.Abstractions;`                                     | Command files, handlers |
| `using MediatR;` | `using LiteBus.Queries.Abstractions;`                                      | Query files, handlers   |
| `using MediatR;` | `using LiteBus.Commands.Abstractions; using LiteBus.Queries.Abstractions;` | Controllers, DI setup   |
| N/A              | `using LiteBus.Extensions.FluentValidation;`                               | DI setup (Program.cs)   |
| N/A              | `using LiteBus.Extensions.Microsoft.Hosting;`                              | DI setup (Program.cs)   |

### File-Specific Rules

**Command Files**:
- Remove: `using MediatR;`
- Add: `using LiteBus.Commands.Abstractions;`

**Query Files**:
- Remove: `using MediatR;`
- Add: `using LiteBus.Queries.Abstractions;`

**Handler Files** (Commands):
- Remove: `using MediatR;`
- Add: `using LiteBus.Commands.Abstractions;`

**Handler Files** (Queries):
- Remove: `using MediatR;`
- Add: `using LiteBus.Queries.Abstractions;`

**Controller Files**:
- Remove: `using MediatR;`
- Add: 
  - `using LiteBus.Commands.Abstractions;`
  - `using LiteBus.Queries.Abstractions;`

**Program.cs**:
- Remove: `using MediatR;`
- Remove: `using MediatR.Extensions.Microsoft.DependencyInjection;`
- Add:
  - `using LiteBus;`
  - `using LiteBus.Extensions.FluentValidation;`
  - `using LiteBus.Extensions.Microsoft.Hosting;`

---

## Phase-by-Phase Inventory

### Phase 1-8: Estimated Scope

Each phase typically has:
- **Commands**: 15-25 per phase
- **Queries**: 15-25 per phase
- **Handlers**: 30-50 nested in command/query files
- **Validators**: 5-15 per phase
- **Controllers**: 3-6 per phase

**Phase 1-8 Totals**: 
- ~170 commands
- ~170 queries
- ~340 handlers (to be separated)
- ~80 validators (to be reorganized)
- ~45 controllers

### Phase 9 (Most Complete)

**Confirmed by analysis** (see PHASE9-FILE-INVENTORY.md):
- **Commands**: 8 (Create, Update, Delete for Pizza, Customer, Order, Notify)
- **Queries**: 6 (Get* for Pizza, Customer, Order, Notify, Products, Stock)
- **Handlers**: 14 (nested in command/query files)
- **Validators**: 6 (CreatePizza, UpdatePizza, DeletePizza, CreateCustomer, UpdateCustomer, DeleteCustomer)
- **Controllers**: 4 (Pizza, Customer, Order, Notify)
- **Behaviors**: 2 (ValidationBehavior, PerformanceBehaviour - to remove/convert)

---

## DI Configuration Mapping

### MediatR Setup (Current)

```csharp
// Program.cs

using MediatR;
using MediatR.Extensions.Microsoft.DependencyInjection;

// ... other setup ...

// MediatR configuration
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreatePizzaCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetPizzasQuery).Assembly);
});

// Validation
builder.Services.AddValidatorsFromAssembly(typeof(CreatePizzaCommandValidator).Assembly);

// Behaviors (Pipeline)
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
```

### LiteBus Setup (Target)

```csharp
// Program.cs

using LiteBus;
using LiteBus.Extensions.FluentValidation;
using LiteBus.Extensions.Microsoft.Hosting;
using FluentValidation;

// ... other setup ...

// LiteBus configuration
builder.Services.AddLiteBus(config =>
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

// Validation (native integration)
builder.Services.AddValidatorsFromAssembly(typeof(CreatePizzaCommandValidator).Assembly);

// Pre/Post handlers (if needed)
builder.Services.AddTransient(typeof(ICommandPreHandler<>), typeof(CommandLoggingPreHandler<>));
builder.Services.AddTransient(typeof(ICommandPostHandler<>), typeof(CommandLoggingPostHandler<>));
```

---

## Execution Order

### Phase A: Preparation
1. ✅ Create all new folder structures (Commands/Handlers, Queries/Handlers, Validators)
2. ✅ Create backups of all files to be modified
3. ✅ Verify tools and scripts are ready

### Phase B: Automated Migration
1. ✅ Run PowerShell migration script (Migrate-MediatRToLiteBus.ps1)
   - Separates handlers from commands/queries
   - Updates all interfaces (IRequest → ICommand/IQuery, IRequestHandler → ICommandHandler/IQueryHandler)
   - Updates namespaces
   - Updates controller DI and calls
   - Creates reorganized validator files

2. ⏳ Manual verification (user performs)
   - Review generated report
   - Check file structure matches expected layout
   - Verify no files were missed

### Phase C: Compilation & Testing
1. ⏳ Build solution: `dotnet build`
2. ⏳ Fix compilation errors (one at a time):
   - Method signature issues
   - Missing type definitions
   - Import/namespace problems
3. ⏳ Update Program.cs using LITEBUS-STARTUP-TEMPLATE.cs
4. ⏳ Run tests: `dotnet test`
5. ⏳ Run application: `dotnet run`

### Phase D: Cleanup
1. ⏳ Uninstall MediatR NuGet packages
2. ⏳ Remove old behavior files if not converted
3. ⏳ Update documentation (AI guides, README)
4. ⏳ Final testing and verification

---

## Quick Reference: Find & Replace

### For use in Visual Studio Find & Replace (if manual):

| Find                                | Replace                                                    | Scope                         |
| ----------------------------------- | ---------------------------------------------------------- | ----------------------------- |
| `IRequest<`                         | `ICommand<`                                                | *.cs (Command files)          |
| `IRequest<`                         | `IQuery<`                                                  | *.cs (Query files)            |
| `IRequestHandler<`                  | `ICommandHandler<`                                         | *.cs (CommandHandler files)   |
| `IRequestHandler<`                  | `IQueryHandler<`                                           | *.cs (QueryHandler files)     |
| `public async Task<(\S+)> Handle\(` | `public async Task<$1> HandleAsync(`                       | *.cs (All handlers)           |
| `using MediatR;`                    | (delete)                                                   | *.cs (All files)              |
| `IMediator mediator`                | `ICommandMediator cmdMediator, IQueryMediator qryMediator` | *Controller.cs                |
| `mediator\.Send\(`                  | `cmdMediator.SendAsync(`                                   | *Controller.cs (for commands) |
| `mediator\.Send\(`                  | `qryMediator.QueryAsync(`                                  | *Controller.cs (for queries)  |

---

## Status Summary

| Task                      | Status | Files                        | Notes                                |
| ------------------------- | ------ | ---------------------------- | ------------------------------------ |
| Global mapping documented | ✅      | This file                    | All interface replacements defined   |
| Phase 9 analyzed          | ✅      | PHASE9-FILE-INVENTORY.md     | 14 handlers, 6 validators identified |
| AutomationScript created  | ✅      | Migrate-MediatRToLiteBus.ps1 | Full PowerShell automation           |
| Startup template created  | ✅      | LITEBUS-STARTUP-TEMPLATE.cs  | Complete DI setup examples           |
| Migration guide created   | ✅      | MIGRATION-LITEBUS-GUIDE.md   | Before/after patterns                |
| Ready for execution       | ✅      | All phases                   | User can run script and fix code     |

---

## Rollback Procedure

If migration needs to be rolled back:

```powershell
# Restore from backup created during migration
Copy-Item -Path "Backup-YYYYMMDD-HHMMSS/*" -Destination "." -Recurse -Force

# Or use git
git checkout HEAD -- .
```

---

**Ready to execute!** See MIGRATION-LITEBUS-GUIDE.md for step-by-step instructions.
