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

## Prerequisites

- Completed Phase 5 (understand error handling and standards)
- .NET 10 SDK installed and on PATH
- Familiarity with caching concepts and distributed systems basics

## How to validate this phase locally

1. Build the start solution:

```powershell
dotnet build "Phase 6/src/01. StartSolution/Pezza.sln"
```

2. Run tests:

```powershell
dotnet test "Phase 6/src/01. StartSolution/Pezza.sln"
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

## Steps

- [ ] [Step 1 - In-Memory Caching](Phase%206/src/02.%20Step%201)
- [ ] [Step 2 - Compression & Distributed Caching](Phase%206/src/03.%20Step%202)
