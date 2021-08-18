namespace Pezza.DataAccess.Contracts
{
    using System.Threading.Tasks;
    using Pezza.Common.Models;

    public interface IDataAccess<T>
    {
        Task<T> GetAsync(int id);

        Task<ListResult<T>> GetAllAsync(T dto);

        Task<T> UpdateAsync(T dto);

        Task<T> SaveAsync(T dto);

        Task<bool> DeleteAsync(int id);
    }
}