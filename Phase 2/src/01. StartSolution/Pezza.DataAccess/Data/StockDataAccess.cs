namespace Pezza.DataAccess.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.Entities;
    using Pezza.DataAccess.Contracts;

    public class StockDataAccess : IStockDataAccess
    {
        private readonly DatabaseContext databaseContext;

        public StockDataAccess(DatabaseContext databaseContext, IMapper mapper)
            => this.databaseContext = databaseContext;

        public async Task<Stock> GetAsync(int id)
            => await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<Stock>> GetAllAsync()
        {
            var entities = await this.databaseContext.Stocks.Select(x => x).AsNoTracking().ToListAsync();
            return entities;
        }

        public async Task<Stock> SaveAsync(Stock stock)
        {
            this.databaseContext.Stocks.Add(stock);
            await this.databaseContext.SaveChangesAsync();

            return stock;
        }

        public async Task<Stock> UpdateAsync(Stock stock)
        {
            this.databaseContext.Stocks.Update(stock);
            await this.databaseContext.SaveChangesAsync();

            return stock;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            this.databaseContext.Stocks.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }

        public async Task<bool> DeleteAsync(Stock stock)
        {
            this.databaseContext.Stocks.Remove(stock);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}