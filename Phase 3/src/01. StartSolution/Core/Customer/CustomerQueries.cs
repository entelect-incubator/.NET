namespace Core.Customer;

public static class CustomerQueries
{
	public static readonly Func<DatabaseContext, int, Task<Common.Entities.Customer?>> FindQuery
		= EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == id));
}
