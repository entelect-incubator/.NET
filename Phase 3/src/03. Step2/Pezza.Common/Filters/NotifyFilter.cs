namespace Pezza.Common.Filters;

using System;
using System.Linq;
using Pezza.Common.Entities;

public static class NotifyFilter
{
    public static IQueryable<Notify> FilterByCustomerId(this IQueryable<Notify> query, int? customerId)
    {
        if (!customerId.HasValue)
        {
            return query;
        }

        return query.Where(x => x.CustomerId == customerId.Value);
    }

    public static IQueryable<Notify> FilterByEmail(this IQueryable<Notify> query, string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return query;
        }

        return query.Where(x => x.Email.Contains(email));
    }

    public static IQueryable<Notify> FilterBySent(this IQueryable<Notify> query, bool? sent)
    {
        if (!sent.HasValue)
        {
            return query;
        }

        return query.Where(x => x.Sent == sent.Value);
    }

    public static IQueryable<Notify> FilterByRetry(this IQueryable<Notify> query, int? retry)
    {
        if (!retry.HasValue)
        {
            return query;
        }

        return query.Where(x => x.Retry == retry.Value);
    }

    public static IQueryable<Notify> FilterByDateSent(this IQueryable<Notify> query, DateTime? dateSent)
    {
        if (!dateSent.HasValue)
        {
            return query;
        }

        return query.Where(x => x.DateSent == dateSent.Value);
    }


}