namespace Pezza.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.DataAccess.Contracts;

    public class NotifyDataAccess : INotifyDataAccess
    {
        private readonly IDatabaseContext databaseContext;

        public NotifyDataAccess(IDatabaseContext databaseContext) => this.databaseContext = databaseContext;

        public async Task<Common.Entities.Notify> GetAsync(int id)
        {
            return await this.databaseContext.Notifies.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Common.Entities.Notify>> GetAllAsync(int id)
        {
            return await this.databaseContext.Notifies.ToListAsync();
        }

        public async Task<Common.Entities.Notify> SaveAsync(Common.Entities.Notify entity)
        {
            this.databaseContext.Notifies.Add(entity);
            await this.databaseContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Common.Entities.Notify> UpdateAsync(Common.Entities.Notify entity)
        {
            this.databaseContext.Notifies.Update(entity);
            await this.databaseContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Common.Entities.Notify entity)
        {
            this.databaseContext.Notifies.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();
            return (result == 1);
        }
    }
}
