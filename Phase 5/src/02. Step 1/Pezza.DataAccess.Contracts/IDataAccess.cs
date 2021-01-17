namespace Pezza.DataAccess.Contracts
{
    using System.Threading.Tasks;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;

    public interface IDataAccess<T>
    {
        Task<T> GetAsync(int id);

        Task<ListResult<T>> GetAllAsync(Entity searchBase);

        Task<T> UpdateAsync(T entity);

        Task<T> SaveAsync(T dto);

        Task<bool> DeleteAsync(int id);
    }
}