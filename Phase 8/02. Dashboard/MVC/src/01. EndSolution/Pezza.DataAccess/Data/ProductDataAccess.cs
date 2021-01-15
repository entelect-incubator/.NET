namespace Pezza.DataAccess.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.Entities;
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

        public async Task<List<Product>> GetAllAsync()
        {
            var entities = await this.databaseContext.Products.Select(x => x).AsNoTracking().ToListAsync();

            return entities;
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