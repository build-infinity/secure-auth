using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureAuth.Domain.Models;

namespace SecureAuth.Infrastructure.Presistence.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");
            builder.HasKey(r => r.RoleId);

            builder.HasData(
                new Role { RoleId = Guid.NewGuid(), Name = "User"},
                new Role { RoleId = Guid.NewGuid(), Name = "Moderator"},
                new Role { RoleId = Guid.NewGuid(), Name = "Admin"}
            );
        }
    }
}