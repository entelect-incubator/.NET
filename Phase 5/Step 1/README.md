<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 5 - Step 1** [![.NET Core - Phase 5 - Step 1](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase5-step1.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase5-step1.yml)

<br/><br/>

Caching

## **Install Lazy Cache**

Install Nuget Package LazyCache.AspNetCore on Core and DataAccess Project

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

```cs
namespace Pezza.DataAccess.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using AutoMapper;
    using LazyCache;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Common.Extensions;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class RestaurantDataAccess : IDataAccess<RestaurantDTO>
    {
        private readonly IAppCache cache;
        private readonly string cacheKey = "RestaurantList";
        private readonly TimeSpan cacheExpiry = new TimeSpan(12, 0, 0);
        private readonly IDatabaseContext databaseContext;
        private readonly IMapper mapper;
        private int count = 0;

        public RestaurantDataAccess(IDatabaseContext databaseContext, IMapper mapper, IAppCache cache)
            => (this.databaseContext, this.mapper, this.cache) = (databaseContext, mapper, cache);

        public async Task<RestaurantDTO> GetAsync(int id)
            => this.mapper.Map<RestaurantDTO>(await this.databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<ListResult<RestaurantDTO>> GetAllAsync(Entity searchBase)
        {
            var searchModel = (RestaurantDTO)searchBase;
            if (searchModel.BustCache)
            {
                this.ClearPageCache();
            }

            Task<List<RestaurantDTO>> CacheFactory() => this.GetRestaurantCache(searchModel);
            var data = await this.cache.GetOrAddAsync(this.cacheKey, CacheFactory, this.cacheExpiry);

            return ListResult<RestaurantDTO>.Success(data, data.Count());
        }

        private async Task<List<RestaurantDTO>> GetRestaurantCache(RestaurantDTO searchModel)
        {
            if (string.IsNullOrEmpty(searchModel.OrderBy))
            {
                searchModel.OrderBy = "DateCreated desc";
            }

            var entities = this.databaseContext.Restaurants.Select(x => x)
                .AsNoTracking()
                .OrderBy(searchModel.OrderBy);

            this.count = entities.Count();
            var paged = await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync();

            return this.mapper.Map<List<RestaurantDTO>>(paged);
        }

        public void ClearPageCache() => this.cache.Remove(this.cacheKey);

        public async Task<RestaurantDTO> SaveAsync(RestaurantDTO entity)
        {
            this.databaseContext.Restaurants.Add(this.mapper.Map<Restaurant>(entity));
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<RestaurantDTO> UpdateAsync(RestaurantDTO entity)
        {
            var findEntity = await this.databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == entity.Id);
            findEntity.Name = !string.IsNullOrEmpty(entity.Name) ? entity.Name : findEntity.Name;
            findEntity.Description = !string.IsNullOrEmpty(entity.Description) ? entity.Description : findEntity.Description;
            findEntity.Address = !string.IsNullOrEmpty(entity?.Address?.Address) ? entity?.Address?.Address : findEntity.Address;
            findEntity.City = !string.IsNullOrEmpty(entity?.Address?.City) ? entity?.Address?.City : findEntity.City;
            findEntity.Province = !string.IsNullOrEmpty(entity?.Address?.Province) ? entity?.Address?.Province : findEntity.Province;
            findEntity.PostalCode = !string.IsNullOrEmpty(entity?.Address?.PostalCode) ? entity?.Address?.PostalCode : findEntity.PostalCode;
            findEntity.PictureUrl = !string.IsNullOrEmpty(entity.PictureUrl) ? entity.PictureUrl : findEntity.PictureUrl;
            findEntity.IsActive = entity.IsActive ?? findEntity.IsActive;

            this.databaseContext.Restaurants.Update(findEntity);
            await this.databaseContext.SaveChangesAsync();

            return this.mapper.Map<RestaurantDTO>(findEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == id);
            this.databaseContext.Restaurants.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return result == 1;
        }
    }
}
```

## **Unit Test**

Add CachingService to QueryTestBase

```cs
namespace Pezza.Test
{
    using System;
    using AutoMapper;
    using LazyCache;
    using Pezza.Common.Profiles;
    using Pezza.DataAccess;
    using static DatabaseContextFactory;

    public class QueryTestBase : IDisposable
    {
        public DatabaseContext Context => Create();

        public CachingService CachingService = new CachingService();

        public static IMapper Mapper()
        {
            var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
            return mappingConfig.CreateMapper();
        }

        public void Dispose() => Destroy(this.Context);
    }
}
```

Add CachingService to all RestaurantDataAccess constructors

```cs
var handler = new RestaurantDataAccess(this.Context, Mapper(), this.CachingService);
```

Move to Phase 5 Step 2
[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%205/Step%202) 