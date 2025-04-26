using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserContactConfiguration : IEntityTypeConfiguration<UserContactEntity>
{
    public void Configure(EntityTypeBuilder<UserContactEntity> builder)
    {
        builder.ToTable("UserContacts");
        builder.HasKey(x=>x.Id);
        builder.Property(x=>x.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()");
        builder.HasOne(x => x.User)
            .WithMany(x=>x.Contacts)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        builder.HasOne(x => x.Contact)
            .WithMany()
            .HasForeignKey(x => x.ContactId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        builder.HasOne(x => x.PrivateChat)
            .WithMany()
            .HasForeignKey(x => x.PrivateChatId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        builder.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();
        builder.HasIndex(x => new { x.UserId, x.ContactId }).IsUnique();
    }
}