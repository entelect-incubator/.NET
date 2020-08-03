namespace Pezza.DataAccess.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Pezza.Common.Entities;

    public interface IOrderDataAccess
    {
        Task<Order> GetAsync(int id);

        Task<List<Order>> GetAllAsync();

        Task<Order> UpdateAsync(Order entity);

        Task<Order> SaveAsync(Order entity);

        Task<bool> DeleteAsync(Order entity);
    }
}