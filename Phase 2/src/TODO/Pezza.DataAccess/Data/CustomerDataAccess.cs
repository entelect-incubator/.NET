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

    public class CustomerDataAccess : ICustomerDataAccess
    {
        private readonly IDatabaseContext databaseContext;

        public CustomerDataAccess(IDatabaseContext databaseContext)
            => this.databaseContext = databaseContext;

        public async Task<Common.Entities.Customer> GetAsync(int id)
        {
            return await this.databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Common.Entities.Customer>> GetAllAsync(CustomerSearchModel searchModel)
        {
            if (string.IsNullOrEmpty(searchModel.OrderBy))
            {
                searchModel.OrderBy = "DateCreated desc";
            }

            var entities = this.databaseContext.Customers.Select(x => x)
                .AsNoTracking()
                .OrderBy(searchModel.OrderBy);

            var paged = await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync();

            return paged;
        }

        public async Task<Common.Entities.Customer> SaveAsync(Common.Entities.Customer entity)
        {
            this.databaseContext.Customers.Add(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Common.Entities.Customer> UpdateAsync(Common.Entities.Customer entity)
        {
            this.databaseContext.Customers.Update(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(Common.Entities.Customer entity)
        {
            this.databaseContext.Customers.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}
