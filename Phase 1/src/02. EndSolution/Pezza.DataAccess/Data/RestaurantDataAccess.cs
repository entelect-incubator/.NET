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

    public class RestaurantDataAccess : IRestaurantDataAccess
    {
        private readonly IDatabaseContext databaseContext;

        public RestaurantDataAccess(IDatabaseContext databaseContext) 
            => this.databaseContext = databaseContext;

        public async Task<Common.Entities.Restaurant> GetAsync(int id)
        {
            return await this.databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Common.Entities.Restaurant>> GetAllAsync(RestaurantSearchModel searchModel)
        {
            if (string.IsNullOrEmpty(searchModel.OrderBy))
            {
                searchModel.OrderBy = "DateCreated desc";
            }

            var entities = this.databaseContext.Restaurants.Select(x => x)
                .AsNoTracking()
                .OrderBy(searchModel.OrderBy);

            var paged = await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync();

            return paged;
        }

        public async Task<Common.Entities.Restaurant> SaveAsync(Common.Entities.Restaurant entity)
        {
            this.databaseContext.Restaurants.Add(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Common.Entities.Restaurant> UpdateAsync(Common.Entities.Restaurant entity)
        {
            this.databaseContext.Restaurants.Update(entity);
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(Common.Entities.Restaurant entity)
        {
            this.databaseContext.Restaurants.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();
            return (result == 1);
        }
    }
}
