namespace DataAccess.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Models.Base;

    public interface IDataAccess<TEntity> where TEntity : EntityBase
    {
        Task<TEntity> GetAsync(int id);

        Task<List<TEntity>> GetAllAsync(string[] includes);

        Task SaveAsync(TEntity entity);
        
        void Update(TEntity entity);

        Task<bool> DeleteAsync(int id);

        Task<int> Complete();
    }
}