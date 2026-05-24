# 🚀 LiteBus Migration - Quick Start Guide

**ONE PAGE** - Everything you need to get started

---

## What You Have

✅ **MIGRATION-LITEBUS-GUIDE.md** - Complete before/after reference  
✅ **PHASE9-FILE-INVENTORY.md** - Phase 9 analysis (8 commands, 6 queries, 14 handlers)  
✅ **REFACTORING-MAPPING.md** - Master mapping for all interfaces  
✅ **LITEBUS-STARTUP-TEMPLATE.cs** - Drop-in DI configuration  
✅ **Migrate-MediatRToLiteBus.ps1** - Fully automated PowerShell script  
✅ **MIGRATION-EXECUTIVE-SUMMARY.md** - Detailed overview  

---

## 60-Second Overview

**What**: Replace MediatR with LiteBus CQRS bus  
**Why**: MediatR now commercial; LiteBus is open-source & better design  
**How**: Automation + manual fixes (big-bang approach)  
**When**: 2-4 hours total  
**Impact**: ~680 files across all phases  

---

## The 4-Step Process

### Step 1: Backup & Preview (5 min)
```powershell
cd d:\Dev\Incubator\.NET
.\scripts\Migrate-MediatRToLiteBus.ps1 `
  -ProjectRoot "d:\Dev\Incubator\.NET\Phase 9" `
  -DryRun $true
```

### Step 2: Run Migration (10 min)
```powershell
.\scripts\Migrate-MediatRToLiteBus.ps1 `
  -ProjectRoot "d:\Dev\Incubator\.NET\Phase 9" `
  -DryRun $false
```

### Step 3: Fix Compilation Errors (1-2 hours)
```powershell
dotnet build              # See errors
# Fix error #1
# Fix error #2
# ...repeat
```

### Step 4: Update DI & Test (30 min)
```powershell
# Update Program.cs using LITEBUS-STARTUP-TEMPLATE.cs
dotnet test
dotnet run
```

---

## What Gets Changed

| What              | Before                        | After                                                  |
| ----------------- | ----------------------------- | ------------------------------------------------------ |
| Commands          | `IRequest<T>`                 | `ICommand<T>`                                          |
| Queries           | `IRequest<T>`                 | `IQuery<T>`                                            |
| Handlers          | Nested in files               | Separate in `Handlers/` folder                         |
| Handler interface | `IRequestHandler<T,R>`        | `ICommandHandler<T,R>` / `IQueryHandler<T,R>`          |
| Handler method    | `Handle()`                    | `HandleAsync()`                                        |
| Mediator DI       | `IMediator`                   | `ICommandMediator` + `IQueryMediator`                  |
| Mediator call     | `mediator.Send()`             | `cmdMediator.SendAsync()` / `qryMediator.QueryAsync()` |
| Validation        | `IPipelineBehavior` (generic) | `IValidator<T>` (specific)                             |

---

## Expected Errors & Fixes

| Error                 | Cause             | Fix                                        |
| --------------------- | ----------------- | ------------------------------------------ |
| `Handle()` not found  | Method renamed    | Rename to `HandleAsync()`                  |
| Parameter mismatch    | Renamed `request` | Use `command` or `query`                   |
| Type not found        | Handler separated | Update import to new namespace             |
| `IMediator` not found | Removed           | Use `ICommandMediator` or `IQueryMediator` |

---

## Key Files Generated

```
📍 Backups:
   └─ Backup-YYYYMMDD-HHMMSS/     (all originals backed up)

📍 New Structure:
   ├─ Core/Feature/Commands/Handlers/
   ├─ Core/Feature/Commands/Validators/
   ├─ Core/Feature/Queries/Handlers/
   └─ Core/Feature/Queries/Validators/

📍 Reports:
   └─ MIGRATION-REPORT.md           (what changed)
```

---

## Common Issues

**Script won't run?**
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

**Build errors after migration?**
- Look for remaining `using MediatR;`
- Search for `IMediator` (should be gone)
- Check all `Handle()` renamed to `HandleAsync()`

**Tests fail?**
- Update test mocks from `IMediator` → `ICommandMediator`/`IQueryMediator`
- Update handler registrations in test DI
- Fix any behavior registrations

---

## Phase 9 Summary (What You'll See)

- 8 commands (Create, Update, Delete for Pizza/Customer + Order + Notify)
- 6 queries (GetPizzas, GetPizza, GetCustomers, GetCustomer, GetOrders, GetNotifies)
- 14 handlers (will be separated into Commands/Handlers and Queries/Handlers)
- 6 validators (move to Commands/Validators)
- 4 controllers (update DI)
- 2 behaviors (ValidationBehavior removed, PerformanceBehaviour converted)

---

## Before/After Code Examples

### Command File
```csharp
// BEFORE
public class CreatePizzaCommand : IRequest<Result<PizzaModel>> { }
public class CreatePizzaCommandHandler : IRequestHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> Handle(CreatePizzaCommand request, CancellationToken ct) { }
}

// AFTER - Separate files
// CreatePizzaCommand.cs
public class CreatePizzaCommand : ICommand<Result<PizzaModel>> { }

// Commands/Handlers/CreatePizzaCommandHandler.cs
public class CreatePizzaCommandHandler : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> HandleAsync(CreatePizzaCommand command, CancellationToken ct = default) { }
}
```

### Controller
```csharp
// BEFORE
public class PizzaController(IMediator mediator)
{
    public async Task<IActionResult> Create([FromBody] CreatePizzaModel model)
    {
        var result = await mediator.Send(new CreatePizzaCommand { Data = model });
    }
}

// AFTER
public class PizzaController(ICommandMediator cmdMediator, IQueryMediator qryMediator)
{
    public async Task<IActionResult> Create([FromBody] CreatePizzaModel model)
    {
        var result = await cmdMediator.SendAsync(new CreatePizzaCommand { Data = model });
    }
}
```

---

## DI Setup (Program.cs)

```csharp
// Remove this:
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(...));

// Add this:
services.AddLiteBus(config =>
{
    config.AddCommandModule(builder => 
        builder.RegisterFromAssembly(typeof(CreatePizzaCommand).Assembly));
    config.AddQueryModule(builder => 
        builder.RegisterFromAssembly(typeof(GetPizzasQuery).Assembly));
});
services.AddValidatorsFromAssembly(typeof(CreatePizzaCommandValidator).Assembly);
```

See **LITEBUS-STARTUP-TEMPLATE.cs** for full template.

---

## Success Checklist

After migration completes:

- [ ] Build succeeds: `dotnet build`
- [ ] Tests pass: `dotnet test`
- [ ] App runs: `dotnet run`
- [ ] No `using MediatR;` statements
- [ ] No `IMediator` type references
- [ ] All commands use `ICommandMediator`
- [ ] All queries use `IQueryMediator`
- [ ] API endpoints respond correctly
- [ ] No unhandled exceptions

---

## Reference During Migration

1. **Interface questions?** → REFACTORING-MAPPING.md
2. **File structure questions?** → PHASE9-FILE-INVENTORY.md
3. **Code pattern examples?** → MIGRATION-LITEBUS-GUIDE.md
4. **DI setup?** → LITEBUS-STARTUP-TEMPLATE.cs
5. **Method signatures?** → Look at before/after in MIGRATION-LITEBUS-GUIDE.md

---

## Ready? Start Here

```powershell
# Test the migration first (DRY RUN)
cd d:\Dev\Incubator\.NET
.\scripts\Migrate-MediatRToLiteBus.ps1 -ProjectRoot "d:\Dev\Incubator\.NET\Phase 9" -DryRun $true

# Review output - should show what WILL change
# If looks good, run for real:

.\scripts\Migrate-MediatRToLiteBus.ps1 -ProjectRoot "d:\Dev\Incubator\.NET\Phase 9" -DryRun $false

# Then: dotnet build, fix errors, update Program.cs, test
```

---

## Timeline Summary

| Step           | Time          | What                       |
| -------------- | ------------- | -------------------------- |
| 1. DRY RUN     | 5 min         | Preview changes            |
| 2. EXECUTE     | 10 min        | Run migration script       |
| 3. BUILD & FIX | 30 min - 2 hr | Fix compilation errors     |
| 4. DI & TEST   | 30 min        | Update Program.cs and test |
| **TOTAL**      | **2-4 hours** | Complete migration         |

---

## After Migration - Next Steps

1. ✅ Commit to git
2. ✅ Update AI documentation (COPILOT-INSTRUCTIONS.md, DEVELOP_WITH_AI.md)
3. ✅ Remove MediatR NuGet packages
4. ✅ Full regression testing
5. ✅ Deploy to production

---

## 💡 Pro Tips

1. **Use VS Find & Replace** if script misses anything:
   - Find: `IRequest<` Replace: `ICommand<` (for commands)
   - Find: `IRequest<` Replace: `IQuery<` (for queries)
   - Find: `Handle(` Replace: `HandleAsync(` (in handlers)

2. **Commit progress**: After each major error fix, commit changes

3. **Keep backup**: The script auto-backs up everything, but also use git

4. **Document issues**: If you find script missed something, note it for Phase 2-8 migrations

5. **Run one phase at a time**: Start with Phase 9, then apply to Phase 8, 7, 6, etc.

---

## Questions?

- **"Why separate handlers?"** → Better file organization, clearer separation of concerns
- **"Why ICommandMediator vs IMediator?"** → Explicit CQRS pattern, better type safety
- **"Can I rollback?"** → Yes! Backups are created automatically
- **"Do I need to update tests?"** → Yes, but similarly (IMediator → ICommandMediator/IQueryMediator)

---

**You're all set! Run the migration script and follow the 4 steps above.** 🚀

---

Generated: October 30, 2025  
Version: 1.0  
Status: ✅ Ready for Execution
