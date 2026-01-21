<img align="left" width="116" height="116" src="../Assets/pezza-logo.png" />

# **Pezza - Phase 2 - Step 3**

<br/><br/>

## Step 3: API Implementation

**Difficulty**: 3/5 (Intermediate)  
**Estimated Time**: 1.5 - 2 hours  
**Prerequisites**:

- Steps 1 and 2 complete
- Basic ASP.NET Core controllers
- Dependency injection fundamentals

### Learning Outcomes

- Keep controllers thin by calling handler interfaces directly
- Standardize responses with `ResponseHelper`
- Pass `CancellationToken` through every async endpoint
- Configure Swagger to verify endpoints quickly

---

## What You Build in this Step

1) `ResponseHelper` that converts `Result`, `Result<T>`, and `Result<List<T>>` into HTTP responses.  
2) Minimal `ApiController` base with shared attributes.  
3) Feature controllers (Pizza/Customer) that resolve handlers via `[FromServices]` and call `ExecuteAsync`.  
4) Swagger enabled for quick manual validation.

Reference implementation: [src/02. EndSolution/Api](../src/02.%20EndSolution/Api).

---

## 1) ResponseHelper

Located at [src/02. EndSolution/Api/Helpers/ResponseHelper.cs](../src/02.%20EndSolution/Api/Helpers/ResponseHelper.cs). It centralizes HTTP status decisions:

```csharp
public static ActionResult ResponseOutcome<T>(Result<T> result, ApiController controller)
    => result.Data is null
        ? controller.NotFound(Result.Failure($"{typeof(T).Name.Replace("Model", string.Empty)} not found"))
        : result.HasError
            ? controller.BadRequest(result)
            : controller.Ok(result);
```

Use the overloads for `Result<List<T>>` and non-generic `Result` too.

---

## 2) Base ApiController

Keep it lean: shared attributes only. Located at [src/02. EndSolution/Api/Controllers/ApiController.cs](../src/02.%20EndSolution/Api/Controllers/ApiController.cs).

```csharp
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
}
```

## 3) Feature Controllers (Handler Injection)

Example: Pizza controller in [src/02. EndSolution/Api/Controllers/PizzaController.cs](../src/02.%20EndSolution/Api/Controllers/PizzaController.cs):

```csharp
[ApiController]
[Route("[controller]")]
public sealed class PizzaController : ApiController
{
    [HttpGet("{id}")]
    public async Task<ActionResult> Get([FromServices] IGetPizzaQuery query, int id, CancellationToken cancellationToken)
        => ResponseHelper.ResponseOutcome(await query.ExecuteAsync(id, cancellationToken), this);

    [HttpPost("Search")]
    public async Task<ActionResult> Search([FromServices] IGetPizzasQuery query, CancellationToken cancellationToken)
        => ResponseHelper.ResponseOutcome(await query.ExecuteAsync(cancellationToken), this);

    [HttpPost]
    public async Task<ActionResult<Pizza>> Create([FromServices] ICreatePizzaCommand command, [FromBody] CreatePizzaModel model, CancellationToken cancellationToken)
        => ResponseHelper.ResponseOutcome(await command.ExecuteAsync(model, cancellationToken), this);

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromServices] IUpdatePizzaCommand command, int id, [FromBody] UpdatePizzaModel model, CancellationToken cancellationToken)
        => ResponseHelper.ResponseOutcome(await command.ExecuteAsync(id, model, cancellationToken), this);

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromServices] IDeletePizzaCommand command, int id, CancellationToken cancellationToken)
        => ResponseHelper.ResponseOutcome(await command.ExecuteAsync(id, cancellationToken), this);
}
```

Notes:

- Dependencies are explicit per action using `[FromServices]`.
- Every async endpoint accepts a `CancellationToken`.
- Controllers never contain business logic; they delegate to handlers and use `ResponseHelper` for envelopes.

---

## 4) Swagger / Debugging

Ensure Swagger is enabled in `Startup.ConfigureServices` and `Startup.Configure` (already present in EndSolution). Launch the API and verify endpoints at `/swagger`.

---

## Checklist

- [ ] `ResponseHelper` handles all `Result` shapes
- [ ] Base `ApiController` carries shared attributes only
- [ ] Controllers resolve handler interfaces via `[FromServices]`
- [ ] Endpoints pass `CancellationToken` to handlers
- [ ] Swagger loads and endpoints return consistent envelopes

---

## Phase Wrap-up

Phase 2 now has handler-based CQRS with clean controllers and standardized responses. Next, Phase 3 introduces the dispatcher/mediator to route commands and queries centrally without changing your handler implementations.

[Go to Phase 3](https://github.com/entelect-incubator/.NET/tree/master/Phase%203)
