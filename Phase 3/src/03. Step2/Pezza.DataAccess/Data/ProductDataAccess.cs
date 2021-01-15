namespace Pezza.DataAccess.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Common.Extensions;
    using Pezza.Common.Filter;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class ProductDataAccess : IDataAccess<Product>
    {
        private readonly IDatabaseContext databaseContext;

        public ProductDataAccess(IDatabaseContext databaseContext)
            => this.databaseContext = databaseContext;

        public async Task<Common.Entities.Product> GetAsync(int id)
        {
            return await this.databaseContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ListResult<Product>> GetAllAsync(ProductDataDTO searchModel)
        {
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
            var paged = await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync();

            return ListResult<Product>.Success(paged, count);
        }

        public async Task<Product> SaveAsync(Product entity)
        {
            this.databaseContext.Products.Add(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Product> UpdateAsync(Product entity)
        {
            this.databaseContext.Products.Update(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.GetAsync(id);
            this.databaseContext.Products.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}