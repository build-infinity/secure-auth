using Microsoft.EntityFrameworkCore;
using SecureAuth.Application.Abstractions;
using SecureAuth.Domain.Models;
using SecureAuth.Infrastructure.Persistence.ApplicationDbContext;

namespace SecureAuth.Infrastructure.Presistence.Repositories
{
    internal sealed class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetByName(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }
    }
}