using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()")
            .HasColumnName("Id");
        builder.HasIndex(u => u.Username)
            .IsUnique();
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("Username");
        builder.Property(u => u.Email)
            .HasConversion(
                v => v.Address,
                v => new Email(v)
            )
            .IsRequired();
        builder.HasIndex(u => u.Email)
            .IsUnique();
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("PasswordHash");
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("FirstName");
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("LastName");
        builder.Property(u => u.DateOfBirth)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName("DateOfBirth");
        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()")
            .HasColumnName("CreatedAt");
    }
}