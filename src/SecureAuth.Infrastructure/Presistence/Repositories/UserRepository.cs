using Microsoft.EntityFrameworkCore;
using SecureAuth.Application.Abstractions;
using SecureAuth.Domain.Models;
using SecureAuth.Infrastructure.Persistence.ApplicationDbContext;

namespace SecureAuth.Infrastructure.Presistence.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository (AppDbContext context)
        {
            _context = context;
        }
        public void Add(User user)
        {
            _context.Users.Add(user);
        }
        public async Task<User?> GetByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
        }
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken) 
        {
          return  await _context.Users.FindAsync(id, cancellationToken);
        }
        
        public IQueryable<User> GetAll()
        {
            return _context.Users.AsNoTracking();
        }
        public void Remove(User user)
        {
           _context.Users.Remove(user);
        }
        public async Task<bool> ExistsByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);
        }
    }
}