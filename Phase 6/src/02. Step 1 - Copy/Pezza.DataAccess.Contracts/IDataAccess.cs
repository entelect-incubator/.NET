namespace Pezza.DataAccess.Contracts
{
    using System.Threading.Tasks;
    using Pezza.Common.DTO.Data;
    using Pezza.Common.Models;

    public interface IDataAccess<T>
    {
        Task<T> GetAsync(int id);

        Task<ListResult<T>> GetAllAsync(SearchBase searchModel);

        Task<T> UpdateAsync(T entity);

        Task<T> SaveAsync(T entity);

        Task<bool> DeleteAsync(int id);
    }
}