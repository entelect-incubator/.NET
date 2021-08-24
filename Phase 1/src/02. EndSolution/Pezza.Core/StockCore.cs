namespace Pezza.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Pezza.Common.DTO;
    using Pezza.Core.Contracts;
    using Pezza.DataAccess.Contracts;

    public class StockCore : IStockCore
    {
        private readonly IStockDataAccess DataAccess;

        public StockCore(IStockDataAccess DataAccess)
            => (this.DataAccess) = (DataAccess);

        public async Task<StockDTO> GetAsync(int id)
            => await this.dataAccess.GetAsync(id);

        public async Task<IEnumerable<StockDTO>> GetAllAsync()
            => await this.dataAccess.GetAllAsync();

        public async Task<StockDTO> SaveAsync(StockDTO dto)
            => await this.dataAccess.SaveAsync(dto);

        public async Task<StockDTO> UpdateAsync(StockDTO dto)
            => await this.dataAccess.UpdateAsync(dto);

        public async Task<bool> DeleteAsync(int id)
            => await this.DataAccess.DeleteAsync(id);
    }
}
