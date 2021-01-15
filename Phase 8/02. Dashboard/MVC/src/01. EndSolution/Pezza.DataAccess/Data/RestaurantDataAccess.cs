namespace Pezza.DataAccess.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.Entities;
    using Pezza.DataAccess.Contracts;

    public class RestaurantDataAccess : IDataAccess<Restaurant>
    {
        private readonly IDatabaseContext databaseContext;

        public RestaurantDataAccess(IDatabaseContext databaseContext)
            => this.databaseContext = databaseContext;

        public async Task<Common.Entities.Restaurant> GetAsync(int id)
        {
            return await this.databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Restaurant>> GetAllAsync()
        {
            var entities = await this.databaseContext.Restaurants.Select(x => x).AsNoTracking().ToListAsync();

            return entities;
        }

        public async Task<Restaurant> SaveAsync(Restaurant entity)
        {
            this.databaseContext.Restaurants.Add(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Restaurant> UpdateAsync(Restaurant entity)
        {
            this.databaseContext.Restaurants.Update(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.GetAsync(id);
            this.databaseContext.Restaurants.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}