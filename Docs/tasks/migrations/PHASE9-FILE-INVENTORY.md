# Phase 9 - File Migration Inventory

**Analysis Date**: October 30, 2025  
**Status**: Code inventory complete from grep analysis  
**Total Requests/Handlers Found**: 18 MediatR patterns identified  

---

## Commands (Write Operations)

### Create Commands

| File                       | Current                           | Target                            | Handler                      | Status  |
| -------------------------- | --------------------------------- | --------------------------------- | ---------------------------- | ------- |
| `CreatePizzaCommand.cs`    | `IRequest<Result<PizzaModel>>`    | `ICommand<Result<PizzaModel>>`    | CreatePizzaCommandHandler    | MIGRATE |
| `CreateCustomerCommand.cs` | `IRequest<Result<CustomerModel>>` | `ICommand<Result<CustomerModel>>` | CreateCustomerCommandHandler | MIGRATE |

### Update Commands

| File                       | Current                           | Target                            | Handler                      | Status  |
| -------------------------- | --------------------------------- | --------------------------------- | ---------------------------- | ------- |
| `UpdatePizzaCommand.cs`    | `IRequest<Result<PizzaModel>>`    | `ICommand<Result<PizzaModel>>`    | UpdatePizzaCommandHandler    | MIGRATE |
| `UpdateCustomerCommand.cs` | `IRequest<Result<CustomerModel>>` | `ICommand<Result<CustomerModel>>` | UpdateCustomerCommandHandler | MIGRATE |
| `UpdateNotifyCommand.cs`   | `IRequest<Result>`                | `ICommand<Result>`                | UpdateNotifyCommandHandler   | MIGRATE |

### Delete Commands

| File                       | Current            | Target             | Handler                      | Status  |
| -------------------------- | ------------------ | ------------------ | ---------------------------- | ------- |
| `DeletePizzaCommand.cs`    | `IRequest<Result>` | `ICommand<Result>` | DeletePizzaCommandHandler    | MIGRATE |
| `DeleteCustomerCommand.cs` | `IRequest<Result>` | `ICommand<Result>` | DeleteCustomerCommandHandler | MIGRATE |

### Complex Commands

| File              | Current            | Target             | Handler             | Status  |
| ----------------- | ------------------ | ------------------ | ------------------- | ------- |
| `OrderCommand.cs` | `IRequest<Result>` | `ICommand<Result>` | OrderCommandHandler | MIGRATE |

---

## Queries (Read Operations)

### Pizza Queries

| File                | Current                            | Target                           | Handler               | Status  |
| ------------------- | ---------------------------------- | -------------------------------- | --------------------- | ------- |
| `GetPizzasQuery.cs` | `IRequest<ListResult<PizzaModel>>` | `IQuery<ListResult<PizzaModel>>` | GetPizzasQueryHandler | MIGRATE |
| `GetPizzaQuery.cs`  | `IRequest<Result<PizzaModel>>`     | `IQuery<Result<PizzaModel>>`     | GetPizzaQueryHandler  | MIGRATE |

### Customer Queries

| File                   | Current                               | Target                              | Handler                  | Status  |
| ---------------------- | ------------------------------------- | ----------------------------------- | ------------------------ | ------- |
| `GetCustomersQuery.cs` | `IRequest<ListResult<CustomerModel>>` | `IQuery<ListResult<CustomerModel>>` | GetCustomersQueryHandler | MIGRATE |
| `GetCustomerQuery.cs`  | `IRequest<Result<CustomerModel>>`     | `IQuery<Result<CustomerModel>>`     | GetCustomerQueryHandler  | MIGRATE |

### Order Queries

| File                | Current                            | Target                           | Handler               | Status  |
| ------------------- | ---------------------------------- | -------------------------------- | --------------------- | ------- |
| `GetOrdersQuery.cs` | `IRequest<ListResult<OrderModel>>` | `IQuery<ListResult<OrderModel>>` | GetOrdersQueryHandler | MIGRATE |

### Notify Queries

| File                  | Current                                        | Target                                       | Handler                 | Status  |
| --------------------- | ---------------------------------------------- | -------------------------------------------- | ----------------------- | ------- |
| `GetNotifiesQuery.cs` | `IRequest<ListResult<Common.Entities.Notify>>` | `IQuery<ListResult<Common.Entities.Notify>>` | GetNotifiesQueryHandler | MIGRATE |

---

## Validators/Behaviors

### Existing Validators

| File                                | Location               | Type              | Status |
| ----------------------------------- | ---------------------- | ----------------- | ------ |
| `CreatePizzaCommandValidator.cs`    | Core/Pizza/Commands    | AbstractValidator | EXISTS |
| `UpdatePizzaCommandValidator.cs`    | Core/Pizza/Commands    | AbstractValidator | EXISTS |
| `DeletePizzaCommandValidator.cs`    | Core/Pizza/Commands    | AbstractValidator | EXISTS |
| `CreateCustomerCommandValidator.cs` | Core/Customer/Commands | AbstractValidator | EXISTS |
| `UpdateCustomerCommandValidator.cs` | Core/Customer/Commands | AbstractValidator | EXISTS |
| `DeleteCustomerCommandValidator.cs` | Core/Customer/Commands | AbstractValidator | EXISTS |

### Behaviors to Migrate

| File                            | Location         | Type              | Status                                           |
| ------------------------------- | ---------------- | ----------------- | ------------------------------------------------ |
| `ValidationBehavior.cs`         | Common/Behaviour | IPipelineBehavior | REMOVE (LiteBus has built-in)                    |
| `PerformanceBehaviour.cs`       | Common/Behaviour | IPipelineBehavior | CONVERT → ICommandPreHandler/ICommandPostHandler |
| `ExceptionHandlerMiddleware.cs` | Common/Behaviour | Middleware        | KEEP (not CQRS-related)                          |

---

## Handler Locations & Refactoring

### Current Handler Pattern

```
Phase 9/src/01. StartSolution/Core/
├── Pizza/
│   ├── Commands/
│   │   ├── CreatePizzaCommand.cs (Handler nested in same file)
│   │   ├── UpdatePizzaCommand.cs (Handler nested in same file)
│   │   ├── DeletePizzaCommand.cs (Handler nested in same file)
│   │   ├── CreatePizzaCommandValidator.cs
│   │   ├── UpdatePizzaCommandValidator.cs
│   │   └── DeletePizzaCommandValidator.cs
│   └── Queries/
│       ├── GetPizzasQuery.cs (Handler nested in same file)
│       └── GetPizzaQuery.cs (Handler nested in same file)
├── Customer/
│   ├── Commands/
│   │   ├── CreateCustomerCommand.cs (Handler nested)
│   │   ├── UpdateCustomerCommand.cs (Handler nested)
│   │   ├── DeleteCustomerCommand.cs (Handler nested)
│   │   ├── CreateCustomerCommandValidator.cs
│   │   ├── UpdateCustomerCommandValidator.cs
│   │   └── DeleteCustomerCommandValidator.cs
│   └── Queries/
│       ├── GetCustomersQuery.cs (Handler nested)
│       └── GetCustomerQuery.cs (Handler nested)
├── Order/
│   ├── Commands/
│   │   └── OrderCommand.cs (Handler nested)
│   └── Queries/
│       └── GetOrdersQuery.cs (Handler nested)
├── Notify/
│   ├── Commands/
│   │   └── UpdateNotifyCommand.cs (Handler nested)
│   └── Queries/
│       └── GetNotifiesQuery.cs (Handler nested)
```

### Target Handler Pattern (LiteBus)

```
Phase 9/src/01. StartSolution/Core/
├── Pizza/
│   ├── Commands/
│   │   ├── CreatePizzaCommand.cs (ICommand only - handler SEPARATED)
│   │   ├── UpdatePizzaCommand.cs (ICommand only)
│   │   ├── DeletePizzaCommand.cs (ICommand only)
│   │   ├── Handlers/
│   │   │   ├── CreatePizzaCommandHandler.cs (ICommandHandler)
│   │   │   ├── UpdatePizzaCommandHandler.cs (ICommandHandler)
│   │   │   └── DeletePizzaCommandHandler.cs (ICommandHandler)
│   │   └── Validators/
│   │       ├── CreatePizzaCommandValidator.cs
│   │       ├── UpdatePizzaCommandValidator.cs
│   │       └── DeletePizzaCommandValidator.cs
│   └── Queries/
│       ├── GetPizzasQuery.cs (IQuery only)
│       ├── GetPizzaQuery.cs (IQuery only)
│       ├── Handlers/
│       │   ├── GetPizzasQueryHandler.cs (IQueryHandler)
│       │   └── GetPizzaQueryHandler.cs (IQueryHandler)
│       └── Validators/ (Optional)
│           ├── GetPizzasQueryValidator.cs
│           └── GetPizzaQueryValidator.cs
```

---

## Refactoring Tasks

### Phase 1: File Structure Creation
- [ ] Create `Commands/Handlers/` subfolder (Pizza)
- [ ] Create `Commands/Validators/` subfolder (Pizza)
- [ ] Create `Queries/Handlers/` subfolder (Pizza)
- [ ] Create `Queries/Validators/` subfolder (Pizza)
- [ ] Repeat for Customer, Order, Notify features

### Phase 2: Separate Commands from Handlers

**Action**: Extract handler code from command files into separate files

| Command File          | →   | Handler File                                   |
| --------------------- | --- | ---------------------------------------------- |
| CreatePizzaCommand.cs | →   | Commands/Handlers/CreatePizzaCommandHandler.cs |
| UpdatePizzaCommand.cs | →   | Commands/Handlers/UpdatePizzaCommandHandler.cs |
| DeletePizzaCommand.cs | →   | Commands/Handlers/DeletePizzaCommandHandler.cs |
| GetPizzasQuery.cs     | →   | Queries/Handlers/GetPizzasQueryHandler.cs      |
| GetPizzaQuery.cs      | →   | Queries/Handlers/GetPizzaQueryHandler.cs       |

**Result**: Each command/query file contains ONLY the interface, no handler.

### Phase 3: Update Interfaces

**Action**: Replace MediatR interfaces with LiteBus interfaces

**Commands**:
```csharp
// BEFORE
public class CreatePizzaCommand : IRequest<Result<PizzaModel>>

// AFTER  
public class CreatePizzaCommand : ICommand<Result<PizzaModel>>
```

**Queries**:
```csharp
// BEFORE
public class GetPizzasQuery : IRequest<ListResult<PizzaModel>>

// AFTER
public class GetPizzasQuery : IQuery<ListResult<PizzaModel>>
```

### Phase 4: Update Handlers

**Action**: Update handler files with new interfaces and namespace

**Commands**:
```csharp
// BEFORE
public class CreatePizzaCommandHandler(DatabaseContext databaseContext, IAppCache cache) 
    : IRequestHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> Handle(
        CreatePizzaCommand request, 
        CancellationToken cancellationToken)
    { }
}

// AFTER
namespace Core.Pizza.Commands.Handlers;

public class CreatePizzaCommandHandler(DatabaseContext databaseContext, IAppCache cache) 
    : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> HandleAsync(
        CreatePizzaCommand command, 
        CancellationToken cancellationToken = default)
    { }
}
```

**Queries**:
```csharp
// BEFORE
public class GetPizzasQueryHandler(DatabaseContext databaseContext, IAppCache cache)
    : IRequestHandler<GetPizzasQuery, ListResult<PizzaModel>>
{
    public async Task<ListResult<PizzaModel>> Handle(
        GetPizzasQuery request, 
        CancellationToken cancellationToken)
    { }
}

// AFTER
namespace Core.Pizza.Queries.Handlers;

public class GetPizzasQueryHandler(DatabaseContext databaseContext, IAppCache cache)
    : IQueryHandler<GetPizzasQuery, ListResult<PizzaModel>>
{
    public async Task<ListResult<PizzaModel>> HandleAsync(
        GetPizzasQuery query, 
        CancellationToken cancellationToken = default)
    { }
}
```

### Phase 5: Update Using Statements

**Remove**:
```csharp
using MediatR;
```

**Add**:
```csharp
using LiteBus.Commands.Abstractions;  // for ICommand, ICommandHandler
using LiteBus.Queries.Abstractions;   // for IQuery, IQueryHandler
```

### Phase 6: Migrate Behaviors to Handlers

**Current**: `Common/Behaviour/ValidationBehavior.cs` (IPipelineBehavior)  
**Target**: LiteBus native validator integration  
**Action**:
- Remove IPipelineBehavior registration
- LiteBus auto-integrates FluentValidation validators
- Validators already exist in Phase 9, just ensure they're named correctly

**Current**: `Common/Behaviour/PerformanceBehaviour.cs` (IPipelineBehavior for logging)  
**Target**: Create `ICommandPreHandler<T>` implementations if needed  
**Action**:
- Convert to `ICommandPreHandler<T>` for pre-processing
- Convert to `ICommandPostHandler<T>` for post-processing
- Or: Use LiteBus's built-in handler tags/priorities for similar functionality

---

## DI Configuration Changes

### Current (MediatR) - Program.cs

```csharp
services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CreatePizzaCommand).Assembly));
services.AddValidatorsFromAssembly(typeof(CreatePizzaCommandValidator).Assembly);
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
```

### Target (LiteBus) - Program.cs

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

services.AddValidatorsFromAssembly(typeof(CreatePizzaCommandValidator).Assembly);

// For performance logging - use ICommandPreHandler if needed
services.AddTransient(typeof(ICommandPreHandler<>), typeof(CommandPerformanceHandler<>));
```

---

## Controller Changes

### Current (MediatR)

```csharp
[ApiController]
public class PizzaController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePizzaModel model)
    {
        var result = await mediator.Send(new CreatePizzaCommand { Data = model });
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var result = await mediator.Send(new GetPizzasQuery { Data = new() });
    }
}
```

### Target (LiteBus)

```csharp
[ApiController]
public class PizzaController(ICommandMediator cmdMediator, IQueryMediator qryMediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePizzaModel model)
    {
        var result = await cmdMediator.SendAsync(new CreatePizzaCommand { Data = model });
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var result = await qryMediator.QueryAsync(new GetPizzasQuery { Data = new() });
    }
}
```

---

## Summary Statistics

| Item                  | Count           | Status            |
| --------------------- | --------------- | ----------------- |
| Commands              | 8               | MIGRATE           |
| Queries               | 6               | MIGRATE           |
| Handlers (nested)     | 14              | SEPARATE          |
| Validators (existing) | 6               | KEEP/REORG        |
| Behaviors to remove   | 2               | REMOVE/CONVERT    |
| Controllers to update | 4-6 (estimated) | UPDATE DI + CALLS |

**Total Files to Touch**: ~50+ files across Core, Api, Tests

---

## Next Steps

1. ✅ Phase 9 inventory complete
2. ⏭️ Create automated rename/refactor scripts
3. ⏭️ Generate global refactoring mapping (all phases)
4. ⏭️ Create LiteBus DI template
5. ⏭️ Update documentation
6. ⏭️ Execute bulk migration
