<img align="left" width="116" height="116" src="pezza-logo.png" />

# &nbsp;**Pezza - Phase 6 — Caching & Compression** [![.NET - Phase 6 - Final Solution](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-finalsolution.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-finalsolution.yml)

<br/><br/>

## Quick facts

- .NET SDK required: 10 (net10)
- Estimated time: 4 - 8 hours
- Difficulty: ★★★★☆ (advanced intermediate)
- Audience: developers who completed Phase 5; ready to optimize performance through caching and compression
- **Building on**: Phase 5's error handling and standards

## Goal

This phase adds **performance optimization** techniques to your dispatcher architecture:

- **Caching Patterns**: Implement cache-aside and distributed caching strategies
- **Query Caching**: Cache expensive query results in handlers using decorator patterns
- **Distributed Caching**: Use Redis or similar for multi-instance cache synchronization
- **Response Compression**: Enable gzip/brotli compression for API responses

Learn to reduce database load, improve response times, and handle cache invalidation scenarios.

## Design Patterns Used in This Phase

- **[Repository Pattern](https://github.com/entelect-incubator/Design-Patterns/tree/main/Repository-Pattern)** – Cache-aside pattern with repository abstraction
- **[CQRS Pattern](https://github.com/entelect-incubator/Design-Patterns/tree/main/CQRS)** – Query handlers decorated with caching logic
- **[Feature Architecture](https://github.com/entelect-incubator/Design-Patterns/tree/main/Feature-Architecture)** – Cache invalidation scoped to feature boundaries
- **[Clean Code Principles](https://github.com/entelect-incubator/Design-Patterns/tree/main/Clean-Code)** – Clear cache key naming, TTL strategies, readable cache policies

## Prerequisites

- Completed Phase 5 (understand error handling and standards)
- .NET 10 SDK installed and on PATH
- Familiarity with caching concepts and distributed systems basics

## How to validate this phase locally

1. Build the start solution:

```powershell
dotnet build "Phase 6/src/01. StartSolution/Pezza.slnx"
```

2. Run tests:

```powershell
dotnet test "Phase 6/src/01. StartSolution/Pezza.slnx"
```

## Topics / learning outcomes

- Implement **Cache-Aside pattern** in query handlers
- Use **IMemoryCache** for in-process caching with TTL strategies
- Configure **IDistributedCache** for Redis or SQL Server distributed caching
- Understand **cache invalidation** strategies (time-based, event-based, manual)
- Enable **response compression** middleware for bandwidth optimization
- Learn performance implications of caching vs. memory usage

## Key Patterns: Caching Handlers

**In-Process Caching:**

```csharp
public sealed class GetPizzaCachedQuery(
    DatabaseContext db, 
    IMemoryCache cache) 
    : IQueryHandler<GetPizzaQuery, Result<PizzaModel?>>
{
    private const string CacheKey = "pizza_{id}";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    public async Task<Result<PizzaModel?>> Handle(GetPizzaQuery query, CancellationToken ct)
    {
        var key = string.Format(CacheKey, query.Id);
        
        if (cache.TryGetValue(key, out PizzaModel? cached))
            return Result<PizzaModel?>.Success(cached);

        var pizza = await db.Pizzas
            .FirstOrDefaultAsync(p => p.Id == query.Id, ct);
        
        if (pizza is not null)
            cache.Set(key, pizza.Map(), CacheDuration);

        return Result<PizzaModel?>.Success(pizza?.Map());
    }
}
```

## References

- Cache-Aside Pattern: [https://learn.microsoft.com/azure/architecture/patterns/cache-aside](https://learn.microsoft.com/azure/architecture/patterns/cache-aside)
- IMemoryCache: [Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.memory.imemorycache)
- IDistributedCache: [Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.distributed.idistributedcache)
- Response Compression Middleware: [Microsoft Docs](https://learn.microsoft.com/en-us/aspnet/core/performance/response-compression)
- Redis: [https://redis.io/](https://redis.io/)

## Why Phase 6 — Performance Optimization

You've built a working, standards-compliant API with centralized error handling and a clean dispatcher architecture. But **production systems must handle scale**:

- **Database queries**: Repeated queries hit the database every request, wasting resources
- **Bandwidth costs**: Large JSON responses consume bandwidth and slow down clients
- **Memory issues**: Without cache invalidation, stale data accumulates in memory
- **Latency**: Each request to the database adds milliseconds of delay

This phase teaches you to **reduce database load through caching** and **optimize bandwidth with compression** while maintaining correctness through proper cache invalidation.

## What We're Building

### **Step 1: In-Memory Caching**

- Implement Cache-Aside pattern in query handlers using `IMemoryCache`
- Store frequently-accessed query results with TTL (time-to-live)
- Invalidate cache when commands modify data
- Filter cached collections without hitting the database

**Before (no caching):**

```csharp
public async Task<Result<IEnumerable<PizzaModel>>> Handle(GetPizzasQuery query, CancellationToken ct)
{
    var entities = await databaseContext.Pizzas.ToListAsync(ct);  // Every request hits database
    return Result<IEnumerable<PizzaModel>>.Success(entities.Map());
}
```

**After (cached):**

```csharp
public async Task<Result<IEnumerable<PizzaModel>>> Handle(
    GetPizzasQuery query,
    IMemoryCache cache,
    CancellationToken ct)
{
    const string cacheKey = "pizzas_all";
    
    if (cache.TryGetValue(cacheKey, out IEnumerable<PizzaModel>? cached))
        return Result<IEnumerable<PizzaModel>>.Success(cached);

    var entities = await databaseContext.Pizzas.ToListAsync(ct);
    var mapped = entities.Map();
    
    cache.Set(cacheKey, mapped, TimeSpan.FromMinutes(10));
    return Result<IEnumerable<PizzaModel>>.Success(mapped);
}
```

Commands invalidate the cache:

```csharp
public async Task<Result<PizzaModel>> Handle(
    CreatePizzaCommand command,
    IMemoryCache cache,
    CancellationToken ct)
{
    // ... create pizza ...
    
    cache.Remove("pizzas_all");  // Bust the cache
    return Result<PizzaModel>.Success(pizza.Map());
}
```

### **Step 2: Compression & Distributed Caching**

- Enable response compression (Brotli/Gzip) middleware
- Use `IDistributedCache` for multi-instance cache synchronization (Redis/SQL)
- Handle cache invalidation across distributed systems
- Measure performance improvements

## How It Works

### **Cache-Aside Pattern**

1. Request comes in for `/api/pizza`
2. Check cache: "pizzas_all" key exists?
   - **Hit**: Return cached data (⚡ fast)
   - **Miss**: Query database → cache result → return data
3. Command (Create/Update/Delete) executes:
   - Modify database
   - Call `cache.Remove("pizzas_all")`
   - Next query request will repopulate cache

### **Response Compression**

1. API builds response (e.g., large JSON array)
2. Middleware checks client's Accept-Encoding header
3. Compress using Brotli (if supported) or Gzip
4. Send compressed response with Content-Encoding header
5. Client automatically decompresses

**Typical compression ratio**: 60-80% size reduction for JSON.

## Steps

- [ ] [Step 1 - In-Memory Caching](Phase%206/src/02.%20Step%201)
- [ ] [Step 2 - Compression & Distributed Caching](Phase%206/src/03.%20Step%202)

## End Solution (what to verify)

- **Why**: Demonstrates optimized query performance with multi-level caching (in-memory + option for distributed) and bandwidth-efficient responses via compression; custom dispatcher routes cached queries seamlessly.
- **What**: `IMemoryCache` in query handlers with TTL strategies; cache invalidation in command handlers via `cache.Remove()`; response compression middleware configured with Brotli/Gzip; controllers calling `Dispatcher.Send/Query`.
- **How to run**: `dotnet build "Phase 6/src/04. EndSolution/Pezza.slnx"` (verify it exists) then `dotnet test`.
- **Learning check**: You should understand cache-aside pattern, TTL trade-offs (freshness vs. database load), when to cache (reads) vs. invalidate (writes), and how compression reduces bandwidth without changing API contract.

## What You Should Know By Now

After completing Phase 6, you should understand:

### **Caching Patterns**

- **Cache-Aside**: Application is responsible for loading cache misses; simplest pattern
- **Cache Invalidation**: Time-based (TTL) vs. event-based (manual removal); trades freshness for performance
- **In-Process vs. Distributed**: In-memory fast for single instance; distributed cache for multi-instance deployments
- **Cache Keys**: Must be deterministic and scoped (e.g., `pizza_{id}`, `pizzas_all_by_name`)

### **Performance Implications**

- Caching reduces database load but increases memory usage
- Cache misses on cold start; cache warming strategies help
- Stale data in cache; TTL and invalidation mitigate
- Compression trades CPU (compression/decompression) for bandwidth
- Measure: Compare response times, database query count, network bytes with/without cache

### **IMemoryCache & IDistributedCache**

- **IMemoryCache**: In-process, fast, lost on application restart
- **IDistributedCache**: Redis/SQL-backed, persistent, multi-instance safe
- API same; implementation differs (trade-off: latency vs. durability)

### **Integration with Dispatcher**

- Caching is a **query concern**; use in `IQueryHandler<TQuery, TResult>`
- Cache invalidation is a **command concern**; use in `ICommandHandler<TCommand, TResult>`
- Dispatcher routes to correct handler; caching logic stays in handlers (single responsibility)
- Avoid caching in middleware; cache in handlers for fine-grained control

### **Why Response Compression**

- Automatic; configured once in Startup/Program
- Transparent to API logic; browser/client handles decompression
- No API contract changes; just smaller responses
- Measure: Compare Content-Length header with/without compression

## Knowledge Check

Test your understanding with these questions:

1. **What's the difference between cache hit and miss, and when does each happen?**
   <details><summary>Answer</summary>
   Hit: Key exists in cache, return cached value immediately (no database query). Miss: Key doesn't exist, query database, cache result, return data. Cold start has many misses; after stabilization, hits dominate.
   </details>

2. **Why invalidate cache on commands but not queries?**
   <details><summary>Answer</summary>
   Queries read data; caching stale reads is safe if TTL is reasonable. Commands modify data; caching post-modification would serve stale data. Invalidate immediately to force next read to get fresh data.
   </details>

3. **What's the trade-off between TTL (time-to-live) and freshness?**
   <details><summary>Answer</summary>
   Longer TTL = fewer database hits but staler data. Shorter TTL = fresher data but more database queries. Choose based on data volatility: stable data (pizza catalog) can have 12-hour TTL; user preferences might need 5-minute TTL.
   </details>

4. **When should you use IDistributedCache instead of IMemoryCache?**
   <details><summary>Answer</summary>
   IMemoryCache for single-instance apps or when stale data across instances is acceptable. IDistributedCache for multi-instance deployments (load balancers, multiple servers) where all instances must see same cache (Redis, SQL Server).
   </details>

5. **How does response compression affect the API contract or client code?**
   <details><summary>Answer</summary>
   No changes. Compression happens at HTTP middleware layer. Clients send Accept-Encoding header; server compresses; client decompresses automatically. API logic, model definitions, and controller signatures remain unchanged.
   </details>

6. **What happens if you cache a mutable object without deep copying?**
   <details><summary>Answer</summary>
   Danger: If cached object is modified, all cache consumers see the change. Solution: Cache immutable snapshots or deep-copy on retrieval. In the pattern shown, we cache mapped models (immutable); safe.
   </details>

Move to Phase 7
[Phase 7](https://github.com/entelect-incubator/.NET/tree/master/Phase%207)

## Next Step
Move to [Phase 7](https://github.com/entelect-incubator/.NET/tree/master/Phase%207)

---

Teaching Thread

- From: Phase 5 enforced standards and error handling.
- This phase: introduce domain events, background jobs, and distributed-safe patterns.
- Next: Phase 7 expands microservices and service-to-service communication.

Libraries (why they matter)

- Hangfire / Background worker frameworks: teaches background processing patterns and reliability concerns.
- Lightweight messaging/eventing utilities: demonstrate domain events without full message brokers.

Clean Code & SOLID (teaching notes)

- Design event handlers with single responsibility; prefer idempotent handlers.
- Keep performance and observability responsibilities out of domain handlers; inject logging/telemetry instead.

MediatR policy

- Continue using the custom dispatcher for command/query flows; if events require richer mediator features, document why a third-party mediator is used.

Notes

- Link to Design-Patterns for Events and CQRS: ../../Design-Patterns/03-CQRS-Pattern/
