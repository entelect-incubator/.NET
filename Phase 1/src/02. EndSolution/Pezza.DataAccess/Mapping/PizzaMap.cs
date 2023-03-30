namespace Pezza.DataAccess.Mapping;

using Microsoft.EntityFrameworkCore;
using Pezza.Common.Entities;

public partial class PizzaMap : IEntityTypeConfiguration<Pizza>
{
	public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Pizza> builder)
	{
		builder.ToTable("Pizza", "dbo");

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

		builder.Property(t => t.Description)
			.HasColumnName("Description")
			.HasColumnType("varchar(500)")
			.HasMaxLength(20);

		builder.Property(t => t.PictureUrl)
			.HasColumnName("PictureUrl")
			.HasColumnType("varchar(1000)");

		builder.Property(t => t.Price)
			.HasColumnName("Price")
			.HasColumnType("decimal(17, 2)");

		builder.Property(t => t.DateCreated)
			.IsRequired()
			.HasColumnName("DateCreated")
			.HasColumnType("datetime")
			.HasDefaultValueSql("(getdate())");
	}
}