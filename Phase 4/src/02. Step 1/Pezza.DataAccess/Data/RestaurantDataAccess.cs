namespace Pezza.DataAccess.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class RestaurantDataAccess : IDataAccess<RestaurantDTO>
    {
        private readonly IDatabaseContext databaseContext;
        private readonly IMapper mapper;

        public RestaurantDataAccess(IDatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<RestaurantDTO> GetAsync(int id)
            => this.mapper.Map<RestaurantDTO>(await this.databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<ListResult<RestaurantDTO>> GetAllAsync(Entity searchBase)
        {
            var searchModel = (RestaurantDTO)searchBase;
            var entities = this.mapper.Map<List<RestaurantDTO>>(await this.databaseContext.Restaurants.Select(x => x).AsNoTracking().ToListAsync());
            var count = entities.Count();

            return ListResult<RestaurantDTO>.Success(entities, count);
        }

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
            findEntity.PostalCode = !string.IsNullOrEmpty(entity?.Address?.ZipCode) ? entity?.Address?.ZipCode : findEntity.PostalCode;
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