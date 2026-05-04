namespace DataAccess.Filters;

public static class CustomerFilter
{
	public static IQueryable<Customer> FilterByName(this IQueryable<Customer> query, string name) => string.IsNullOrWhiteSpace(name) ? query : query.Where(x => x.Name.Contains(name));

	public static IQueryable<Customer> FilterByAddress(this IQueryable<Customer> query, string address) => string.IsNullOrWhiteSpace(address) ? query : query.Where(x => x.Address.Contains(address));

	public static IQueryable<Customer> FilterByPhone(this IQueryable<Customer> query, string cellphone) => string.IsNullOrWhiteSpace(cellphone) ? query : query.Where(x => x.Cellphone.Contains(cellphone));

	public static IQueryable<Customer> FilterByEmail(this IQueryable<Customer> query, string email) => string.IsNullOrWhiteSpace(email) ? query : query.Where(x => x.Email.Contains(email));

	public static IQueryable<Customer> FilterByDateCreated(this IQueryable<Customer> query, DateTime? dateCreated) => !dateCreated.HasValue ? query : query.Where(x => x.DateCreated == dateCreated.Value);
}