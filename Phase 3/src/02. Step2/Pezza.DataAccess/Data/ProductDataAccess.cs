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
    using Pezza.Common.Extensions;
    using Pezza.Common.Filter;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class ProductDataAccess : IDataAccess<ProductDTO>
    {
        private readonly IDatabaseContext databaseContext;

        private readonly IMapper mapper;

        public ProductDataAccess(IDatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<ProductDTO> GetAsync(int id)
            => this.mapper.Map<ProductDTO>(await this.databaseContext.Products.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<ListResult<ProductDTO>> GetAllAsync(Entity searchBase)
        {
            var searchModel = (ProductDTO)searchBase;
            if (string.IsNullOrEmpty(searchModel.OrderBy))
            {
                searchModel.OrderBy = "DateCreated desc";
            }

            var entities = this.databaseContext.Products.Select(x => x)
                .AsNoTracking()
                .FilterByName(searchModel.Name)
                .FilterByDescription(searchModel.Description)
                .FilterByPictureUrl(searchModel.PictureUrl)
                .FilterByPrice(searchModel.Price)
                .FilterBySpecial(searchModel.Special)
                .FilterByOfferEndDate(searchModel.OfferEndDate)
                .FilterByOfferPrice(searchModel.OfferPrice)
                .FilterByIsActive(searchModel.IsActive)
                .FilterByDateCreated(searchModel.DateCreated)

                .OrderBy(searchModel.OrderBy);

            var count = entities.Count();
            var paged = this.mapper.Map<List<ProductDTO>>(await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync());

            return ListResult<ProductDTO>.Success(paged, count);
        }

        public async Task<ProductDTO> SaveAsync(ProductDTO entity)
        {
            this.databaseContext.Products.Add(this.mapper.Map<Product>(entity));
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<ProductDTO> UpdateAsync(ProductDTO entity)
        {
            var findEntity = await this.databaseContext.Products.FirstOrDefaultAsync(x => x.Id == entity.Id);
            findEntity.Name = !string.IsNullOrEmpty(entity.Name) ? entity.Name : findEntity.Name;
            findEntity.Description = !string.IsNullOrEmpty(entity.Description) ? entity.Description : findEntity.Description;
            findEntity.PictureUrl = !string.IsNullOrEmpty(entity.PictureUrl) ? entity.PictureUrl : findEntity.PictureUrl;
            findEntity.Price = entity.Price ?? findEntity.Price;
            findEntity.Special = entity.Special ?? findEntity.Special;
            findEntity.OfferEndDate = entity.OfferEndDate ?? findEntity.OfferEndDate;
            findEntity.OfferPrice = entity.OfferPrice ?? findEntity.OfferPrice;
            findEntity.IsActive = entity.IsActive ?? findEntity.IsActive;

            this.databaseContext.Products.Update(findEntity);
            await this.databaseContext.SaveChangesAsync();

            return this.mapper.Map<ProductDTO>(findEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.databaseContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            this.databaseContext.Products.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}