namespace Pezza.DataAccess.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.Extensions;
    using Pezza.Common.Models.SearchModels;
    using Pezza.DataAccess.Contracts;

    public class OrderDataAccess : IOrderDataAccess
    {
        private readonly IDatabaseContext databaseContext;

        public OrderDataAccess(IDatabaseContext databaseContext)
            => this.databaseContext = databaseContext;

        public async Task<Common.Entities.Order> GetAsync(int id) => await this.databaseContext.Orders
            .Include(i => i.OrderItems)
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<Common.Entities.Order>> GetAllAsync(OrderSearchModel searchModel)
        {
            if (string.IsNullOrEmpty(searchModel.OrderBy))
            {
                searchModel.OrderBy = "DateCreated desc";
            }

            var entities = this.databaseContext.Orders.Select(x => x)
                .AsNoTracking()
                .OrderBy(searchModel.OrderBy)
                .Include(i => i.Restaurant)
                .Include(i => i.Customer)
                .Include(i => i.OrderItems)
                .ThenInclude(i => i.Product);

            var paged = await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync();

            return paged;
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
