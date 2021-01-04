namespace Pezza.DataAccess.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.Entities;
    using Pezza.DataAccess.Contracts;

    public class CustomerDataAccess : IDataAccess<Customer>
    {
        private readonly IDatabaseContext databaseContext;

        public CustomerDataAccess(IDatabaseContext databaseContext)
            => this.databaseContext = databaseContext;

        public async Task<Customer> GetAsync(int id)
        {
            return await this.databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            var entities = await this.databaseContext.Customers.Select(x => x).AsNoTracking().ToListAsync();

            return entities;
        }

        public async Task<Customer> SaveAsync(Customer entity)
        {
            this.databaseContext.Customers.Add(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Customer> UpdateAsync(Customer entity)
        {
            this.databaseContext.Customers.Update(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.GetAsync(id);
            this.databaseContext.Customers.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}