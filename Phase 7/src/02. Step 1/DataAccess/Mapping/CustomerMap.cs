namespace DataAccess.Mapping;

using Microsoft.EntityFrameworkCore;
using Common.Entities;

public sealed class CustomerMap : IEntityTypeConfiguration<Customer>
{
	public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Customer> builder)
	{
		builder.ToTable("Customer", "dbo");

		builder.HasKey(t => t.Id);

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
			.HasColumnName("Address")
			.HasColumnType("varchar(500)")
			.HasMaxLength(500);

		builder.Property(t => t.Email)
			.HasColumnName("Email")
			.HasColumnType("varchar(500)")
			.HasMaxLength(500);

		builder.Property(t => t.Cellphone)
			.HasColumnName("Cellphone")
			.HasColumnType("varchar(50)")
			.HasMaxLength(50);

		builder.Property(t => t.DateCreated)
			.IsRequired()
			.HasColumnName("DateCreated")
			.HasColumnType("datetime")
			.HasDefaultValueSql("(getdate())");
	}
}