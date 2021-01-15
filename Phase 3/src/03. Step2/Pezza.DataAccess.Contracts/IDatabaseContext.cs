namespace Pezza.DataAccess.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.Entities;

    public interface IDatabaseContext
    {
        DbSet<Customer> Customers { get; set; }

        DbSet<Notify> Notify { get; set; }

        DbSet<Order> Orders { get; set; }

        DbSet<OrderItem> OrderItems { get; set; }

        DbSet<Product> Products { get; set; }

        DbSet<Restaurant> Restaurants { get; set; }

        DbSet<Stock> Stocks { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
