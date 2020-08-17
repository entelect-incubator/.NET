namespace Pezza.DataAccess.Filter
{
    using System.Linq;
    using Pezza.Common.Entities;

    public static class OrderItemFilter
    {
        public static IQueryable<OrderItem> FilterByProductId(this IQueryable<OrderItem> query, int? productId)
            => productId != null ? query : query.Where(x => x.ProductId == productId);
    }
}