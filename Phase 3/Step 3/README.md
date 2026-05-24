<img align="left" width="116" height="116" src="../Assets/pezza-logo.png" />

# &nbsp;**Pezza - Phase 3 - Step 3**

<br/><br/>

## Step 3: API Implementation

**Difficulty**: ★★★☆☆ (Intermediate)  
**Estimated Time**: 1.5 - 2 hours  
**Prerequisites**:

- Completed Steps 1 and 2
- Understanding of REST API controllers
- Familiarity with dependency injection

### Learning Outcomes

- Wire controllers to the custom dispatcher
- Use `ResponseHelper` for consistent HTTP responses
- Return `Result<T>` and `ListResult<T>` from endpoints
- Keep controllers thin and handler-focused

---

## What You Build

1) **ResponseHelper** (`Api/Helpers/ResponseHelper.cs`) centralizes HTTP response logic
2) **Base ApiController** exposes `Dispatcher` from DI
3) **Feature controllers** call `dispatcher.Send` and `dispatcher.Query`
4) **Swagger** for quick endpoint verification

### Controller Pattern

```csharp
[HttpPost]
public async Task<ActionResult> Create(
    CreatePizzaCommand command,
    CancellationToken ct)
{
    var result = await dispatcher.Send(command, ct);
    return ResponseHelper.ResponseOutcome(result, this);
}
```

### Base Controller

- `Api/Controllers/ApiController.cs` exposes `Dispatcher` from `HttpContext.RequestServices`
- No direct knowledge of handlers inside controllers

### DI Configuration

- `Core/DependencyInjection.cs` registers `Dispatcher` and scans handlers via Scrutor
- `Api/Startup.cs` calls `services.AddApplication();`

---

## Checklist

- [ ] Base ApiController exposes `Dispatcher` from DI
- [ ] Controllers call `dispatcher.Send(command, ct)` 
- [ ] ResponseHelper returns consistent ActionResult envelopes
- [ ] Swagger runs and endpoints work

---

## What You Learned

After Step 3, you understand:

- How controllers become thinner by delegating to the dispatcher
- Why centralized `ResponseHelper` beats duplicated status code logic
- The trade-off: slightly more abstraction for cleaner controllers
- How the dispatcher resolves handlers at runtime via generic constraints

---

## 🎉 Phase 3 Complete!

You've built a custom dispatcher from scratch and understand:

- **Why** mediator patterns exist (decoupling, extensibility)
- **How** generic constraints enable type-safe routing
- **When** to use a dispatcher vs direct injection

**Next:** Phase 4 adds validation behaviors, transaction handling, and demonstrates extending the dispatcher with decorators.

## Next Step

[Go to Phase 4](https://github.com/entelect-incubator/.NET/tree/master/Phase%204)
