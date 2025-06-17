using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()"); 
        builder.HasIndex(u => u.Username)
            .IsUnique();
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(u => u.Email)
            .IsRequired()
            .HasConversion(
                v => v.Address,
                v => new Email(v)
            );
        builder.HasIndex(u => u.Email)
            .IsUnique();
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(u => u.DateOfBirth)
            .IsRequired()
            .HasColumnType("date");
        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}