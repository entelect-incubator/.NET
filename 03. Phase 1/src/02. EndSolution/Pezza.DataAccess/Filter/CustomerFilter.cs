namespace Pezza.DataAccess.Filter
{
    using System.Linq;
    using Pezza.Common.Entities;

    public static class CustomerFilter
    {
        public static IQueryable<Customer> FilterByName(this IQueryable<Customer> query, string name)
            => string.IsNullOrEmpty(name) ? query : query.Where(x => x.Name.ToLower().Contains(name.ToLower()));

        public static IQueryable<Customer> FilterByEmail(this IQueryable<Customer> query, string email)
            => string.IsNullOrEmpty(email) ? query : query.Where(x => x.Email.ToLower().Contains(email.ToLower()));
    }
}