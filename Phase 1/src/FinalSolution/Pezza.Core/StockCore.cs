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
        private readonly IStockDataAccess DataAccess;

        private readonly IMapper mapper;

        public StockCore(IStockDataAccess DataAccess, IMapper mapper)
            => (this.DataAccess, this.mapper) = (DataAccess, mapper);

        public async Task<StockDTO> GetAsync(int id)
        {
            var search = await this.DataAccess.GetAsync(id);

            return this.mapper.Map<StockDTO>(search);
        }

        public async Task<IEnumerable<StockDTO>> GetAllAsync()
        {
            var search = await this.DataAccess.GetAllAsync();

            return this.mapper.Map<List<StockDTO>>(search);
        }

        public async Task<StockDTO> SaveAsync(StockDTO dto)
        {
            var entity = this.mapper.Map<Stock>(dto);
            var outcome = await this.DataAccess.SaveAsync(entity);
            dto.Id = entity.Id;

            return dto;
        }

        public async Task<StockDTO> UpdateAsync(StockDTO model)
        {

            var entity = await this.DataAccess.GetAsync(model.Id);

            entity.Name = !string.IsNullOrEmpty(model.Name) ? model.Name : entity.Name;
            entity.UnitOfMeasure = !string.IsNullOrEmpty(model.UnitOfMeasure) ? model.UnitOfMeasure : entity.UnitOfMeasure;
            entity.UnitOfMeasure = !string.IsNullOrEmpty(model.UnitOfMeasure) ? model.UnitOfMeasure : entity.UnitOfMeasure;
            entity.ValueOfMeasure = (model.ValueOfMeasure.HasValue) ? model.ValueOfMeasure : entity.ValueOfMeasure;
            entity.Quantity = (model.Quantity.HasValue) ? model.Quantity.Value : entity.Quantity;
            entity.ExpiryDate = (model.ExpiryDate.HasValue) ? model.ExpiryDate : entity.ExpiryDate;
            entity.Comment = (!string.IsNullOrEmpty(model.Comment)) ? model.Comment : entity.Comment;

            var outcome = await this.DataAccess.UpdateAsync(entity);
            return this.mapper.Map<StockDTO>(outcome);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var outcome = await this.DataAccess.DeleteAsync(id);

            return outcome;
        }
    }
}
