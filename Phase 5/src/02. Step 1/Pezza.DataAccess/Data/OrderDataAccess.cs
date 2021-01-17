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

    public class OrderDataAccess : IDataAccess<OrderDTO>
    {
        private readonly IDatabaseContext databaseContext;

        private readonly IMapper mapper;

        public OrderDataAccess(IDatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<OrderDTO> GetAsync(int id)
            => this.mapper.Map<OrderDTO>(await this.databaseContext.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).Include(x => x.Restaurant).Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == id));

        public async Task<ListResult<OrderDTO>> GetAllAsync(Entity searchBase)
        {
            var searchModel = (OrderDTO)searchBase;
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
            var paged = this.mapper.Map<List<OrderDTO>>(await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync());

            return ListResult<OrderDTO>.Success(paged, count);
        }

        public async Task<OrderDTO> SaveAsync(OrderDTO entity)
        {
            this.databaseContext.Orders.Add(this.mapper.Map<Order>(entity));
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<OrderDTO> UpdateAsync(OrderDTO entity)
        {
            var findEntity = await this.databaseContext.Orders.FirstOrDefaultAsync(x => x.Id == entity.Id);
            findEntity.Completed = entity.Completed ?? findEntity.Completed;
            findEntity.RestaurantId = entity.RestaurantId ?? findEntity.RestaurantId;
            findEntity.CustomerId = entity.CustomerId ?? findEntity.CustomerId;

            this.databaseContext.Orders.Update(findEntity);
            await this.databaseContext.SaveChangesAsync();

            return this.mapper.Map<OrderDTO>(findEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.databaseContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
            this.databaseContext.Orders.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return result != 0;
        }
    }
}