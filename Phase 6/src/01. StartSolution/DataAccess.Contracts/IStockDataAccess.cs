namespace DataAccess.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Entities;

    public interface IStockDataAccess
    {
        Task<Stock> GetAsync(int id);

        Task<List<Stock>> GetAllAsync();

        Task<Stock> UpdateAsync(Stock pizza);

        Task<Stock> SaveAsync(Stock pizza);

        Task<bool> DeleteAsync(int id);
    }
}