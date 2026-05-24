# AI Prompting Examples — Real Scenarios

Complete, copy-paste-ready prompts for common development scenarios in the Pezza project.

---

## Scenario 1: Create a New Query Handler

**Situation:** You need to add a search feature for customers.

```
I'm building a GetCustomerSearchQuery handler for the Pezza project.

CONTEXT:
- .NET 10 with CQRS pattern using LiteBus
- DatabaseContext for EF Core (in-memory)
- IAppCache for 12-hour caching
- Models use .Map() extension methods
- Primary constructors for DI
- Result<T> and ListResult<T> patterns

REQUEST:
Generate GetCustomerSearchQueryHandler implementing IQueryHandler<GetCustomerSearchQuery, ListResult<CustomerModel>> with:

1. Primary constructor: DatabaseContext databaseContext, IAppCache cache
2. HandleAsync method with CancellationToken
3. Null check: if (request.Data == null) return error
4. Cache lookup with key "Customers" and 12-hour expiry
5. Filter methods:
   - FilterByName (case-insensitive substring)
   - FilterByEmail (case-insensitive substring)
   - FilterByCity (exact match)
6. Apply filters in order: Name → Email → City
7. Order by DateCreated ascending
8. Return ListResult<CustomerModel>.Success(data, total)
9. Map using .Map() extension

STYLE:
- No underscore-prefixed properties
- Async all the way
- Guard clauses for null
- Fluent filtering chain
```

**Output:** Copy the handler code from AI, review against checklist, paste into project.

---

## Scenario 2: Create Command Handler with Validation

**Situation:** Need to create a new pizza with validation and cache invalidation.

```
Generate CreatePizzaCommandHandler for the Pezza project.

CONTEXT:
- Command: CreatePizzaCommand with CreatePizzaModel? Data property
- Models: Pizza entity, PizzaModel DTO
- Pattern: Result<T>, primary constructors
- Cache: IAppCache with key "Pizzas"
- Validation: Check non-null, check Name is not empty

REQUEST:
Generate ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>:

1. Primary constructor: DatabaseContext databaseContext, IAppCache cache
2. Async Task<Result<PizzaModel>> HandleAsync(CreatePizzaCommand request, CancellationToken ct)
3. Validation:
   - If request.Data is null: return Result<PizzaModel>.Failure("Data required")
   - If string.IsNullOrEmpty(request.Data.Name): return Result<PizzaModel>.Failure("Name required")
4. Create entity:
   - new Pizza { Name, Description, Price, DateCreated = DateTime.UtcNow }
5. Add to context and save:
   - databaseContext.Pizzas.Add(entity)
   - var rows = await databaseContext.SaveChangesAsync(ct)
6. Cache invalidation:
   - cache.Remove("Pizzas")
7. Return:
   - if (rows > 0): Result<PizzaModel>.Success(entity.Map())
   - else: Result<PizzaModel>.Failure("Save failed")
8. Mapping: Use entity.Map() extension

CONSTRAINTS:
- No .Result or .Wait()
- CancellationToken for all async
- Early returns for validation
- No try-catch (let exceptions bubble)
```

---

## Scenario 3: Add Extension Methods for Filtering

**Situation:** Multiple queries need the same filtering logic.

```
Generate extension methods for PizzaModel filtering in namespace Common.Extensions.

REQUIREMENTS:
Create static class PizzaExtensions with methods:

1. FilterByName(this IEnumerable<PizzaModel> source, string? name)
   - If name is null/empty, return source unchanged
   - Otherwise: Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase))

2. FilterByPrice(this IEnumerable<PizzaModel> source, decimal? minPrice, decimal? maxPrice)
   - If both null, return source
   - If min: >= minPrice
   - If max: <= maxPrice
   - If both: min <= Price <= max

3. FilterByDescription(this IEnumerable<PizzaModel> source, string? keyword)
   - Case-insensitive substring match
   - Handle null/empty

4. SortByPopularity(this IEnumerable<PizzaModel> source)
   - Order by DateCreated descending

STYLE:
- Chainable (return IEnumerable<PizzaModel>)
- Guard against null input parameters
- Use StringComparison.OrdinalIgnoreCase
- Clean, readable names
```

**Expected Output:** Extension methods that can be used in queries like:
```csharp
cachedData
    .FilterByName(searchTerm)
    .FilterByPrice(minPrice, maxPrice)
    .FilterByDescription(keyword)
    .SortByPopularity()
    .ToList()
```

---

## Scenario 4: Update Mapping Extension

**Situation:** New properties added to entity, need to update mapping.

```
Update the Pizza mapping extension in Common.Mappings.

CONTEXT:
Entity: Pizza (Id, Name, Description, Price, DateCreated, IsAvailable, ImageUrl)
Model: PizzaModel (Id, Name, Description, Price, DateCreated, IsAvailable, ImageUrl)

REQUEST:
1. Update single entity mapper: Pizza.Map() -> PizzaModel
   - Map all properties 1:1
   - Handle null Description/ImageUrl (nullable)

2. Update collection mapper: IEnumerable<Pizza>.Map() -> IEnumerable<PizzaModel>
   - Use Select(x => x.Map())

3. Add reverse mapper if needed: PizzaModel.MapToEntity() -> Pizza

CONSTRAINTS:
- No manual foreach loops
- Use LINQ Select for collections
- Return IEnumerable<T> (lazy evaluation)
- Add XML doc comments
```

---

## Scenario 5: Create API Controller Endpoint

**Situation:** Add new endpoint to handle pizza search.

```
Generate SearchPizzaController endpoint for the Pezza API.

CONTEXT:
- Inherits ApiController (provides this.CmdMediator and this.QryMediator)
- Query: GetPizzasQuery : IQuery<ListResult<PizzaModel>>
- Request model: SearchPizzaModel (Name, MinPrice, MaxPrice, etc.)
- Response: ListResult<PizzaModel>
- Helper: ResponseHelper.ResponseOutcome(result, this)

REQUEST:
1. [HttpPost("Search")] endpoint
2. Parameters: [FromBody] SearchPizzaModel data
3. Send GetPizzasQuery { Data = data } via QryMediator.QueryAsync()
4. Return ResponseHelper.ResponseOutcome(result, this)
5. Include:
   - [ProducesResponseType(200)] for success
   - [ProducesResponseType(400)] for bad request
   - XML doc comments explaining parameters

CONSTRAINTS:
- No business logic in controller
- Await QryMediator.QueryAsync(query)
- Use appropriate HTTP methods (POST for search with body)
```

---

## Scenario 6: Create Unit Test for Handler

**Situation:** Test CreatePizzaCommandHandler with multiple scenarios.

```
Generate xUnit test class for CreatePizzaCommandHandler.

CONTEXT:
- xUnit test framework
- Moq for mocking
- DatabaseContext and IAppCache need mocks
- SaveChangesAsync should return > 0 for success

REQUEST:
Create TestCreatePizzaCommandHandler with tests:

1. Handle_WithValidData_ReturnSuccess
   - Arrange: valid CreatePizzaCommand with all properties
   - Act: await handler.Handle(command, CancellationToken.None)
   - Assert: result.Success == true, result.Data != null, result.Data.Name == input.Name

2. Handle_WithNullData_ReturnFailure
   - Command with Data = null
   - Assert: result.Success == false, result.Message contains "Data"

3. Handle_WithEmptyName_ReturnFailure
   - Command with Data.Name = ""
   - Assert: result.Success == false

4. Handle_WithValidData_InvalidateCache
   - Mock IAppCache
   - Assert: cache.Remove() called once with correct key

5. Handle_WhenSaveFails_ReturnFailure
   - Mock SaveChangesAsync to return 0
   - Assert: result.Success == false

STYLE:
- Arrange-Act-Assert pattern
- Descriptive test names
- Use Mock.It.IsAny<>() for flexible assertions
- Mock only what's needed
- No sleep/delays
```

---

## Scenario 7: Ask for Code Review

**Situation:** AI generated some code, you want a second opinion.

```
Please review this CreatePizzaCommandHandler for issues:

[PASTE CODE HERE]

CHECKLIST:
- Primary constructor used correctly?
- CancellationToken passed to all async methods?
- Null checks present and correct?
- Result<T> pattern correctly applied?
- Cache invalidation included?
- Follows naming conventions (no _underscores)?
- Any performance issues?
- Any missing null reference guards?
- Is this following CQRS correctly?

SPECIFIC CONCERNS:
- [mention any worries, e.g., "Is the cache key correct?"]
```

---

## Scenario 8: Refactor for DRY

**Situation:** Notice similar code across multiple handlers.

```
I notice filtering logic is repeated in GetPizzasQuery, GetCustomersQuery, and GetOrdersQuery.

Common pattern:
- Cache lookup
- Filter by name
- Filter by status/category
- Order by date
- Return ListResult

REQUEST:
1. Extract common behavior into validators or shared utilities
   - For caching: Create QueryCacheAttribute
   - For filtering: Create FilterExtension<T> methods
   - For ordering: Create OrderExtension<T> methods

2. Update each handler to use validators and extensions
3. Keep handlers focused on business logic

CONSTRAINTS:
- Maintain current Result<T> pattern
- Don't break existing code
- Use LiteBus IValidator<T> for validation where needed
```

---

## Scenario 9: Add Logging for Handlers

**Situation:** Need to add logging without cluttering handler logic.

```
Create logging utilities for LiteBus handlers in namespace Common.Utilities.

REQUEST:
1. Create LoggingExtensions class with ILogger helper methods
2. Inject ILogger<HandlerName> into handlers
3. Log on entry:
   - Handler type name
   - Request data (JSON serialized)
4. Log on exit:
   - Response type
   - Success/failure status
   - Duration (use Stopwatch)
5. Log on exception (rethrow)
   - Exception message and stack trace

STYLE:
- Use ILogger.LogInformation, LogError, LogDebug
- Measure duration with System.Diagnostics.Stopwatch
- Don't catch exceptions (let them propagate)
```

---

## Scenario 10: Generate Migration Instructions

**Situation:** Need to upgrade existing code to new standards.

```
Generate step-by-step instructions to modernize GetPizzasQueryHandler.

CURRENT STATE:
[PASTE OLD CODE]

TARGET STATE:
- Primary constructor
- No private fields
- Proper CancellationToken usage
- Improved null handling
- DRY filtering with extensions

REQUEST:
1. Step-by-step refactoring guide
2. Before/after code snippets
3. Testing strategy (how to verify changes work)
4. Risk assessment (what could break)
5. Rollback plan if needed
```

---

## Pro Tips for AI Prompting

### 1. Start with Context
Always tell AI about:
- Project structure
- Existing patterns
- .NET version
- Frameworks (LiteBus, EF Core, etc.)

### 2. Be Specific About Constraints
Say what you DON'T want:
- "No .Result or .Wait()"
- "No underscore prefixes"
- "Primary constructor only"

### 3. Ask for Code Review
Before using generated code:
```
Review this for:
- Async/await correctness
- Null safety
- DRY violations
- Pattern adherence
```

### 4. Iterate
```
AI: [generates code]
YOU: "This is good, but add X and fix Y"
AI: [refactors]
YOU: "Perfect, now add unit tests"
AI: [adds tests]
```

### 5. Test the Output
```
"Generate the most comprehensive test case for this handler:
- Test every code path
- Test error conditions
- Test cache behavior
- Use Moq for mocks"
```

### 6. Ask for Alternatives
```
"Generate 3 alternative approaches to this problem:
1. Current approach
2. Using [pattern]
3. Using [pattern]

Explain pros/cons of each"
```

---

## Summary Workflow

1. **Clarify requirements** (what, why, constraints)
2. **Provide context** (architecture, patterns, frameworks)
3. **Ask specifically** (be concrete, not vague)
4. **Review output** (checklist from Quick Reference)
5. **Refactor together** (iterate with AI)
6. **Test thoroughly** (unit tests, integration tests)
7. **Document** (XML comments, README updates)
8. **Commit** (meaningful commit messages referencing AI assistance)
