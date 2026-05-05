namespace DataAccess.Mapping;

public sealed class TodoMap : IEntityTypeConfiguration<Todo>
{
	public void Configure(EntityTypeBuilder<Todo> builder)
	{
		builder.ToTable("Todos", "dbo");

		builder.HasKey(t => t.Id);

		builder.Property(t => t.Id)
			.IsRequired()
			.HasColumnType("int")
			.ValueGeneratedOnAdd();

		builder.Property(t => t.Task)
			.IsRequired()
			.HasColumnType("varchar(250)")
			.HasMaxLength(250);

		builder.Property(t => t.IsCompleted)
			.IsRequired()
			.HasDefaultValue(false);

		builder.Property(t => t.DateCreated)
			.IsRequired()
			.HasColumnType("datetime")
			.HasDefaultValueSql("(getdate())");

		builder.Property(t => t.SessionId)
			.IsRequired()
			.HasColumnType("uniqueidentifier");
	}
}