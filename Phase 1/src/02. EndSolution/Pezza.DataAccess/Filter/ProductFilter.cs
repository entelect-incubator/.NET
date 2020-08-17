namespace Pezza.DataAccess.Filter
{
    using System.Linq;
    using Pezza.Common.Entities;

    public static class ProductFilter
    {
        public static IQueryable<Product> FilterByName(this IQueryable<Product> query, string name)
            => string.IsNullOrEmpty(name) ? query : query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
    }
}