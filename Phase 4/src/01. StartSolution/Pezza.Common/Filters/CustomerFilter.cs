namespace Common.Filters;

using System;
using System.Linq;
using Common.Entities;

public static class CustomerFilter
{
    public static IQueryable<Customer> FilterByName(this IQueryable<Customer> query, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return query;
        }

        return query.Where(x => x.Name.Contains(name));
    }

    public static IQueryable<Customer> FilterByAddress(this IQueryable<Customer> query, string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return query;
        }

        return query.Where(x => x.Address.Contains(address));
    }

    public static IQueryable<Customer> FilterByCity(this IQueryable<Customer> query, string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return query;
        }

        return query.Where(x => x.City.Contains(city));
    }

    public static IQueryable<Customer> FilterByProvince(this IQueryable<Customer> query, string province)
    {
        if (string.IsNullOrWhiteSpace(province))
        {
            return query;
        }

        return query.Where(x => x.Province.Contains(province));
    }

    public static IQueryable<Customer> FilterByPostalCode(this IQueryable<Customer> query, string PostalCode)
    {
        if (string.IsNullOrWhiteSpace(PostalCode))
        {
            return query;
        }

        return query.Where(x => x.PostalCode.Contains(PostalCode));
    }

    public static IQueryable<Customer> FilterByPhone(this IQueryable<Customer> query, string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return query;
        }

        return query.Where(x => x.Phone.Contains(phone));
    }

    public static IQueryable<Customer> FilterByEmail(this IQueryable<Customer> query, string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return query;
        }

        return query.Where(x => x.Email.Contains(email));
    }

    public static IQueryable<Customer> FilterByContactPerson(this IQueryable<Customer> query, string contactPerson)
    {
        if (string.IsNullOrWhiteSpace(contactPerson))
        {
            return query;
        }

        return query.Where(x => x.ContactPerson.Contains(contactPerson));
    }

    public static IQueryable<Customer> FilterByDateCreated(this IQueryable<Customer> query, DateTime? dateCreated)
    {
        if (!dateCreated.HasValue)
        {
            return query;
        }

        return query.Where(x => x.DateCreated == dateCreated.Value);
    }


}