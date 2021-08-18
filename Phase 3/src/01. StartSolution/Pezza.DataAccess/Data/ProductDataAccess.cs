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
    using Pezza.DataAccess.Contracts;

    public class ProductDataAccess : IDataAccess<ProductDTO>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public ProductDataAccess(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<ProductDTO> GetAsync(int id)
            => this.mapper.Map<ProductDTO>(await this.databaseContext.Products.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<List<ProductDTO>> GetAllAsync()
        {
            var entities = await this.databaseContext.Products.Select(x => x).AsNoTracking().ToListAsync();
            return this.mapper.Map<List<ProductDTO>>(entities);
        }

        public async Task<ProductDTO> SaveAsync(ProductDTO dto)
        {
            var entity = this.mapper.Map<Product>(dto);
            this.databaseContext.Products.Add(entity);
            await this.databaseContext.SaveChangesAsync();
            dto.Id = entity.Id;

            return dto;
        }

        public async Task<ProductDTO> UpdateAsync(ProductDTO dto)
        {
            var findEntity = await this.databaseContext.Products.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if(findEntity == null)
            {
                return null;
            }

            findEntity.Name = !string.IsNullOrEmpty(dto.Name) ? dto.Name : findEntity.Name;
            findEntity.Description = !string.IsNullOrEmpty(dto.Description) ? dto.Description : findEntity.Description;
            findEntity.PictureUrl = !string.IsNullOrEmpty(dto.PictureUrl) ? dto.PictureUrl : findEntity.PictureUrl;
            findEntity.Price = dto.Price ?? findEntity.Price;
            findEntity.Special = dto.Special ?? findEntity.Special;
            findEntity.OfferEndDate = dto.OfferEndDate ?? findEntity.OfferEndDate;
            findEntity.OfferPrice = dto.OfferPrice ?? findEntity.OfferPrice;
            findEntity.IsActive = dto.IsActive ?? findEntity.IsActive;

            this.databaseContext.Products.Update(findEntity);
            await this.databaseContext.SaveChangesAsync();

            return this.mapper.Map<ProductDTO>(findEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.databaseContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }

            this.databaseContext.Products.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}