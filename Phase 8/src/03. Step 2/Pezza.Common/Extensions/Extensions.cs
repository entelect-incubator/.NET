namespace Pezza.Common.Extensions;

using System.Collections.Generic;
using System.Linq;
using Pezza.Common.Models;

public static class Extensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagingArgs pagingArgs)
    {
        var myPagingArgs = pagingArgs;

        if (pagingArgs == null)
        {
            myPagingArgs = PagingArgs.Default;
        }

        return myPagingArgs.UsePaging ? query.Skip(myPagingArgs.Offset).Take(myPagingArgs.Limit) : query;
    }

    public static IQueryable<T> ApplyPaging<T>(this List<T> query, PagingArgs pagingArgs)
    {
        var myPagingArgs = pagingArgs;

        if (pagingArgs == null)
        {
            myPagingArgs = PagingArgs.Default;
        }

        return query.ApplyPaging(myPagingArgs);
    }
}
