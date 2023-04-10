namespace DataAccess;

using Microsoft.EntityFrameworkCore;
using Common.Entities;
using DataAccess.Mapping;

public class DatabaseContext : DbContext
{
	public DatabaseContext()
	{
	}

	public DatabaseContext(DbContextOptions options) : base(options)
	{
	}

	public virtual DbSet<Stock> Stocks { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
		=> modelBuilder.ApplyConfiguration(new StockMap());
}
