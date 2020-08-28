namespace Pezza.DataAccess.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Pezza.Common.Entities;
    using Pezza.Common.Models.SearchModels;

    public interface IRestaurantDataAccess
    {
        Task<Restaurant> GetAsync(int id);

        Task<List<Restaurant>> GetAllAsync(RestaurantSearchModel searchModel);

        Task<Restaurant> UpdateAsync(Restaurant entity);

        Task<Restaurant> SaveAsync(Restaurant entity);

        Task<bool> DeleteAsync(Restaurant entity);
    }
}