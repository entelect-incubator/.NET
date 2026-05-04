namespace Common.Filters;

using System;
using System.Linq;
using Common.Entities;

public static class ProductFilter
{
    public static IQueryable<Product> FilterByName(this IQueryable<Product> query, string name)
    {
        return string.IsNullOrWhiteSpace(name) ? query : query.Where(x => x.Name.Contains(name));
    }

    public static IQueryable<Product> FilterByDescription(this IQueryable<Product> query, string description)
    {
        return string.IsNullOrWhiteSpace(description) ? query : query.Where(x => x.Description.Contains(description));
    }

    public static IQueryable<Product> FilterByPictureUrl(this IQueryable<Product> query, string pictureUrl)
    {
        return string.IsNullOrWhiteSpace(pictureUrl) ? query : query.Where(x => x.PictureUrl.Contains(pictureUrl));
    }

    public static IQueryable<Product> FilterByPrice(this IQueryable<Product> query, decimal? price)
    {
        return !price.HasValue ? query : query.Where(x => x.Price == price.Value);
    }

    public static IQueryable<Product> FilterBySpecial(this IQueryable<Product> query, bool? special)
    {
        return !special.HasValue ? query : query.Where(x => x.Special == special.Value);
    }

    public static IQueryable<Product> FilterByOfferEndDate(this IQueryable<Product> query, DateTime? offerEndDate)
    {
        return !offerEndDate.HasValue ? query : query.Where(x => x.OfferEndDate == offerEndDate.Value);
    }

    public static IQueryable<Product> FilterByOfferPrice(this IQueryable<Product> query, decimal? offerPrice)
    {
        return !offerPrice.HasValue ? query : query.Where(x => x.OfferPrice == offerPrice.Value);
    }

    public static IQueryable<Product> FilterByIsActive(this IQueryable<Product> query, bool? isActive)
    {
        return !isActive.HasValue ? query : query.Where(x => x.IsActive == isActive.Value);
    }

    public static IQueryable<Product> FilterByDateCreated(this IQueryable<Product> query, DateTime? dateCreated)
    {
        return !dateCreated.HasValue ? query : query.Where(x => x.DateCreated == dateCreated.Value);
    }
}