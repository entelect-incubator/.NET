namespace Pezza.DataAccess
{
	using System;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata;

    public class PezzaDbContext : DbContext
    {
        public PezzaDbContext(DbContextOptions<PezzaDbContext> options)
            : base(options)
        {
        }
		
        public virtual DbSet<Customer> Customers { get; set; }
		
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerMap());
        }
    }
}
