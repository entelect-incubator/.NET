namespace Common.Filters;

using System;
using System.Linq;
using Common.Entities;

public static class RestaurantFilter
{
    public static IQueryable<Restaurant> FilterByName(this IQueryable<Restaurant> query, string name)
    {
        return string.IsNullOrWhiteSpace(name) ? query : query.Where(x => x.Name.Contains(name));
    }

    public static IQueryable<Restaurant> FilterByDescription(this IQueryable<Restaurant> query, string description)
    {
        return string.IsNullOrWhiteSpace(description) ? query : query.Where(x => x.Description.Contains(description));
    }

    public static IQueryable<Restaurant> FilterByPictureUrl(this IQueryable<Restaurant> query, string pictureUrl)
    {
        return string.IsNullOrWhiteSpace(pictureUrl) ? query : query.Where(x => x.PictureUrl.Contains(pictureUrl));
    }

    public static IQueryable<Restaurant> FilterByAddress(this IQueryable<Restaurant> query, string address)
    {
        return string.IsNullOrWhiteSpace(address) ? query : query.Where(x => x.Address.Contains(address));
    }

    public static IQueryable<Restaurant> FilterByCity(this IQueryable<Restaurant> query, string city)
    {
        return string.IsNullOrWhiteSpace(city) ? query : query.Where(x => x.City.Contains(city));
    }

    public static IQueryable<Restaurant> FilterByProvince(this IQueryable<Restaurant> query, string province)
    {
        return string.IsNullOrWhiteSpace(province) ? query : query.Where(x => x.Province.Contains(province));
    }

    public static IQueryable<Restaurant> FilterByPostalCode(this IQueryable<Restaurant> query, string postalCode)
    {
        return string.IsNullOrWhiteSpace(postalCode) ? query : query.Where(x => x.PostalCode.Contains(postalCode));
    }

    public static IQueryable<Restaurant> FilterByIsActive(this IQueryable<Restaurant> query, bool? isActive)
    {
        return !isActive.HasValue ? query : query.Where(x => x.IsActive == isActive.Value);
    }

    public static IQueryable<Restaurant> FilterByDateCreated(this IQueryable<Restaurant> query, DateTime? dateCreated)
    {
        return !dateCreated.HasValue ? query : query.Where(x => x.DateCreated == dateCreated.Value);
    }
}