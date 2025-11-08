using InstaCore.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InstaCore.Infrastructure.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity
                .HasKey(u => u.Id);

            entity
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            entity
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            entity
                .Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(200);

            entity
                .Property(u => u.Bio)
                .HasMaxLength(500);

            entity
                .Property(u => u.AvatarUrl)
                .HasMaxLength(512);

            entity
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();
        }
    }
}
