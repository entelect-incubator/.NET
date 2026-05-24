<img align="left" width="116" height="116" src="../Assets/pezza-logo.png" />

# **Pezza - Phase 2 - Step 1**

<br/><br/>

## Step 1: Scaffolding (Handlers + Data)

**Difficulty**: 3/5 (Intermediate)  
**Estimated Time**: 1.5 - 2.5 hours  
**Prerequisites**:

- Completed the Phase 2 main README
- Familiarity with Entity Framework Core
- Basic CQRS concepts (commands vs queries)

### Learning Outcomes

- Model the domain with entities and action-specific models
- Map between entities and models with clear mappers
- Configure EF Core (including new Customer entity)
- Create command/query handler interfaces with `ExecuteAsync`
- Register handlers automatically with Scrutor (no MediatR yet)

---

## What You Build in this Step

1) Domain entities and EF Core maps for Pizza and Customer.  
2) Action-specific models and mappers for both entities.  
3) Result/Result<T>/ListResult<T> for consistent outcomes (already in Common).  
4) Command and query handler interfaces plus implementations for Pizza/Customer.  
5) Dependency injection that scans and registers all handlers.

Refer to the completed version in [src/02. EndSolution](../src/02.%20EndSolution) if you get stuck.

---

## 1) Entities + DbContext

- Add `Customer` alongside `Pizza` in [src/02. EndSolution/Common/Entities](../src/02.%20EndSolution/Common/Entities).  
- Add the EF maps `CustomerMap` and `PizzaMap` in [src/02. EndSolution/DataAccess/Mapping](../src/02.%20EndSolution/DataAccess/Mapping).  
- Update `DatabaseContext` to expose `DbSet<Customer>` and `DbSet<Pizza>` and apply both maps. Use the in-memory provider for this phase.

Key rules:

- Required properties use `required` (e.g., `Name`).
- Nullable references are marked with `?`.
- Keep constructors lean; EF handles instantiation.

---

## 2) Models + Mappers

- Create action-focused models in [src/02. EndSolution/Common/Models](../src/02.%20EndSolution/Common/Models) (e.g., `CreatePizzaModel`, `UpdatePizzaModel`, `PizzaModel`).
- Update mappers in [src/02. EndSolution/Common/Mappers](../src/02.%20EndSolution/Common/Mappers) to convert between entities and models.
- Ensure list helpers exist (`IEnumerable<Pizza>.Map()` and `IEnumerable<Customer>.Map()`).

Mapper tips:

- Keep mapping deterministic and null-safe.
- Do not duplicate mapping logic in handlers—use the extensions instead.

---

## 3) Result Pattern

`Result`, `Result<T>`, and `ListResult<T>` already live in [src/02. EndSolution/Common/Models/Results](../src/02.%20EndSolution/Common/Models/Results). Use them for every handler return to keep responses consistent and exception-free.

Success cases:

```csharp
return Result<PizzaModel>.Success(entity.Map());
```

Failure cases:

```csharp
return Result<PizzaModel>.Failure("Not found");
```

---

## 4) Command + Query Handlers (Interface-First)

Phase 2 keeps dependencies explicit: controllers will resolve the handlers directly, without a mediator yet.

Example: Create Pizza command

```csharp
// Interface
public interface ICreatePizzaCommand
{
    Task<Result<PizzaModel>> ExecuteAsync(CreatePizzaModel model, CancellationToken cancellationToken = default);
}

// Implementation
public sealed class CreatePizzaCommand(DatabaseContext databaseContext) : ICreatePizzaCommand
{
    public async Task<Result<PizzaModel>> ExecuteAsync(CreatePizzaModel model, CancellationToken cancellationToken = default)
    {
        var entity = new Pizza
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            DateCreated = DateTime.UtcNow
        };

        databaseContext.Pizzas.Add(entity);
        var saved = await databaseContext.SaveChangesAsync(cancellationToken);
        return saved > 0
            ? Result<PizzaModel>.Success(entity.Map())
            : Result<PizzaModel>.Failure("Error saving pizza");
    }
}
```

Do the same for queries (`IGetPizzaQuery`, `IGetPizzasQuery`, `IGetCustomerQuery`, etc.) using `ExecuteAsync` signatures that accept `CancellationToken`.

---

## 5) Dependency Injection (Scrutor)

Register handlers automatically in [src/02. EndSolution/Core/DependencyInjection.cs](../src/02.%20EndSolution/Core/DependencyInjection.cs):

```csharp
services.Scan(scan => scan
    .FromAssemblyOf<ICreatePizzaCommand>()
    .AddClasses(c => c.InNamespaces("Core.Pizza.Commands", "Core.Customer.Commands"))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

services.Scan(scan => scan
    .FromAssemblyOf<IGetPizzaQuery>()
    .AddClasses(c => c.InNamespaces("Core.Pizza.Queries", "Core.Customer.Queries"))
    .AsImplementedInterfaces()
    .WithScopedLifetime());
```

This keeps controller signatures clean and prepares the code for the dispatcher that arrives in Phase 3.

---

## Checklist

- [ ] Entities: `Customer` added; DbContext updated with maps for both entities
- [ ] Models and mappers created/updated for Pizza and Customer
- [ ] Result pattern used by all handlers (no exceptions for flow control)
- [ ] Handlers expose `ExecuteAsync(..., CancellationToken)`
- [ ] Scrutor DI scans command/query namespaces and registers interfaces
- [ ] Start solution builds: `dotnet build "Phase 2/src/01. StartSolution/Pezza.slnx"`

---

## Next Step

Proceed to [Step 2 - Unit Tests](https://github.com/entelect-incubator/.NET/tree/master/Phase%202/Step%202) to cover these handlers with in-memory EF tests.
