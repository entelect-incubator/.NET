namespace Pezza.DataAccess.Data
{
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.DTO.Data;
    using Pezza.Common.Entities;
    using Pezza.Common.Extensions;
    using Pezza.Common.Filter;
    using Pezza.Common.Models;
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

        public async Task<ListResult<Order>> GetAllAsync(SearchBase searchBase)
        {
            var searchModel = (OrderDataDTO)searchBase;
            var entities = this.databaseContext.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .Include(x => x.Restaurant)
                .Include(x => x.Customer)
                .AsNoTracking()
                .FilterByCustomerId(searchModel.CustomerId)
                .FilterByRestaurantId(searchModel.RestaurantId)
                .FilterByAmount(searchModel.Amount)
                .FilterByDateCreated(searchModel.DateCreated)
                .FilterByCompleted(searchModel.Completed)

                .OrderBy(searchModel.OrderBy);

            var count = entities.Count();
            var paged = await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync();

            return ListResult<Order>.Success(paged, count);
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

            return result != 0;
        }
    }
}