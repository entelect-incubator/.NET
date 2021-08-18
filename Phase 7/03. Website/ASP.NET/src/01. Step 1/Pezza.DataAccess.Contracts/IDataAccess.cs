namespace Pezza.DataAccess.Contracts
{
    using System.Threading.Tasks;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;

    public interface IDataAccess<T1>
    {
        Task<T1> GetAsync(int id);

        Task<ListResult<T1>> GetAllAsync(T1 searchBase);

        Task<T1> UpdateAsync(T1 entity);

        Task<T1> SaveAsync(T1 dto);

        Task<bool> DeleteAsync(int id);
    }
}