namespace Pezza.Test;

using Microsoft.EntityFrameworkCore;
using Pezza.DataAccess;

public class DatabaseContextTest
{
	protected DatabaseContextTest(DbContextOptions<DbContext> contextOptions)
	{
		this.ContextOptions = contextOptions;
		this.Seed();
	}

	protected DbContextOptions<DbContext> ContextOptions { get; }

	private void Seed()
	{
		using var context = new DatabaseContext(this.ContextOptions);

		context.Database.EnsureDeleted();
		context.Database.EnsureCreated();

		context.SaveChanges();
	}
}
