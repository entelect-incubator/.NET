namespace Pezza.DataAccess.Data
{
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.DTO.Data;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class OrderItemDataAccess : IDataAccess<OrderItem>
    {
        private readonly IDatabaseContext databaseContext;

        public OrderItemDataAccess(IDatabaseContext databaseContext)
            => this.databaseContext = databaseContext;

        public async Task<OrderItem> GetAsync(int id)
        {
            return await this.databaseContext.OrderItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ListResult<OrderItem>> GetAllAsync(SearchBase searchBase)
        {
            var searchModel = (OrderItemDataDTO)searchBase;
            var entities = await this.databaseContext.OrderItems.Select(x => x).AsNoTracking().ToListAsync();

            return ListResult<OrderItem>.Success(entities, entities.Count);
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

            return result == 1;
        }
    }
}