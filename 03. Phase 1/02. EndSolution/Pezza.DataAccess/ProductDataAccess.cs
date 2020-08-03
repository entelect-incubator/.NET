namespace Pezza.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.DataAccess.Contracts;

    public class ProductDataAccess : IProductDataAccess
    {
        private readonly IDatabaseContext databaseContext;

        public ProductDataAccess(IDatabaseContext databaseContext) => this.databaseContext = databaseContext;

        public async Task<Common.Entities.Product> GetAsync(int id)
        {
            return await this.databaseContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Common.Entities.Product>> GetAllAsync(int id)
        {
            return await this.databaseContext.Products.ToListAsync();
        }

        public async Task<Common.Entities.Product> SaveAsync(Common.Entities.Product entity)
        {
            this.databaseContext.Products.Add(entity);
            await this.databaseContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Common.Entities.Product> UpdateAsync(Common.Entities.Product entity)
        {
            this.databaseContext.Products.Update(entity);
            await this.databaseContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Common.Entities.Product entity)
        {
            this.databaseContext.Products.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();
            return (result == 1);
        }
    }
}
