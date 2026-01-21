<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 7 - Step 1** [![.NET - Phase 7 - Step 1](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step1.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase7-step1.yml)

<br/><br/>

## In-Memory Caching with IMemoryCache

**Difficulty**: ★★★☆☆ (Intermediate)  
**Estimated Time**: 2-3 hours  
**Prerequisites**:

- Completed Phase 5 (understand dispatcher and error handling)
- Basic understanding of cache patterns

### Learning Outcomes

After completing this step, you will:

- Implement Cache-Aside pattern in query handlers
- Use `IMemoryCache` with TTL (time-to-live) strategies
- Understand cache invalidation in command handlers
- Apply distributed cache concepts (preparation for Step 2)
- Measure performance impact of caching

## Why Cache Queries?

Queries that hit the database every request waste resources:

- **Database load**: Same query executed thousands of times for identical data
- **Latency**: Network I/O to database adds milliseconds
- **Bandwidth**: Transferring same large datasets repeatedly

**Solution**: Store frequently-accessed read-only data in memory with TTL. Next identical request returns cached result in microseconds.

## How to Implement

### Step 1: Register IMemoryCache

In `Api/Startup.cs` or `Program.cs`:

```csharp
// Add to ConfigureServices()
services.AddMemoryCache();
```

### Step 2: Create Cache Keys Constant

In `Common/Data.cs`:

```csharp
namespace Common;

public static class Data
{
    public const string PizzasCacheKey = "pizzas_all";
    public const string PizzaCacheKey = "pizza_{0}";  // Use string.Format for specific ID
    public const string CustomersCacheKey = "customers_all";
}
```

### Step 3: Add Caching to Query Handler

Inject `IMemoryCache` and implement cache-aside pattern:

```csharp
namespace Core.Pizza.Queries;

using Microsoft.Extensions.Caching.Memory;

public sealed class GetPizzasQueryHandler(
    DatabaseContext databaseContext,
    IMemoryCache cache) : IQueryHandler<GetPizzasQuery, Result<IEnumerable<PizzaModel>>>
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    public async Task<Result<IEnumerable<PizzaModel>>> Handle(
        GetPizzasQuery request,
        CancellationToken cancellationToken)
    {
        const string cacheKey = Common.Data.PizzasCacheKey;
        
        // Try to get from cache
        if (cache.TryGetValue(cacheKey, out IEnumerable<PizzaModel>? cached))
            return Result<IEnumerable<PizzaModel>>.Success(cached);

        // Cache miss: query database
        var entities = await databaseContext.Pizzas
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        var mapped = entities.Map();
        
        // Cache the result for 10 minutes
        cache.Set(cacheKey, mapped, CacheDuration);
        
        return Result<IEnumerable<PizzaModel>>.Success(mapped);
    }
}
```

### Step 4: Apply Filters on Cached Data

Filters can now work on cached collections without hitting the database:

```csharp
// Extend the handler to support filtering on cache
public async Task<Result<IEnumerable<PizzaModel>>> Handle(
    GetPizzasQuery request,
    CancellationToken cancellationToken)
{
    const string cacheKey = Common.Data.PizzasCacheKey;
    
    if (cache.TryGetValue(cacheKey, out IEnumerable<PizzaModel>? cached))
    {
        // Filter in-memory (fast!)
        var filtered = cached
            .FilterByName(request.Data?.Name)
            .FilterByDescription(request.Data?.Description)
            .ToList();
        
        return Result<IEnumerable<PizzaModel>>.Success(filtered);
    }

    // Cache miss: full database query + cache
    var entities = await databaseContext.Pizzas
        .AsNoTracking()
        .ToListAsync(cancellationToken);
    
    var mapped = entities.Map();
    cache.Set(cacheKey, mapped, CacheDuration);
    
    return Result<IEnumerable<PizzaModel>>.Success(mapped);
}
```

### Step 5: Invalidate Cache on Writes

In command handlers, remove cache keys after modifications:

```csharp
namespace Core.Pizza.Commands;

using Microsoft.Extensions.Caching.Memory;

public sealed class CreatePizzaCommandHandler(
    DatabaseContext databaseContext,
    IMemoryCache cache) : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> Handle(
        CreatePizzaCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Data == null)
            return Result<PizzaModel>.Failure("Pizza data required");

        var entity = new Pizza
        {
            Name = request.Data.Name,
            Description = request.Data.Description,
            Price = request.Data.Price,
            DateCreated = DateTime.UtcNow
        };

        databaseContext.Pizzas.Add(entity);
        var result = await databaseContext.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            // Invalidate cache after successful write
            cache.Remove(Common.Data.PizzasCacheKey);
        }

        return result > 0
            ? Result<PizzaModel>.Success(entity.Map())
            : Result<PizzaModel>.Failure("Failed to create pizza");
    }
}
```

### Step 6: Test Cache Behavior

Verify caching works by checking handler execution:

```csharp
[TestClass]
public class GetPizzasQueryHandlerTests
{
    private IMemoryCache cache;
    private DatabaseContext db;
    private GetPizzasQueryHandler handler;

    [TestInitialize]
    public void Setup()
    {
        cache = new MemoryCache(new MemoryCacheOptions());
        db = new DatabaseContext(new DbContextOptionsBuilder()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
        handler = new GetPizzasQueryHandler(db, cache);
    }

    [TestMethod]
    public async Task Handle_SecondCall_ReturnsCachedData()
    {
        var query = new GetPizzasQuery { Data = new SearchPizzaModel() };

        // First call: hits database
        var result1 = await handler.Handle(query, CancellationToken.None);
        Assert.IsTrue(result1.Succeeded);

        // Second call: returns from cache (no database hit)
        var result2 = await handler.Handle(query, CancellationToken.None);
        Assert.IsTrue(result2.Succeeded);
        
        // Data should be identical
        CollectionAssert.AreEqual(result1.Data, result2.Data);
    }

    [TestMethod]
    public async Task Handle_AfterInvalidation_QueriesDatabaseAgain()
    {
        var query = new GetPizzasQuery { Data = new SearchPizzaModel() };
        
        // First call: cache populated
        await handler.Handle(query, CancellationToken.None);
        
        // Invalidate
        cache.Remove(Common.Data.PizzasCacheKey);
        
        // Second call: cache miss, queries database again
        var result = await handler.Handle(query, CancellationToken.None);
        Assert.IsTrue(result.Succeeded);
    }
}
```

## Key Points

- **TTL (Time-to-Live)**: 10 minutes for pizza catalog (rarely changes); 1 minute for customer data (may update frequently)
- **Cache Keys**: Use constants for consistency; include entity type in key (`pizzas_all`, not just `all`)
- **Invalidation Strategy**: Remove cache on command success; keep cache on failure
- **Thread-Safety**: `IMemoryCache` is thread-safe; safe to use in concurrent requests
- **Memory Pressure**: Use `CacheEntryOptions` to set max size limits and eviction policies if needed

## Next Steps

Move to [Step 2 - Compression & Distributed Caching](../Step%202/README.md) to:

- Enable response compression (Brotli/Gzip)
- Implement `IDistributedCache` for multi-instance deployments
- Handle cache invalidation across distributed systems

## Recap: What You've Learned

- Implemented Cache-Aside pattern with `IMemoryCache`
- Set appropriate TTL values based on data volatility
- Invalidated cache on command execution
- Applied filters on cached collections
- Understood performance benefits (faster responses, reduced database load)

```cs
	public CachingService CachingService = new();

	public DatabaseContext Context => Create();

	public void Dispose() => Destroy(this.Context);
```

Add CachingService to all RestaurantDataAccess constructors

```cs
var sutGetAll = new GetPizzasQueryHandler(this.Context, this.CachingService);
```

[Move to Phase 6 Step 2](https://github.com/entelect-incubator/.NET/tree/master/Phase%206/Step%202)
