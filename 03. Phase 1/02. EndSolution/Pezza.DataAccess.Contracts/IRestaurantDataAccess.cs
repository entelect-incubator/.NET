namespace Pezza.DataAccess.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Pezza.Common.Entities;

    public interface IRestaurantDataAccess
    {
        Task<Restaurant> GetAsync(int id);

        Task<List<Restaurant>> GetAllAsync();

        Task<Restaurant> UpdateAsync(Restaurant entity);

        Task<Restaurant> SaveAsync(Restaurant entity);

        Task<bool> DeleteAsync(Restaurant entity);
    }
}