namespace Pezza.Common.Filters
{
    using System;
    using System.Linq;
    using Pezza.Common.Entities;

    public static class ProductFilter
    {
        public static IQueryable<Product> FilterByName(this IQueryable<Product> query, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return query;
            }

            return query.Where(x => x.Name.Contains(name));
        }

        public static IQueryable<Product> FilterByDescription(this IQueryable<Product> query, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return query;
            }

            return query.Where(x => x.Description.Contains(description));
        }

        public static IQueryable<Product> FilterByPictureUrl(this IQueryable<Product> query, string pictureUrl)
        {
            if (string.IsNullOrWhiteSpace(pictureUrl))
            {
                return query;
            }

            return query.Where(x => x.PictureUrl.Contains(pictureUrl));
        }

        public static IQueryable<Product> FilterByPrice(this IQueryable<Product> query, decimal? price)
        {
            if (!price.HasValue)
            {
                return query;
            }

            return query.Where(x => x.Price == price.Value);
        }

        public static IQueryable<Product> FilterBySpecial(this IQueryable<Product> query, bool? special)
        {
            if (!special.HasValue)
            {
                return query;
            }

            return query.Where(x => x.Special == special.Value);
        }

        public static IQueryable<Product> FilterByOfferEndDate(this IQueryable<Product> query, DateTime? offerEndDate)
        {
            if (!offerEndDate.HasValue)
            {
                return query;
            }

            return query.Where(x => x.OfferEndDate == offerEndDate.Value);
        }

        public static IQueryable<Product> FilterByOfferPrice(this IQueryable<Product> query, decimal? offerPrice)
        {
            if (!offerPrice.HasValue)
            {
                return query;
            }

            return query.Where(x => x.OfferPrice == offerPrice.Value);
        }

        public static IQueryable<Product> FilterByIsActive(this IQueryable<Product> query, bool? isActive)
        {
            if (!isActive.HasValue)
            {
                return query;
            }

            return query.Where(x => x.IsActive == isActive.Value);
        }

        public static IQueryable<Product> FilterByDateCreated(this IQueryable<Product> query, DateTime? dateCreated)
        {
            if (!dateCreated.HasValue)
            {
                return query;
            }

            return query.Where(x => x.DateCreated == dateCreated.Value);
        }


    }
}