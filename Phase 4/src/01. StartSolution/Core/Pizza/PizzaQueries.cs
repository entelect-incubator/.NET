namespace Core.Pizza;

public static class PizzaQueries
{
	public static readonly Func<DatabaseContext, int, Task<Common.Entities.Pizza?>> FindQuery
		= EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
}
