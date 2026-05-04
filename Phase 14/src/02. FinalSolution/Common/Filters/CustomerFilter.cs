namespace Common.Filters;

using System;
using System.Linq;
using Common.Entities;

public static class CustomerFilter
{
    public static IQueryable<Customer> FilterByName(this IQueryable<Customer> query, string name)
    {
        return string.IsNullOrWhiteSpace(name) ? query : query.Where(x => x.Name.Contains(name));
    }

    public static IQueryable<Customer> FilterByAddress(this IQueryable<Customer> query, string address)
    {
        return string.IsNullOrWhiteSpace(address) ? query : query.Where(x => x.Address.Contains(address));
    }

    public static IQueryable<Customer> FilterByCity(this IQueryable<Customer> query, string city)
    {
        return string.IsNullOrWhiteSpace(city) ? query : query.Where(x => x.City.Contains(city));
    }

    public static IQueryable<Customer> FilterByProvince(this IQueryable<Customer> query, string province)
    {
        return string.IsNullOrWhiteSpace(province) ? query : query.Where(x => x.Province.Contains(province));
    }

    public static IQueryable<Customer> FilterByPostalCode(this IQueryable<Customer> query, string postalCode)
    {
        return string.IsNullOrWhiteSpace(postalCode) ? query : query.Where(x => x.PostalCode.Contains(postalCode));
    }

    public static IQueryable<Customer> FilterByPhone(this IQueryable<Customer> query, string phone)
    {
        return string.IsNullOrWhiteSpace(phone) ? query : query.Where(x => x.Phone.Contains(phone));
    }

    public static IQueryable<Customer> FilterByEmail(this IQueryable<Customer> query, string email)
    {
        return string.IsNullOrWhiteSpace(email) ? query : query.Where(x => x.Email.Contains(email));
    }

    public static IQueryable<Customer> FilterByContactPerson(this IQueryable<Customer> query, string contactPerson)
    {
        return string.IsNullOrWhiteSpace(contactPerson) ? query : query.Where(x => x.ContactPerson.Contains(contactPerson));
    }

    public static IQueryable<Customer> FilterByDateCreated(this IQueryable<Customer> query, DateTime? dateCreated)
    {
        return !dateCreated.HasValue ? query : query.Where(x => x.DateCreated == dateCreated.Value);
    }
}