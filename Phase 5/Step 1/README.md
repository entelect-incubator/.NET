<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 5 - Step 1** [![.NET 7 - Phase 5 - Step 1](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase5-step1.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase5-step1.yml)

<br/><br/>

## **Caching**

### **Install Lazy Cache**

Install Nuget Package LazyCache.AspNetCore on Core and API

![](2021-01-15-12-44-19.png)

### **Dependency Injection in DependencyInjection.cs**

In Pezza.API Startup.cs ConfigureServices() add

```cs
services.AddLazyCache();
```

This will inject IAppCache throughout your application.

Modify the Restaurant Data Access. Remove the filters, because we won't be using that.

Add CacheBust property to RestaurantDTO

```cs
public bool BustCache { get; set; } = false;
```

This will cache the request to memory if it doesn't exist. When you change anything on the database it will bust the cache.

Modify GetRestaurantsQuery.cs to add caching

```cs
namespace Pezza.Core.Restaurant.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pezza.Common.DTO;
using Pezza.Common.Extensions;
using Pezza.Common.Models;
using Pezza.DataAccess;

public class GetRestaurantsQuery : IRequest<ListResult<RestaurantDTO>>
{
    public RestaurantDTO Data { get; set; }
}

public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, ListResult<RestaurantDTO>>
{
    private readonly IAppCache cache;

    private readonly string cacheKey = "RestaurantList";

    private readonly TimeSpan cacheExpiry = new (12, 0, 0);

    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetRestaurantsQueryHandler(DatabaseContext databaseContext, IMapper mapper, IAppCache cache)
        => (this.databaseContext, this.mapper, this.cache) = (databaseContext, mapper, cache);

    public async Task<ListResult<RestaurantDTO>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Data;

        if (dto.BustCache)
        {
            this.ClearCache();
        }

        Task<List<RestaurantDTO>> DataDelegate()
        {
            return this.GetRestaurantData();
        }

        var data = await this.cache.GetOrAddAsync(this.cacheKey, DataDelegate, this.cacheExpiry);

        var orderBy = string.IsNullOrEmpty(dto.OrderBy) ? "DateCreated desc" : dto.OrderBy;
        var orderedData = data.AsQueryable().ApplyPaging(dto.PagingArgs).OrderBy(orderBy);

        var count = data.Count;
        var paged = this.mapper.Map<List<RestaurantDTO>>(orderedData);

        return ListResult<RestaurantDTO>.Success(paged, count);
    }

    private async Task<List<RestaurantDTO>> GetRestaurantData()
    {
        var entities = await this.databaseContext.Restaurants.Select(x => x)
            .AsNoTracking()
            .ToListAsync();

        return this.mapper.Map<List<RestaurantDTO>>(entities);
    }

    private void ClearCache() => this.cache.Remove(this.cacheKey);
}
```

## **Unit Test**

Add CachingService to QueryTestBase

```cs
namespace Pezza.Test;

using AutoMapper;
using LazyCache;
using Pezza.Common.Profiles;
using Pezza.DataAccess;
using static DatabaseContextFactory;

public class QueryTestBase : IDisposable
{
    public CachingService CachingService = new();

    public static DatabaseContext Context => Create();

    public static IMapper Mapper()
    {
        var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
        return mappingConfig.CreateMapper();
    }

    public void Dispose() => Destroy(Context);
}
```

Add CachingService to all RestaurantDataAccess constructors

```cs
var handler = new RestaurantDataAccess(this.Context, Mapper(), this.CachingService);
```

Move to Phase 5 Step 2
[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%205/Step%202) 