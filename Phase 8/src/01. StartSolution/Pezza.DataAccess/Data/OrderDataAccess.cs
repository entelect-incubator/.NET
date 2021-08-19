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
    using Pezza.Common.Filters;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class OrderDataAccess : IDataAccess<OrderDTO>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public OrderDataAccess(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<OrderDTO> GetAsync(int id)
            => this.mapper.Map<OrderDTO>(await this.databaseContext.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).Include(x => x.Restaurant).Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == id));

        public async Task<ListResult<OrderDTO>> GetAllAsync(OrderDTO dto)
        {
            var entities = this.databaseContext.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .Include(x => x.Restaurant)
                .Include(x => x.Customer)
                .AsNoTracking()
                .FilterByCustomerId(dto.CustomerId)
                .FilterByRestaurantId(dto.RestaurantId)
                .FilterByAmount(dto.Amount)
                .FilterByCompleted(dto.Completed);

            var count = entities.Count();
            var paged = this.mapper.Map<List<OrderDTO>>(await entities.ApplyPaging(dto.PagingArgs).ToListAsync());

            return ListResult<OrderDTO>.Success(paged, count);
        }

        public async Task<OrderDTO> SaveAsync(OrderDTO dto)
        {
            var entity = this.mapper.Map<Order>(dto);

            this.databaseContext.Orders.Add(entity);
            await this.databaseContext.SaveChangesAsync();
            dto.Id = entity.Id;

            return dto;
        }

        public async Task<OrderDTO> UpdateAsync(OrderDTO dto)
        {
            var findEntity = await this.databaseContext.Orders.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (findEntity == null)
            {
                return null;
            }

            findEntity.Completed = dto.Completed ?? findEntity.Completed;
            findEntity.RestaurantId = dto.RestaurantId ?? findEntity.RestaurantId;
            findEntity.CustomerId = dto.CustomerId ?? findEntity.CustomerId;
            findEntity.Amount = dto.Amount ?? findEntity.Amount;

            this.databaseContext.Orders.Update(findEntity);
            await this.databaseContext.SaveChangesAsync();

            return this.mapper.Map<OrderDTO>(findEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.databaseContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }

            this.databaseContext.Orders.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return result != 0;
        }
    }
}