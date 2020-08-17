namespace Pezza.DataAccess.Filter
{
    using System.Linq;
    using Pezza.Common.Entities;

    public static class StockFilter
    {
        public static IQueryable<Stock> FilterByName(this IQueryable<Stock> query, string name)
            => string.IsNullOrEmpty(name) ? query : query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
    }
}