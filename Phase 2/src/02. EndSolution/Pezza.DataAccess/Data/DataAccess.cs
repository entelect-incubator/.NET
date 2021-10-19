namespace Pezza.DataAccess.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.Models.Base;
    using Pezza.DataAccess.Contracts;

    public class DataAccess<TEntity> : IDataAccess<TEntity> where TEntity : EntityBase
    {
        private readonly DatabaseContext databaseContext;        

        private DbSet<TEntity> dbSet;

        protected DbSet<TEntity> DbSet
            => this.dbSet ??= this.databaseContext.Set<TEntity>();

        public DataAccess(DatabaseContext databaseContext, IMapper mapper)
            => this.databaseContext = databaseContext;

        public async Task<TEntity> GetAsync(int id)
            => await this.dbSet.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<TEntity>> GetAllAsync(string[] includes)
        {
            var query = this.dbSet.Select(x => x).AsNoTracking();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task SaveAsync(TEntity entity)
            => await this.dbSet.AddAsync(entity);

        public void Update(TEntity entity)
            => this.dbSet.Update(entity);

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.dbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }

            this.dbSet.Remove(entity);

            return true;
        }

        public async Task<int> Complete()
            => await this.databaseContext.SaveChangesAsync();
    }
}