namespace Pezza.DataAccess.Map;

using Microsoft.EntityFrameworkCore;
using Pezza.Common.Entities;

public partial class OrderMap
    : IEntityTypeConfiguration<Order>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Order> builder)
    {
        // table
        builder.ToTable("Order", "dbo");

        // key
        builder.HasKey(t => t.Id);

        // properties
        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName("Id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.CustomerId)
            .IsRequired()
            .HasColumnName("CustomerId")
            .HasColumnType("int");

        builder.Property(t => t.RestaurantId)
            .IsRequired()
            .HasColumnName("RestaurantId")
            .HasColumnType("int");

        builder.Property(t => t.Amount)
            .IsRequired()
            .HasColumnName("Amount")
            .HasColumnType("decimal(17, 2)");

        builder.Property(t => t.DateCreated)
            .IsRequired()
            .HasColumnName("DateCreated")
            .HasColumnType("datetime")
            .HasDefaultValueSql("(getdate())");

        builder.Property(t => t.Completed)
            .IsRequired()
            .HasColumnName("Completed")
            .HasColumnType("bit")
            .HasDefaultValueSql("(0)");

        // relationships
        builder.HasOne(t => t.Customer)
            .WithMany(t => t.Orders)
            .HasForeignKey(d => d.CustomerId)
            .HasConstraintName("FK_Order_Customer");
    }

}
