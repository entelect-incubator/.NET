namespace Pezza.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Core.Contracts;
    using Pezza.DataAccess;

    public class StockCore : IStockCore
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public StockCore(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<StockDTO> GetAsync(int id)
            => this.mapper.Map<StockDTO>(await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<IEnumerable<StockDTO>> GetAllAsync()
        {
            var entities = await this.databaseContext.Stocks.Select(x => x).AsNoTracking().ToListAsync();
            return this.mapper.Map<List<StockDTO>>(entities);
        }

        public async Task<StockDTO> SaveAsync(StockDTO stock)
        {
            var entity = this.mapper.Map<Stock>(stock);
            this.databaseContext.Stocks.Add(entity);
            await this.databaseContext.SaveChangesAsync();
            return this.mapper.Map<StockDTO>(entity);
        }

        public async Task<StockDTO> UpdateAsync(StockDTO stock)
        {
            var findEntity = await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == stock.Id);

            findEntity.Name = !string.IsNullOrEmpty(stock.Name) ? stock.Name : findEntity.Name;
            findEntity.UnitOfMeasure = !string.IsNullOrEmpty(stock.UnitOfMeasure) ? stock.UnitOfMeasure : findEntity.UnitOfMeasure;
            findEntity.ValueOfMeasure = stock.ValueOfMeasure ?? findEntity.ValueOfMeasure;
            findEntity.Quantity = stock.Quantity ?? findEntity.Quantity;
            findEntity.ExpiryDate = stock.ExpiryDate ?? findEntity.ExpiryDate;
            findEntity.Comment = stock.Comment;
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