namespace Pezza.DataAccess.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Pezza.Common.Entities;
    using Pezza.Common.Models.SearchModels;

    public interface IProductDataAccess
    {
        Task<Product> GetAsync(int id);

        Task<List<Product>> GetAllAsync(ProductSearchModel searchModel);

        Task<Product> UpdateAsync(Product entity);

        Task<Product> SaveAsync(Product entity);

        Task<bool> DeleteAsync(Product entity);
    }
}