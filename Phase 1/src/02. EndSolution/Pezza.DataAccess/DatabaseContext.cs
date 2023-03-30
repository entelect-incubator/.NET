namespace Pezza.DataAccess;

using Microsoft.EntityFrameworkCore;
using Pezza.Common.Entities;
using Pezza.DataAccess.Mapping;

public class DatabaseContext : DbContext
{
	public DatabaseContext()
	{
	}

	public DatabaseContext(DbContextOptions options) : base(options)
	{
	}

	public virtual DbSet<Pizza> Pizzas { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
		=> modelBuilder.ApplyConfiguration(new PizzaMap());
}
