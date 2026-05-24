namespace Common.Filters;

using System;
using System.Linq;
using Common.Entities;

public static class NotifyFilter
{
    public static IQueryable<Notify> FilterByCustomerId(this IQueryable<Notify> query, int? customerId)
    {
        return !customerId.HasValue ? query : query.Where(x => x.CustomerId == customerId.Value);
    }

    public static IQueryable<Notify> FilterByEmail(this IQueryable<Notify> query, string email)
    {
        return string.IsNullOrWhiteSpace(email) ? query : query.Where(x => x.Email.Contains(email));
    }

    public static IQueryable<Notify> FilterBySent(this IQueryable<Notify> query, bool? sent)
    {
        return !sent.HasValue ? query : query.Where(x => x.Sent == sent.Value);
    }

    public static IQueryable<Notify> FilterByRetry(this IQueryable<Notify> query, int? retry)
    {
        return !retry.HasValue ? query : query.Where(x => x.Retry == retry.Value);
    }

    public static IQueryable<Notify> FilterByDateSent(this IQueryable<Notify> query, DateTime? dateSent)
    {
        return !dateSent.HasValue ? query : query.Where(x => x.DateSent == dateSent.Value);
    }
}