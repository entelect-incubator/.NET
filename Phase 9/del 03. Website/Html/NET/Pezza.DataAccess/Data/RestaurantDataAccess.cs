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
            this.ClearPageCache();
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
            this.ClearPageCache();
            return this.mapper.Map<RestaurantDTO>(findEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == id);
            this.databaseContext.Restaurants.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();
            this.ClearPageCache();
            return result == 1;
        }
    }
}