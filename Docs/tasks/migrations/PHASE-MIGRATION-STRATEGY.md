# Multi-Phase Migration Strategy & Execution Guide

**Status**: Phase 9 Complete ✅ | Phases 1-8 Ready for Migration  
**Date**: October 30, 2025

## Overview

Due to the complexity of multi-phase migrations with different project structures (Pezza.* naming in phases 1-8 vs generic naming in Phase 9), I recommend a **strategic** approach:

### Phase Architecture Overview

```
Phase 9 (Complete):
  src/01. StartSolution/
    ├── Api/
    ├── Core/
    ├── DataAccess/
    ├── Common/
    └── Scheduler/

Phases 1-8 (Require Migration):
  src/01. StartSolution/
    ├── Pezza.Api/
    ├── Pezza.Core/
    ├── Pezza.DataAccess/
    ├── Pezza.Common/
    ├── Pezza.Scheduler/
    ├── Pezza.BackEnd/
    ├── Pezza.Test/
    └── Pezza.sln
```

## Migration Strategy

### Option A: Automated Batch (Fastest)
**Time**: ~30 minutes | **Risk**: Medium  
- Use global find-replace across all phases
- Run builds to identify errors
- Apply fixes manually per phase

### Option B: Manual Phase-by-Phase (Safest)
**Time**: 4-6 hours | **Risk**: Low  
- Migrate each phase individually
- Test each phase before moving to next
- Catch phase-specific issues early

### Option C: Hybrid (Recommended)
**Time**: 2-3 hours | **Risk**: Low-Medium  
- Use automated scripts for common patterns
- Manual intervention for phase-specific code
- Build and verify each phase

---

## Automated Migration Checklist for All Phases

### For Each Phase (8, 7, 6, 5, 4, 3, 2, 1):

#### Step 1: Pre-Migration
```powershell
# 1a. Navigate to phase
cd "d:\Dev\Incubator\.NET\Phase X\src\01. StartSolution"

# 1b. Check MediatR usage
Get-ChildItem -Filter "*.cs" -Recurse | Select-String "using MediatR" | Measure-Object

# 1c. Backup solution
Copy-Item . "../BackupPhaseX-$(Get-Date -Format 'yyyyMMdd')" -Recurse
```

#### Step 2: Global String Replacements
```
MediatR → LiteBus Mappings:
- using MediatR; → using LiteBus.Commands.Abstractions; using LiteBus.Queries.Abstractions;
- IRequest<T> (in Commands) → ICommand<T>
- IRequest<T> (in Queries) → IQuery<T>
- IRequestHandler<Req,Res> (in Commands) → ICommandHandler<Req,Res>
- IRequestHandler<Req,Res> (in Queries) → IQueryHandler<Req,Res>
- Handle() → HandleAsync()
- IMediator → ICommandMediator + IQueryMediator
```

#### Step 3: DI Configuration Updates
**File**: `Pezza.Core/DependencyInjection.cs`
```csharp
// OLD
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

// NEW
// Manual handler registration
var handlers = typeof(DependencyInjection).Assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && 
        (t.GetInterfaces().Any(i => i.IsGenericType && 
            (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
             i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))))
    .ToList();

foreach (var handler in handlers) {
    var interfaces = handler.GetInterfaces()
        .Where(i => i.IsGenericType && 
            (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
             i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
        .ToList();
    
    foreach (var iface in interfaces) {
        services.AddTransient(iface, handler);
    }
}

services.AddScoped<ICommandMediator, CommandMediator>();
services.AddScoped<IQueryMediator, QueryMediator>();
```

#### Step 4: GlobalUsings Updates
**File**: All project `GlobalUsings.cs`
```
ADD:
- global using LiteBus.Commands.Abstractions;
- global using LiteBus.Queries.Abstractions;
```

#### Step 5: Controller Updates
For all `*Controller.cs` files:
```csharp
// OLD
public class PizzaController(IMediator mediator)

// NEW
public class PizzaController(ICommandMediator cmdMediator, IQueryMediator qryMediator)

// OLD
var result = await mediator.Send(command);
var result = await mediator.Send(query);

// NEW
var result = await cmdMediator.SendAsync(command);
var result = await qryMediator.SendAsync(query, CancellationToken.None);
```

#### Step 6: Build & Verify
```powershell
dotnet build 2>&1 | Select-String "error CS"
# Should return: (no results = success)
```

---

## Post-Migration Tasks

### Documentation Migration (After all phases complete)

1. **Move Migration Files**
```
OLD: d:\Dev\Incubator\.NET\MIGRATION-*.md
NEW: d:\Dev\Incubator\.NET\docs\tasks\migrations\MIGRATION-*.md
```

2. **Update Code Snippets**
Replace all MediatR patterns with LiteBus in:
- `/docs/tasks/TASK-ORGANIZATION-GUIDE.md`
- `/docs/development/README.md`
- All `.md` files in `/docs`

3. **Create Master Report**
File: `d:\Dev\Incubator\.NET\PHASE-MIGRATION-COMPLETE.md`

---

## Critical Files to Review per Phase

- `GlobalUsings.cs` in each project
- `DependencyInjection.cs` or `ServiceConfiguration.cs`
- All `*Controller.cs` files
- `Program.cs` or `Startup.cs`
- Test projects setup

---

## Recommended Execution Order

**Rationale**: Start with simpler phases, build up to complex ones

1. **Phase 3** (Simplest) - Test workflow
2. **Phase 4**
3. **Phase 5**
4. **Phase 6**
5. **Phase 7**
6. **Phase 8** (Most complex - has BackEnd, Portal projects)
7. **Phase 2**
8. **Phase 1**

---

## Risk Mitigation

- ✅ Backup each phase before migration
- ✅ Build after each phase
- ✅ Run unit tests if available
- ✅ Keep Phase 9 as reference
- ✅ Use version control commits

---

## Next Steps

1. Review this strategy
2. Choose execution option (A, B, or C)
3. Execute migrations phase by phase
4. Update documentation
5. Final testing and commit

**Estimated Total Time**: 2-6 hours depending on option chosen
