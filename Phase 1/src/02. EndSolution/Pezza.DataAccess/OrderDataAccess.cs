namespace Pezza.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.DataAccess.Contracts;

    public class OrderDataAccess : IOrderDataAccess
    {
        private readonly IDatabaseContext databaseContext;

        public OrderDataAccess(IDatabaseContext databaseContext) => this.databaseContext = databaseContext;

        public async Task<Common.Entities.Order> GetAsync(int id)
        {
            return await this.databaseContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Common.Entities.Order>> GetAllAsync(int id)
        {
            return await this.databaseContext.Orders.ToListAsync();
        }

        public async Task<Common.Entities.Order> SaveAsync(Common.Entities.Order entity)
        {
            this.databaseContext.Orders.Add(entity);
            await this.databaseContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Common.Entities.Order> UpdateAsync(Common.Entities.Order entity)
        {
            this.databaseContext.Orders.Update(entity);
            await this.databaseContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Common.Entities.Order entity)
        {
            this.databaseContext.Orders.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();
            return (result == 1);
        }
    }
}
