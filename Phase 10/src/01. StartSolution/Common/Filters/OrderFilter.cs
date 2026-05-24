namespace Common.Filters;

using System;
using System.Linq;
using Common.Entities;

public static class OrderFilter
{
    public static IQueryable<Order> FilterByCustomerId(this IQueryable<Order> query, int? customerId)
    {
        return !customerId.HasValue ? query : query.Where(x => x.CustomerId == customerId.Value);
    }

    public static IQueryable<Order> FilterByRestaurantId(this IQueryable<Order> query, int? restaurantId)
    {
        return !restaurantId.HasValue ? query : query.Where(x => x.RestaurantId == restaurantId.Value);
    }

    public static IQueryable<Order> FilterByAmount(this IQueryable<Order> query, decimal? amount)
    {
        return !amount.HasValue ? query : query.Where(x => x.Amount == amount.Value);
    }

    public static IQueryable<Order> FilterByDateCreated(this IQueryable<Order> query, DateTime? dateCreated)
    {
        return !dateCreated.HasValue ? query : query.Where(x => x.DateCreated == dateCreated.Value);
    }

    public static IQueryable<Order> FilterByCompleted(this IQueryable<Order> query, bool? completed)
    {
        return !completed.HasValue ? query : query.Where(x => x.Completed == completed.Value);
    }
}