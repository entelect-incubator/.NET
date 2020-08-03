namespace Pezza.DataAccess.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Pezza.Common.Entities;

    public interface INotifyDataAccess
    {
        Task<Notify> GetAsync(int id);

        Task<List<Notify>> GetAllAsync();

        Task<Notify> UpdateAsync(Notify entity);

        Task<Notify> SaveAsync(Notify entity);

        Task<bool> DeleteAsync(Notify entity);
    }
}