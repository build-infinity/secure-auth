using Microsoft.EntityFrameworkCore;
using SecureAuth.Domain.Models;

namespace SecureAuth.Infrastructure.Persistence.ApplicationDbContext
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) {} 

        public DbSet<User> Users => Set<User>();
        public DbSet<EmailVerification> EmailVerifications => Set<EmailVerification>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}