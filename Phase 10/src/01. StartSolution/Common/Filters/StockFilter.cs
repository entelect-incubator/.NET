namespace Common.Filters;

using System;
using System.Linq;
using Common.Entities;

public static class StockFilter
{
    public static IQueryable<Stock> FilterByName(this IQueryable<Stock> query, string name)
    {
        return string.IsNullOrWhiteSpace(name) ? query : query.Where(x => x.Name.Contains(name));
    }

    public static IQueryable<Stock> FilterByUnitOfMeasure(this IQueryable<Stock> query, string unitOfMeasure)
    {
        return string.IsNullOrWhiteSpace(unitOfMeasure) ? query : query.Where(x => x.UnitOfMeasure.Contains(unitOfMeasure));
    }

    public static IQueryable<Stock> FilterByValueOfMeasure(this IQueryable<Stock> query, double? valueOfMeasure)
    {
        return !valueOfMeasure.HasValue ? query : query.Where(x => x.ValueOfMeasure == valueOfMeasure.Value);
    }

    public static IQueryable<Stock> FilterByQuantity(this IQueryable<Stock> query, int? quantity)
    {
        return !quantity.HasValue ? query : query.Where(x => x.Quantity == quantity.Value);
    }

    public static IQueryable<Stock> FilterByExpiryDate(this IQueryable<Stock> query, DateTime? expiryDate)
    {
        return !expiryDate.HasValue ? query : query.Where(x => x.ExpiryDate == expiryDate.Value);
    }

    public static IQueryable<Stock> FilterByDateCreated(this IQueryable<Stock> query, DateTime? dateCreated)
    {
        return !dateCreated.HasValue ? query : query.Where(x => x.DateCreated == dateCreated.Value);
    }

    public static IQueryable<Stock> FilterByComment(this IQueryable<Stock> query, string comment)
    {
        return string.IsNullOrWhiteSpace(comment) ? query : query.Where(x => x.Comment.Contains(comment));
    }
}