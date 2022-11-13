namespace Pezza.DataAccess.Map;

using Microsoft.EntityFrameworkCore;
using Pezza.Common.Entities;

public partial class ProductMap
    : IEntityTypeConfiguration<Product>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
    {
        // table
        builder.ToTable("Product", "dbo");

        // key
        builder.HasKey(t => t.Id);

        // properties
        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName("Id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("varchar(150)")
            .HasMaxLength(150);

        builder.Property(t => t.Description)
            .IsRequired()
            .HasColumnName("Description")
            .HasColumnType("varchar(1000)")
            .HasMaxLength(1000);

        builder.Property(t => t.PictureUrl)
            .IsRequired()
            .HasColumnName("PictureUrl")
            .HasColumnType("varchar(1000)")
            .HasMaxLength(1000);

        builder.Property(t => t.Price)
            .IsRequired()
            .HasColumnName("Price")
            .HasColumnType("decimal(17, 2)");

        builder.Property(t => t.Special)
            .IsRequired()
            .HasColumnName("Special")
            .HasColumnType("bit");

        builder.Property(t => t.OfferEndDate)
            .HasColumnName("OfferEndDate")
            .HasColumnType("datetime");

        builder.Property(t => t.OfferPrice)
            .HasColumnName("OfferPrice")
            .HasColumnType("decimal(17, 2)");

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasColumnName("IsActive")
            .HasColumnType("bit")
            .HasDefaultValueSql("((1))");

        builder.Property(t => t.DateCreated)
            .IsRequired()
            .HasColumnName("DateCreated")
            .HasColumnType("datetime")
            .HasDefaultValueSql("(getdate())");
    }

}
