using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ChatConfiguration: IEntityTypeConfiguration<ChatEntity>
{
    public void Configure(EntityTypeBuilder<ChatEntity> builder)
    {
        builder.ToTable("Chats");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("newid()")
            .HasColumnName("Id");
        builder.Property(o=>o.Title)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("Title");
        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()")
            .HasColumnName("CreatedAt");
        builder.Property(u => u.IsPrivate)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("IsPrivate");
        builder.Property(u => u.ChatType)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnName("ChatType");
    }
}