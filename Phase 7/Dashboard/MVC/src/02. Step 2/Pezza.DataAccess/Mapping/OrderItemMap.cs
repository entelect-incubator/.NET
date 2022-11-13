namespace Pezza.DataAccess.Mapping;

using Microsoft.EntityFrameworkCore;
using Pezza.Common.Entities;

public class OrderItemMap : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<OrderItem> builder)
    {
        // table
        builder.ToTable("OrderItem", "dbo");

        // key
        builder.HasKey(t => t.Id);

        // properties
        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName("Id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.OrderId)
            .IsRequired()
            .HasColumnName("OrderId")
            .HasColumnType("int");

        builder.Property(t => t.ProductId)
            .IsRequired()
            .HasColumnName("ProductId")
            .HasColumnType("int");

        builder.Property(t => t.Quantity)
            .IsRequired()
            .HasColumnName("Quantity")
            .HasColumnType("int");
    }
}
