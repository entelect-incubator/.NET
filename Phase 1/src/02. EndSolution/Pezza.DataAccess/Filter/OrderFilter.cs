namespace Pezza.DataAccess.Filter
{
    using System.Linq;
    using Pezza.Common.Entities;

    public static class OrderFilter
    {
        public static IQueryable<Order> FilterByCustomerId(this IQueryable<Order> query, int? customerId)
            => customerId != null ? query : query.Where(x => x.CustomerId == customerId);
    }
}