using System.Security.Cryptography.X509Certificates;
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
        public async Task<User?> GetByNormalizedEmailAsync(string normalizedEmail)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
        }
        public async Task<User?> GetByIdAsync(Guid id) 
        {
          return  await _context.Users.FindAsync(id);
        }
        
        public IQueryable<User> GetAll()
        {
            return _context.Users.AsNoTracking();
        }
        public void Remove(User user)
        {
           _context.Users.Remove(user);
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }
    }
}