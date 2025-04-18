using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserChatConfiguration:IEntityTypeConfiguration<UserChatEntity>
{
    public void Configure(EntityTypeBuilder<UserChatEntity> builder)
    {
        builder.ToTable("UserChat");
        builder.HasOne(o=>o.User)
            .WithMany(o=>o.UserChatEntities)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(o=>o.Chat)
            .WithMany(o=>o.UserChatEntities)
            .HasForeignKey(x => x.ChatId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasKey(o=>new {o.ChatId,o.UserId});
        builder.Property(o=>o.ChatId).IsRequired();
        builder.Property(o=>o.UserId).IsRequired();
        builder.Property(o=>o.IsMuted)
            .HasDefaultValue(false)
            .IsRequired();
        builder.Property(o=>o.ChatRole)
            .HasDefaultValue(ChatRole.User)
            .IsRequired()
            .HasConversion<string>()
            .IsRequired();
    }
}