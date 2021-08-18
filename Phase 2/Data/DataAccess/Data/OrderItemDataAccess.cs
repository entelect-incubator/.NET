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
    using Pezza.DataAccess.Contracts;

    public class OrderItemDataAccess : IDataAccess<OrderItemDTO>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public OrderItemDataAccess(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<OrderItemDTO> GetAsync(int id)
            => this.mapper.Map<OrderItemDTO>(await this.databaseContext.OrderItems.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<List<OrderItemDTO>> GetAllAsync()
        {
            var entities = await this.databaseContext.OrderItems.Select(x => x).AsNoTracking().ToListAsync();
            return this.mapper.Map<List<OrderItemDTO>>(entities);
        }

        public async Task<OrderItemDTO> SaveAsync(OrderItemDTO dto)
        {
            var entity = this.mapper.Map<OrderItem>(dto);
            this.databaseContext.OrderItems.Add(entity);
            await this.databaseContext.SaveChangesAsync();
            dto.Id = entity.Id;

            return dto;
        }

        public async Task<OrderItemDTO> UpdateAsync(OrderItemDTO dto)
        {
            var findEntity = await this.databaseContext.OrderItems.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (findEntity == null)
            {
                return null;
            }

            findEntity.Quantity = dto.Quantity ?? findEntity.Quantity;
            findEntity.ProductId = dto.ProductId ?? findEntity.ProductId;

            this.databaseContext.OrderItems.Update(findEntity);
            await this.databaseContext.SaveChangesAsync();

            return this.mapper.Map<OrderItemDTO>(findEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.databaseContext.OrderItems.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }

            this.databaseContext.OrderItems.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}