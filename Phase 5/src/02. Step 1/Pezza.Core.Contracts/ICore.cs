namespace Pezza.Core.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICore<T>
    {
        Task<T> GetAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> UpdateAsync(T model);

        Task<T> SaveAsync(T model);

        Task<bool> DeleteAsync(int id);
    }
}
