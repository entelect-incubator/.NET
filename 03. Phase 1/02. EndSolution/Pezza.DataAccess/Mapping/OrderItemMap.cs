namespace Pezza.DataAccess.Mapping
{
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.Entities;

    public partial class OrderItemMap
        : IEntityTypeConfiguration<OrderItem>
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

            // relationships
            builder.HasOne(t => t.Order)
                .WithMany(t => t.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderItem_Order");

            builder.HasOne(t => t.Product)
                .WithMany(t => t.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_OrderItem_Product");
        }

    }
}
