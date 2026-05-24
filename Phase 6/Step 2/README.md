<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 6 - Step 2** [![.NET - Phase 6 - Step 2](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step2.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step2.yml)

<br/><br/>

## Response Compression & Distributed Caching

**Difficulty**: ★★★☆☆ (Intermediate)  
**Estimated Time**: 2-3 hours  
**Prerequisites**:

- Completed Step 1 (understand in-memory caching)
- Basic understanding of distributed systems and Redis/SQL Server

### Learning Outcomes

After completing this step, you will:

- Enable response compression (Brotli/Gzip) middleware
- Understand compression trade-offs (CPU vs. bandwidth)
- Implement `IDistributedCache` for multi-instance deployments
- Handle cache invalidation across distributed instances
- Measure compression effectiveness and performance

## Part 1: Response Compression

### Why Compress Responses?

Large API responses consume bandwidth:

- JSON arrays with 100+ objects can exceed 1-2 MB
- Mobile clients on slow networks benefit from smaller payloads
- Compression reduces bandwidth costs and improves latency

**Typical compression ratios for JSON**: 60-80% size reduction.

### How to Enable

In `Api/Startup.cs` or `Program.cs`:

```csharp
// In ConfigureServices()
services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
});

// In Configure() / Program.cs
app.UseResponseCompression();
```

### How It Works

1. **Client sends request** with `Accept-Encoding: br, gzip` header
2. **Server middleware** checks header and response size
3. **Server compresses** response using Brotli (preferred) or Gzip
4. **Server sends** response with `Content-Encoding: br` (or `gzip`) header
5. **Client automatically decompresses** (transparent to user)

**No API changes required**: Controllers return `Ok(data)` as normal; compression happens at HTTP layer.

### Verify Compression

Test with curl:

```powershell
# Without compression
curl -I https://localhost:5001/api/pizza

# With compression (Gzip)
curl -H "Accept-Encoding: gzip" -I https://localhost:5001/api/pizza
# Check response header: Content-Encoding: gzip
# Compare Content-Length reduction
```

## Part 2: Distributed Caching with IDistributedCache

### Why Distributed Cache?

Single-instance caching (`IMemoryCache`) works for:

- Development
- Single-server production deployments
- Acceptable eventual consistency

But fails when:

- Multiple app instances behind load balancer
- Different instances have different cache contents
- Cache invalidation doesn't sync across instances

**Solution**: `IDistributedCache` stores in Redis or SQL Server; all instances access same cache.

### Option A: Redis Implementation

Install NuGet packages:

```
StackExchange.Redis
Microsoft.Extensions.Caching.StackExchangeRedis
```

In `Startup.cs`:

```csharp
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = Configuration.GetConnectionString("Redis");
    // Connection string example: "localhost:6379"
});
```

### Option B: SQL Server Implementation

Install NuGet package:
```
Microsoft.Extensions.Caching.SqlServer
```

In `Startup.cs`:

```csharp
services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
    options.SchemaName = "dbo";
    options.TableName = "DistributedCache";
});
```

### Using IDistributedCache

Replace `IMemoryCache` with `IDistributedCache` in handlers (API same!):

```csharp
public sealed class GetPizzasQueryHandler(
    DatabaseContext databaseContext,
    IDistributedCache cache) : IQueryHandler<GetPizzasQuery, Result<IEnumerable<PizzaModel>>>
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    public async Task<Result<IEnumerable<PizzaModel>>> Handle(
        GetPizzasQuery request,
        CancellationToken cancellationToken)
    {
        const string cacheKey = Common.Data.PizzasCacheKey;

        // Try to get from distributed cache
        var cached = await cache.GetStringAsync(cacheKey, cancellationToken);
        if (!string.IsNullOrEmpty(cached))
        {
            var data = JsonSerializer.Deserialize<IEnumerable<PizzaModel>>(cached);
            return Result<IEnumerable<PizzaModel>>.Success(data);
        }

        // Cache miss: query database
        var entities = await databaseContext.Pizzas
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var mapped = entities.Map();

        // Cache as JSON string for 10 minutes
        await cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(mapped),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheDuration },
            cancellationToken);

        return Result<IEnumerable<PizzaModel>>.Success(mapped);
    }
}
```

### Cache Invalidation Across Instances

Commands invalidate same way (all instances see it):

```csharp
public sealed class CreatePizzaCommandHandler(
    DatabaseContext databaseContext,
    IDistributedCache cache) : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
    public async Task<Result<PizzaModel>> Handle(
        CreatePizzaCommand request,
        CancellationToken cancellationToken)
    {
        // ... create pizza ...

        databaseContext.Pizzas.Add(entity);
        var result = await databaseContext.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            // All instances see this invalidation
            await cache.RemoveAsync(Common.Data.PizzasCacheKey, cancellationToken);
        }

        return result > 0
            ? Result<PizzaModel>.Success(entity.Map())
            : Result<PizzaModel>.Failure("Failed to create pizza");
    }
}
```

## Recap: What You've Learned

- **Response Compression**: Brotli/Gzip at middleware layer; transparent to API logic
- **Distributed Caching**: Redis/SQL-backed cache for multi-instance deployments
- **IMemoryCache vs. IDistributedCache**: Trade-off between latency (in-process) and consistency (distributed)
- **Cache Invalidation**: Same pattern works for both; `IDistributedCache.RemoveAsync()` syncs across instances
- **Performance Measurement**: Compare response times, database hits, network bytes with/without compression and caching

## Knowledge Check

Test your understanding with these questions:

1. **Why might you use response compression even with in-memory caching?**
   <details><summary>Answer</summary>
   Cache stores data in memory; compression happens on-the-wire. Together: cache reduces database queries; compression reduces bandwidth. Complementary optimizations at different layers.
   </details>

2. **What's the difference between IMemoryCache and IDistributedCache?**
   <details><summary>Answer</summary>
   IMemoryCache: in-process, per-instance, lost on restart, microsecond latency. IDistributedCache: Redis/SQL-backed, shared across instances, persistent, millisecond latency. Choose based on deployment model.
   </details>

3. **How does cache invalidation work across distributed instances with Redis?**
   <details><summary>Answer</summary>
   All instances use same Redis. When instance A calls `cache.RemoveAsync(key)`, Redis deletes key. Instance B's next cache.GetAsync(key) misses and queries database. Automatic synchronization.
   </details>

4. **What's the compression trade-off you're making?**
   <details><summary>Answer</summary>
   Trading CPU (compression/decompression) for bandwidth. Modern CPUs compress JSON very fast; savings on bandwidth usually outweigh CPU cost. Measure in production for your data patterns.
   </details>

5. **Can you change from IMemoryCache to IDistributedCache without changing handler code?**
   <details><summary>Answer</summary>
   Mostly yes, with caveats. API is similar (Get/Set/Remove), but IDistributedCache works with serialized strings; you must handle serialization/deserialization. Plan for this when designing handlers.
   </details>

## Next Steps

Complete this step and move to Phase 7 for advanced patterns or return to Phase 6 main README for summary.

Move to [Phase 7](https://github.com/entelect-incubator/.NET/tree/master/Phase%207)
