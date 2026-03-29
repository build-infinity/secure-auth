using Microsoft.EntityFrameworkCore;
using SecureAuth.Application.Abstractions;
using SecureAuth.Domain.Models;
using SecureAuth.Infrastructure.Persistence.ApplicationDbContext;

namespace SecureAuth.Infrastructure.Presistence.Repositories
{
    internal sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context; 
        public RefreshTokenRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public void Add(RefreshToken refreshToken)
        {
           _context.RefreshTokens.Add(refreshToken);
        }
        public async Task<RefreshToken?> GetByIdAsync(Guid id)
        {
           return await _context.RefreshTokens.FindAsync(id);
        }
        public async Task<RefreshToken?> GetByTokenHashAsync( string tokenHash)
        {
            return await _context.RefreshTokens.Include(t => t.User).FirstOrDefaultAsync(t => t.TokenHash == tokenHash);
        } 
        public async Task<bool> TryRevokeAsync(string tokenHash)
        {
            return await _context.RefreshTokens
                           .Where(t => t.TokenHash == tokenHash && !t.IsRevoked)
                           .ExecuteUpdateAsync(setters => setters
                           .SetProperty(x => x.IsRevoked, true)) > 0;
        }
    }
}