namespace Pezza.DataAccess.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Common.Extensions;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class RestaurantDataAccess : IDataAccess<RestaurantDTO>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public RestaurantDataAccess(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<RestaurantDTO> GetAsync(int id)
            => this.mapper.Map<RestaurantDTO>(await this.databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<ListResult<RestaurantDTO>> GetAllAsync(RestaurantDTO dto)
        {
            var entities = this.databaseContext.Restaurants.Select(x => x)
                .AsNoTracking();

            var count = entities.Count();
            var paged = await entities.ApplyPaging(dto.PagingArgs).ToListAsync();

            return ListResult<RestaurantDTO>.Success(this.mapper.Map<List<RestaurantDTO>>(paged), count);
        }

        public async Task<RestaurantDTO> SaveAsync(RestaurantDTO dto)
        {
            var entity = this.mapper.Map<Restaurant>(dto);
            this.databaseContext.Restaurants.Add(entity);
            await this.databaseContext.SaveChangesAsync();
            dto.Id = entity.Id;

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

            return (result == 1);
        }
    }
}