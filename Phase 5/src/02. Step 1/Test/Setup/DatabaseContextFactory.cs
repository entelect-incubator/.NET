namespace Test.Setup;

public class DatabaseContextFactory
{
	protected DatabaseContextFactory()
	{
	}

	public static DatabaseContext DBContext()
	{
		var options = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase("EListDb").Options;
		return new DatabaseContext(options);
	}

	public static DatabaseContext Create()
	{
		var context = DBContext();

		context.Database.EnsureCreated();

		return context;
	}

	public static void Destroy(DatabaseContext context)
	{
		context.Database.EnsureDeleted();

		context.Dispose();
	}
}