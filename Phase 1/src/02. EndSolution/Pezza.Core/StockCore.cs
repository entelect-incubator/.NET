namespace Pezza.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Core.Contracts;
    using Pezza.DataAccess.Contracts;

    public class StockCore : IStockCore
    {
        private readonly IStockDataAccess dataAccess;

        private readonly IMapper mapper;

        public StockCore(IStockDataAccess dataAccess, IMapper mapper)
            => (this.dataAccess, this.mapper) = (dataAccess, mapper);


        public async Task<StockDTO> GetAsync(int id)
            => this.mapper.Map<StockDTO>(await this.dataAccess.GetAsync(id));

        public async Task<IEnumerable<StockDTO>> GetAllAsync()
            => this.mapper.Map<List<StockDTO>>(await this.dataAccess.GetAllAsync());

        public async Task<StockDTO> SaveAsync(StockDTO stock)
            => this.mapper.Map<StockDTO>(await this.dataAccess.SaveAsync(this.mapper.Map<Stock>(stock)));

        public async Task<StockDTO> UpdateAsync(StockDTO stock)
        {
            var findEntity = await this.dataAccess.GetAsync(stock.Id);

            findEntity.Name = !string.IsNullOrEmpty(stock.Name) ? stock.Name : findEntity.Name;
            findEntity.UnitOfMeasure = !string.IsNullOrEmpty(stock.UnitOfMeasure) ? stock.UnitOfMeasure : findEntity.UnitOfMeasure;
            findEntity.ValueOfMeasure = stock.ValueOfMeasure ?? findEntity.ValueOfMeasure;
            findEntity.Quantity = stock.Quantity ?? findEntity.Quantity;
            findEntity.ExpiryDate = stock.ExpiryDate ?? findEntity.ExpiryDate;
            findEntity.Comment = stock.Comment;
            await this.dataAccess.UpdateAsync(findEntity);

            return this.mapper.Map<StockDTO>(findEntity);
        }

        public async Task<bool> DeleteAsync(int id)
            => await this.dataAccess.DeleteAsync(id);
    }
}