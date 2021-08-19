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

    public class StockDataAccess : IDataAccess<StockDTO>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public StockDataAccess(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<StockDTO> GetAsync(int id)
            => this.mapper.Map<StockDTO>(await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<ListResult<StockDTO>> GetAllAsync(StockDTO dto)
        {
            if (string.IsNullOrEmpty(dto.OrderBy))
            {
                dto.OrderBy = "DateCreated desc";
            }

            var entities = this.databaseContext.Stocks.Select(x => x)
                .AsNoTracking()
                .FilterByName(dto.Name)
                .FilterByUnitOfMeasure(dto.UnitOfMeasure)
                .FilterByValueOfMeasure(dto.ValueOfMeasure)
                .FilterByQuantity(dto.Quantity)
                .FilterByExpiryDate(dto.ExpiryDate)
                .FilterByComment(dto.Comment)

                .OrderBy(dto.OrderBy);

            var count = entities.Count();
            var paged = this.mapper.Map<List<StockDTO>>(await entities.ApplyPaging(dto.PagingArgs).ToListAsync());

            return ListResult<StockDTO>.Success(paged, count);
        }

        public async Task<StockDTO> SaveAsync(StockDTO dto)
        {
            var entity = this.mapper.Map<Stock>(dto);
            this.databaseContext.Stocks.Add(entity);
            await this.databaseContext.SaveChangesAsync();
            dto.Id = entity.Id;

            return dto;
        }

        public async Task<StockDTO> UpdateAsync(StockDTO dto)
        {
            var findEntity = await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (findEntity == null)
            {
                return null;
            }

            findEntity.Name = !string.IsNullOrEmpty(dto.Name) ? dto.Name : findEntity.Name;
            findEntity.UnitOfMeasure = !string.IsNullOrEmpty(dto.UnitOfMeasure) ? dto.UnitOfMeasure : findEntity.UnitOfMeasure;
            findEntity.ValueOfMeasure = dto.ValueOfMeasure ?? findEntity.ValueOfMeasure;
            findEntity.Quantity = dto.Quantity ?? findEntity.Quantity;
            findEntity.ExpiryDate = dto.ExpiryDate ?? findEntity.ExpiryDate;
            findEntity.Comment = dto.Comment;
            this.databaseContext.Stocks.Update(findEntity);
            await this.databaseContext.SaveChangesAsync();

            return this.mapper.Map<StockDTO>(findEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }

            this.databaseContext.Stocks.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}