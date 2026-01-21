<img align="left" width="116" height="116" src="../Assets/pezza-logo.png" />

# &nbsp;**Pezza - Phase 3 - Step 1**

<br/><br/>

## Step 1: Dispatcher Scaffolding

**Difficulty**: ★★★☆☆ (Intermediate)  
**Estimated Time**: 1.5 - 2.5 hours  
**Prerequisites**:
- Completed Phase 2
- Familiar with Entity Framework Core
- Understand CQRS basics (commands vs queries)

### Learning Outcomes

- Build the lightweight dispatcher in `Common/CQRS/MediatorLite.cs`
- Configure Scrutor to auto-register command/query handlers
- Understand how generic constraints enable type-safe dispatch
- See how handlers stay unchanged from Phase 2

---

## What You Build

1. **Dispatcher** (`Common/CQRS/MediatorLite.cs`) with `Send` and `Query` methods
2. **DI Registration** (`Core/DependencyInjection.cs`) using Scrutor to scan handlers
3. **Command/Query Handlers** that implement `ICommandHandler<,>` and `IQueryHandler<,>`
4. **Controllers** that call `dispatcher.Send(command, ct)` instead of `[FromServices]` injection

### Key Files

- `Common/CQRS/MediatorLite.cs` — ~60 lines containing the entire pattern
- `Core/DependencyInjection.cs` — Scrutor registration
- `Core/Pizza/Commands/CreatePizzaCommand.cs` — Example command + handler
- `Api/Controllers/PizzaController.cs` — Using the dispatcher

Reference: [src/02. Step1](../src/02.%20Step1)

## Checklist

- [ ] `MediatorLite.cs` implements `Send` and `Query` with generic constraints
- [ ] Scrutor scans `ICommandHandler<,>` and `IQueryHandler<,>` interfaces
- [ ] Handlers implement interfaces and return `Result<T>`/`ListResult<T>`
- [ ] Controllers call `dispatcher.Send(command, ct)` instead of direct handler injection

---

## What You Learned

After Step 1, you understand:
- How the dispatcher uses generic constraints to resolve handlers
- Why Scrutor assembly scanning beats manual DI registration
- The difference between `ICommand<T>` and `IQuery<T>` marker interfaces
- How handlers stay unchanged from Phase 2—only the calling code changes

---

## Next Step

[Go to Step 2 - Testing](https://github.com/entelect-incubator/.NET/tree/master/Phase%203/Step%202)
