namespace Pezza.DataAccess.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Pezza.Common.Entities;
    using Pezza.Common.Models.SearchModels;

    public interface ICustomerDataAccess
    {
        Task<Customer> GetAsync(int id);

        Task<List<Customer>> GetAllAsync(CustomerSearchModel searchModel);

        Task<Customer> UpdateAsync(Customer entity);

        Task<Customer> SaveAsync(Customer entity);

        Task<bool> DeleteAsync(Customer entity);
    }
}