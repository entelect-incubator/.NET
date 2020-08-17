namespace Pezza.DataAccess.Filter
{
    using System.Linq;
    using Pezza.Common.Entities;

    public static class RestaurantFilter
    {
        public static IQueryable<Restaurant> FilterByName(this IQueryable<Restaurant> query, string name)
            => string.IsNullOrEmpty(name) ? query : query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
    }
}