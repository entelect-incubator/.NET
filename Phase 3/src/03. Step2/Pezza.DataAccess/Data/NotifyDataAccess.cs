namespace Pezza.DataAccess.Data
{
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Common.Extensions;
    using Pezza.Common.Filter;
    using Pezza.Common.Models;
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

        public async Task<ListResult<Notify>> GetAllAsync(NotifyDataDTO searchModel)
        {
            if (string.IsNullOrEmpty(searchModel.OrderBy))
            {
                searchModel.OrderBy = "DateCreated desc";
            }

            var entities = this.databaseContext.Notify.Select(x => x)
                .AsNoTracking()
                .FilterByCustomerId(searchModel.CustomerId)
                .FilterByEmail(searchModel.Email)
                .FilterBySent(searchModel.Sent)
                .FilterByRetry(searchModel.Retry)
                .FilterByDateSent(searchModel.DateSent)

                .OrderBy(searchModel.OrderBy);

            var count = entities.Count();
            var paged = await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync();

            return ListResult<Notify>.Success(paged, count);
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