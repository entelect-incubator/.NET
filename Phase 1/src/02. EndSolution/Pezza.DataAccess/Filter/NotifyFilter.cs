namespace Pezza.DataAccess.Filter
{
    using System.Linq;
    using Pezza.Common.Entities;

    public static class NotifyFilter
    {
        public static IQueryable<Notify> FilterByEmail(this IQueryable<Notify> query, string email)
            => string.IsNullOrEmpty(email) ? query : query.Where(x => x.Email.ToLower().Contains(email.ToLower()));
    }
}