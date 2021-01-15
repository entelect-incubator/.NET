namespace Pezza.DataAccess.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.Entities;
    using Pezza.DataAccess.Contracts;

    public class NotifyDataAccess : IDataAccess<Notify>
    {
        private readonly IDatabaseContext databaseContext;

        public NotifyDataAccess(IDatabaseContext databaseContext)
            => this.databaseContext = databaseContext;

        public async Task<Notify> GetAsync(int id)
        {
            return await this.databaseContext.Notify.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Notify>> GetAllAsync()
        {
            var entities = await this.databaseContext.Notify.Select(x => x).AsNoTracking().ToListAsync();

            return entities;
        }

        public async Task<Notify> SaveAsync(Notify entity)
        {
            this.databaseContext.Notify.Add(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Notify> UpdateAsync(Notify entity)
        {
            this.databaseContext.Notify.Update(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.GetAsync(id);
            this.databaseContext.Notify.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}