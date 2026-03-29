using Microsoft.EntityFrameworkCore;
using SecureAuth.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SecureAuth.Infrastructure.Persistence.Configurations
{
    internal sealed class UserConiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(u => u.UserId);

            builder.HasIndex(u => u.NormalizedEmail)
                   .IsUnique();
        }
    }
}