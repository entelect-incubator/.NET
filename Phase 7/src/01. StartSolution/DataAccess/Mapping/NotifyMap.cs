namespace DataAccess.Mapping;

public sealed class NotifyMap : IEntityTypeConfiguration<Notify>
{
	public void Configure(EntityTypeBuilder<Notify> builder)
	{
		builder.ToTable("Notify", "dbo");

		builder.HasKey(t => t.Id);

		builder.Property(t => t.Id)
			.IsRequired()
			.HasColumnName("Id")
			.HasColumnType("int")
			.ValueGeneratedOnAdd();

		builder.Property(t => t.CustomerId)
			.IsRequired()
			.HasColumnName("CustomerId")
			.HasColumnType("int");

		builder.Property(t => t.CustomerEmail)
			.IsRequired()
			.HasColumnName("CustomerEmail")
			.HasColumnType("varchar(500)")
			.HasMaxLength(500);

		builder.Property(t => t.EmailContent)
			.HasColumnName("EmailContent")
			.HasColumnType("varchar(max)");

		builder.Property(t => t.Sent)
			.HasColumnName("Sent")
			.HasColumnType("bool");

		builder.Property(t => t.DateSent)
			.IsRequired()
			.HasColumnName("DateSent")
			.HasColumnType("datetime");

		builder.HasOne(y => y.Customer)
			.WithMany(x => x.Notifies)
			.HasForeignKey(x => x.CustomerId);
	}
}