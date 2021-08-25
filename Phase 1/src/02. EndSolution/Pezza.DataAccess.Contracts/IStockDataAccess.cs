namespace Pezza.DataAccess.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Pezza.Common.DTO;

    public interface IStockDataAccess
    {
        Task<StockDTO> GetAsync(int id);

        Task<List<StockDTO>> GetAllAsync();

        Task<StockDTO> UpdateAsync(StockDTO dto);

        Task<StockDTO> SaveAsync(StockDTO dto);

        Task<bool> DeleteAsync(int id);
    }
}