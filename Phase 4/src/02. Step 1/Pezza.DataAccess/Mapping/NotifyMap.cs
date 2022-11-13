namespace Pezza.DataAccess.Map;

using Microsoft.EntityFrameworkCore;
using Pezza.Common.Entities;

public class NotifyMap : IEntityTypeConfiguration<Notify>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Notify> builder)
    {
        // table
        builder.ToTable("Notify", "dbo");

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

        builder.Property(t => t.Email)
            .IsRequired()
            .HasColumnName("Email")
            .HasColumnType("nvarchar(max)");

        builder.Property(t => t.Sent)
            .IsRequired()
            .HasColumnName("Sent")
            .HasColumnType("bit");

        builder.Property(t => t.Retry)
            .IsRequired()
            .HasColumnName("Retry")
            .HasColumnType("int");

        builder.Property(t => t.DateSent)
            .IsRequired()
            .HasColumnName("DateSent")
            .HasColumnType("datetime");
    }
}
