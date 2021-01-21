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

    public class StockDataAccess : IDataAccess<StockDTO>
    {
        private readonly IDatabaseContext databaseContext;

        private readonly IMapper mapper;

        public StockDataAccess(IDatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<StockDTO> GetAsync(int id)
            => this.mapper.Map<StockDTO>(await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<ListResult<StockDTO>> GetAllAsync(Entity searchBase)
        {
            var searchModel = (StockDTO)searchBase;
            if (string.IsNullOrEmpty(searchModel.OrderBy))
            {
                searchModel.OrderBy = "DateCreated desc";
            }

            var entities = this.databaseContext.Stocks.Select(x => x)
                .AsNoTracking()
                .FilterByName(searchModel.Name)
                .FilterByUnitOfMeasure(searchModel.UnitOfMeasure)
                .FilterByValueOfMeasure(searchModel.ValueOfMeasure)
                .FilterByQuantity(searchModel.Quantity)
                .FilterByExpiryDate(searchModel.ExpiryDate)
                .FilterByComment(searchModel.Comment)

                .OrderBy(searchModel.OrderBy);

            var count = entities.Count();
            var paged = this.mapper.Map<List<StockDTO>>(await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync());

            return ListResult<StockDTO>.Success(paged, count);
        }

        public async Task<StockDTO> SaveAsync(StockDTO entity)
        {
            this.databaseContext.Stocks.Add(this.mapper.Map<Stock>(entity));
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<StockDTO> UpdateAsync(StockDTO entity)
        {
            var findEntity = await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == entity.Id);

            findEntity.Name = !string.IsNullOrEmpty(entity.Name) ? entity.Name : findEntity.Name;
            findEntity.UnitOfMeasure = !string.IsNullOrEmpty(entity.UnitOfMeasure) ? entity.UnitOfMeasure : findEntity.UnitOfMeasure;
            findEntity.ValueOfMeasure = entity.ValueOfMeasure ?? findEntity.ValueOfMeasure;
            findEntity.Quantity = entity.Quantity ?? findEntity.Quantity;
            findEntity.ExpiryDate = entity.ExpiryDate ?? findEntity.ExpiryDate;
            findEntity.Comment = entity.Comment;
            this.databaseContext.Stocks.Update(findEntity);
            await this.databaseContext.SaveChangesAsync();

            return this.mapper.Map<StockDTO>(findEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            this.databaseContext.Stocks.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return result == 1;
        }
    }
}