namespace Pezza.DataAccess.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Common.Extensions;
    using Pezza.Common.Filter;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class StockDataAccess : IDataAccess<Stock>
    {
        private readonly IDatabaseContext databaseContext;

        public StockDataAccess(IDatabaseContext databaseContext)
            => this.databaseContext = databaseContext;

        public async Task<Common.Entities.Stock> GetAsync(int id)
        {
            return await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ListResult<Stock>> GetAllAsync(StockDataDTO searchModel)
        {
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
                .FilterByDateCreated(searchModel.DateCreated)
                .FilterByComment(searchModel.Comment)

                .OrderBy(searchModel.OrderBy);

            var count = entities.Count();
            var paged = await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync();

            return ListResult<Stock>.Success(paged, count);
        }

        public async Task<Stock> SaveAsync(Stock entity)
        {
            this.databaseContext.Stocks.Add(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Stock> UpdateAsync(Stock entity)
        {
            this.databaseContext.Stocks.Update(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.GetAsync(id);
            this.databaseContext.Stocks.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}