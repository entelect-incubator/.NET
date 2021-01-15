namespace Pezza.DataAccess.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.Entities;
    using Pezza.DataAccess.Contracts;

    public class OrderDataAccess : IDataAccess<Order>
    {
        private readonly IDatabaseContext databaseContext;

        public OrderDataAccess(IDatabaseContext databaseContext)
            => this.databaseContext = databaseContext;

        public async Task<Order> GetAsync(int id)
        {
            return await this.databaseContext.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).Include(x => x.Restaurant).Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            var entities = await this.databaseContext.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).Include(x => x.Restaurant).Include(x => x.Customer).Where(x => x.Completed == false).Select(x => x).AsNoTracking().ToListAsync();

            return entities;
        }

        public async Task<Order> SaveAsync(Order entity)
        {
            this.databaseContext.Orders.Add(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Order> UpdateAsync(Order entity)
        {
            this.databaseContext.Orders.Update(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.GetAsync(id);
            this.databaseContext.Orders.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result != 0);
        }
    }
}