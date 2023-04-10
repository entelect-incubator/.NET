namespace DataAccess.Map;

using Microsoft.EntityFrameworkCore;
using Common.Entities;

public partial class CustomerMap
    : IEntityTypeConfiguration<Customer>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Customer> builder)
    {
        // table
        builder.ToTable("Customer", "dbo");

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
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);

        builder.Property(t => t.Address)
            .IsRequired()
            .HasColumnName("Address")
            .HasColumnType("varchar(500)")
            .HasMaxLength(500);

        builder.Property(t => t.City)
            .IsRequired()
            .HasColumnName("City")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);

        builder.Property(t => t.Province)
            .IsRequired()
            .HasColumnName("Province")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);

        builder.Property(t => t.PostalCode)
            .IsRequired()
            .HasColumnName("PostalCode")
            .HasColumnType("varchar(8)")
            .HasMaxLength(8);

        builder.Property(t => t.Phone)
            .IsRequired()
            .HasColumnName("Phone")
            .HasColumnType("varchar(20)")
            .HasMaxLength(20);

        builder.Property(t => t.Email)
            .IsRequired()
            .HasColumnName("Email")
            .HasColumnType("varchar(200)")
            .HasMaxLength(200);

        builder.Property(t => t.ContactPerson)
            .IsRequired()
            .HasColumnName("ContactPerson")
            .HasColumnType("varchar(200)")
            .HasMaxLength(200);

        builder.Property(t => t.DateCreated)
            .IsRequired()
            .HasColumnName("DateCreated")
            .HasColumnType("datetime")
            .HasDefaultValueSql("(getdate())");
    }

}
