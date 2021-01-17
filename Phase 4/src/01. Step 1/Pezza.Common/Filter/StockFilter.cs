namespace Pezza.Common.Filter
{
    using System;
    using System.Linq;
    using Pezza.Common.Entities;

    public static class StockFilter
    {
        public static IQueryable<Stock> FilterByName(this IQueryable<Stock> query, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return query;
            }

            return query.Where(x => x.Name.Contains(name));
        }

        public static IQueryable<Stock> FilterByUnitOfMeasure(this IQueryable<Stock> query, string unitOfMeasure)
        {
            if (string.IsNullOrWhiteSpace(unitOfMeasure))
            {
                return query;
            }

            return query.Where(x => x.UnitOfMeasure.Contains(unitOfMeasure));
        }

        public static IQueryable<Stock> FilterByValueOfMeasure(this IQueryable<Stock> query, double? valueOfMeasure)
        {
            if (!valueOfMeasure.HasValue)
            {
                return query;
            }

            return query.Where(x => x.ValueOfMeasure == valueOfMeasure.Value);
        }

        public static IQueryable<Stock> FilterByQuantity(this IQueryable<Stock> query, int? quantity)
        {
            if (!quantity.HasValue)
            {
                return query;
            }

            return query.Where(x => x.Quantity == quantity.Value);
        }

        public static IQueryable<Stock> FilterByExpiryDate(this IQueryable<Stock> query, DateTime? expiryDate)
        {
            if (!expiryDate.HasValue)
            {
                return query;
            }

            return query.Where(x => x.ExpiryDate == expiryDate.Value);
        }

        public static IQueryable<Stock> FilterByDateCreated(this IQueryable<Stock> query, DateTime? dateCreated)
        {
            if (!dateCreated.HasValue)
            {
                return query;
            }

            return query.Where(x => x.DateCreated == dateCreated.Value);
        }

        public static IQueryable<Stock> FilterByComment(this IQueryable<Stock> query, string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                return query;
            }

            return query.Where(x => x.Comment.Contains(comment));
        }
    }
}