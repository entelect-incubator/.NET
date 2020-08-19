namespace Pezza.DataAccess.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.Extensions;
    using Pezza.Common.Models.SearchModels;
    using Pezza.DataAccess.Contracts;

    public class NotifyDataAccess : INotifyDataAccess
    {
        private readonly IDatabaseContext databaseContext;

        public NotifyDataAccess(IDatabaseContext databaseContext)
            => this.databaseContext = databaseContext;

        public async Task<Common.Entities.Notify> GetAsync(int id)
        {
            return await this.databaseContext.Notifies.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Common.Entities.Notify>> GetAllAsync(NotifySearchModel searchModel)
        {
            if (string.IsNullOrEmpty(searchModel.OrderBy))
            {
                searchModel.OrderBy = "DateSent desc";
            }

            var entities = this.databaseContext.Notifies.Select(x => x)
                .AsNoTracking()
                .Include(i => i.Customer)
                .OrderBy(searchModel.OrderBy);

            var paged = await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync();

            return paged;
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
