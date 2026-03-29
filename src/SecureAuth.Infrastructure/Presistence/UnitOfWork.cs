using SecureAuth.Application.Abstractions;
using SecureAuth.Infrastructure.Persistence.ApplicationDbContext;

namespace SecureAuth.Infrastructure.Presistence
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cl = default)
        {
            return await _context.SaveChangesAsync(cl);
        }
    }
}