namespace Pezza.Common.Filters;

using System;
using System.Linq;
using Pezza.Common.Entities;

public static class RestaurantFilter
{
    public static IQueryable<Restaurant> FilterByName(this IQueryable<Restaurant> query, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return query;
        }

        return query.Where(x => x.Name.Contains(name));
    }

    public static IQueryable<Restaurant> FilterByDescription(this IQueryable<Restaurant> query, string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return query;
        }

        return query.Where(x => x.Description.Contains(description));
    }

    public static IQueryable<Restaurant> FilterByPictureUrl(this IQueryable<Restaurant> query, string pictureUrl)
    {
        if (string.IsNullOrWhiteSpace(pictureUrl))
        {
            return query;
        }

        return query.Where(x => x.PictureUrl.Contains(pictureUrl));
    }

    public static IQueryable<Restaurant> FilterByAddress(this IQueryable<Restaurant> query, string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return query;
        }

        return query.Where(x => x.Address.Contains(address));
    }

    public static IQueryable<Restaurant> FilterByCity(this IQueryable<Restaurant> query, string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return query;
        }

        return query.Where(x => x.City.Contains(city));
    }

    public static IQueryable<Restaurant> FilterByProvince(this IQueryable<Restaurant> query, string province)
    {
        if (string.IsNullOrWhiteSpace(province))
        {
            return query;
        }

        return query.Where(x => x.Province.Contains(province));
    }

    public static IQueryable<Restaurant> FilterByPostalCode(this IQueryable<Restaurant> query, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
        {
            return query;
        }

        return query.Where(x => x.PostalCode.Contains(postalCode));
    }

    public static IQueryable<Restaurant> FilterByIsActive(this IQueryable<Restaurant> query, bool? isActive)
    {
        if (!isActive.HasValue)
        {
            return query;
        }

        return query.Where(x => x.IsActive == isActive.Value);
    }

    public static IQueryable<Restaurant> FilterByDateCreated(this IQueryable<Restaurant> query, DateTime? dateCreated)
    {
        if (!dateCreated.HasValue)
        {
            return query;
        }

        return query.Where(x => x.DateCreated == dateCreated.Value);
    }
}