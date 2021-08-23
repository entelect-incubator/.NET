<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 5 - Step 1** [![.NET Core - Phase 5 - Step 1](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase5-step1.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase5-step1.yml)

<br/><br/>

## **Caching**

### **Install Lazy Cache**

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
        private readonly DatabaseContext databaseContext;
        private readonly IMapper mapper;

        public RestaurantDataAccess(DatabaseContext databaseContext, IMapper mapper, IAppCache cache)
            => (this.databaseContext, this.mapper, this.cache) = (databaseContext, mapper, cache);

        public async Task<RestaurantDTO> GetAsync(int id)
            => this.mapper.Map<RestaurantDTO>(await this.databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<ListResult<RestaurantDTO>> GetAllAsync(RestaurantDTO dto)
        {
            if (dto.BustCache)
            {
                this.ClearCache();
            }

            Task<List<RestaurantDTO>> DataDelegate() => this.GetRestaurantData();
            var data = await this.cache.GetOrAddAsync(this.cacheKey, DataDelegate, this.cacheExpiry);

            string orderBy = string.IsNullOrEmpty(dto.OrderBy) ? "DateCreated desc" : dto.OrderBy;
            var orderedData = data.ApplyPaging(dto.PagingArgs).OrderBy(orderBy).ToList();

            return ListResult<RestaurantDTO>.Success(orderedData, orderedData.Count);
        }

        public async Task<RestaurantDTO> SaveAsync(RestaurantDTO dto)
        {
            var entity = this.mapper.Map<Restaurant>(dto);
            this.databaseContext.Restaurants.Add(entity);
            await this.databaseContext.SaveChangesAsync();
            dto.Id = entity.Id;
            this.ClearCache();

            return dto;
        }

        public async Task<RestaurantDTO> UpdateAsync(RestaurantDTO dto)
        {
            var findEntity = await this.databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (findEntity == null)
            {
                return null;
            }

            findEntity.Name = !string.IsNullOrEmpty(dto.Name) ? dto.Name : findEntity.Name;
            findEntity.Description = !string.IsNullOrEmpty(dto.Description) ? dto.Description : findEntity.Description;
            findEntity.Address = !string.IsNullOrEmpty(dto?.Address?.Address) ? dto?.Address?.Address : findEntity.Address;
            findEntity.City = !string.IsNullOrEmpty(dto?.Address?.City) ? dto?.Address?.City : findEntity.City;
            findEntity.Province = !string.IsNullOrEmpty(dto?.Address?.Province) ? dto?.Address?.Province : findEntity.Province;
            findEntity.PostalCode = !string.IsNullOrEmpty(dto?.Address?.PostalCode) ? dto?.Address?.PostalCode : findEntity.PostalCode;
            findEntity.PictureUrl = !string.IsNullOrEmpty(dto.PictureUrl) ? dto.PictureUrl : findEntity.PictureUrl;
            findEntity.IsActive = dto.IsActive ?? findEntity.IsActive;

            this.databaseContext.Restaurants.Update(findEntity);
            await this.databaseContext.SaveChangesAsync();
            this.ClearCache();

            return this.mapper.Map<RestaurantDTO>(findEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }

            this.databaseContext.Restaurants.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();
            this.ClearCache();

            return result == 1;
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
        public CachingService CachingService = new CachingService();

        public DatabaseContext Context => Create();

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