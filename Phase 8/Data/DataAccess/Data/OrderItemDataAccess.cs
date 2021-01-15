namespace Pezza.DataAccess.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.Entities;
    using Pezza.DataAccess.Contracts;

    public class OrderItemDataAccess : IDataAccess<OrderItem>
    {
        private readonly IDatabaseContext databaseContext;

        public OrderItemDataAccess(IDatabaseContext databaseContext)
            => this.databaseContext = databaseContext;

        public async Task<Common.Entities.OrderItem> GetAsync(int id)
        {
            return await this.databaseContext.OrderItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<OrderItem>> GetAllAsync()
        {
            var entities = await this.databaseContext.OrderItems.Select(x => x).AsNoTracking().ToListAsync();

            return entities;
        }

        public async Task<OrderItem> SaveAsync(OrderItem entity)
        {
            this.databaseContext.OrderItems.Add(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<OrderItem> UpdateAsync(OrderItem entity)
        {
            this.databaseContext.OrderItems.Update(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.GetAsync(id);
            this.databaseContext.OrderItems.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}