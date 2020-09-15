namespace Pezza.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Pezza.Common.DTO;
    using Pezza.Common.Mapping;
    using Pezza.Core.Contracts;
    using Pezza.DataAccess.Contracts;

    public class StockCore : IStockCore
    {
        private readonly IStockDataAccess dataAcess;

        public StockCore(IStockDataAccess dataAcess) => this.dataAcess = dataAcess;

        public async Task<StockDTO> GetAsync(int id)
        {
            var search = await this.dataAcess.GetAsync(id);

            return search.Map();
        }

        public async Task<IEnumerable<StockDTO>> GetAllAsync()
        {
            var search = await this.dataAcess.GetAllAsync();

            return search.Map();
        }

        public async Task<StockDTO> SaveAsync(StockDTO model)
        {

            var outcome = await this.dataAcess.SaveAsync(model.Map());

            return outcome.Map();
        }

        public async Task<StockDTO> UpdateAsync(StockDTO model)
        {

            var entity = await this.dataAcess.GetAsync(model.Id);

            if (!string.IsNullOrEmpty(model.Name))
            {
                entity.Name = model.Name;
            }

            if (!string.IsNullOrEmpty(model.UnitOfMeasure))
            {
                entity.UnitOfMeasure = model.UnitOfMeasure;
            }

            if (model.ValueOfMeasure.HasValue)
            {
                entity.ValueOfMeasure = model.ValueOfMeasure;
            }

            if (model.Quantity.HasValue)
            {
                entity.Quantity = model.Quantity.Value;
            }

            if (model.ExpiryDate.HasValue)
            {
                entity.ExpiryDate = model.ExpiryDate;
            }

            if (!string.IsNullOrEmpty(model.Comment))
            {
                entity.Comment = model.Comment;
            }

            var outcome = await this.dataAcess.UpdateAsync(entity);
            return outcome.Map();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var outcome = await this.dataAcess.DeleteAsync(id); 
            
            return outcome;
        }
    }
}
