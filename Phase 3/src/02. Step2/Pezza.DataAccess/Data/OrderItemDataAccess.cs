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
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class OrderItemDataAccess : IDataAccess<OrderItemDTO>
    {
        private readonly IDatabaseContext databaseContext;

        private readonly IMapper mapper;

        public OrderItemDataAccess(IDatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<OrderItemDTO> GetAsync(int id)
            => this.mapper.Map<OrderItemDTO>(await this.databaseContext.OrderItems.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<ListResult<OrderItemDTO>> GetAllAsync(Entity searchBase)
        {
            var searchModel = (OrderItemDTO)searchBase;
            var entities = this.mapper.Map<List<OrderItemDTO>>(await this.databaseContext.OrderItems.Select(x => x).AsNoTracking().ToListAsync());
            return ListResult<OrderItemDTO>.Success(entities, entities.Count);
        }

        public async Task<OrderItemDTO> SaveAsync(OrderItemDTO entity)
        {
            this.databaseContext.OrderItems.Add(this.mapper.Map<OrderItem>(entity));
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<OrderItemDTO> UpdateAsync(OrderItemDTO entity)
        {
            var findEntity = await this.databaseContext.OrderItems.FirstOrDefaultAsync(x => x.Id == entity.Id);
            findEntity.Quantity = entity.Quantity ?? findEntity.Quantity;
            findEntity.ProductId = entity.ProductId ?? findEntity.ProductId;

            this.databaseContext.OrderItems.Update(findEntity);
            await this.databaseContext.SaveChangesAsync();

            return this.mapper.Map<OrderItemDTO>(findEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.databaseContext.OrderItems.FirstOrDefaultAsync(x => x.Id == id);
            this.databaseContext.OrderItems.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}