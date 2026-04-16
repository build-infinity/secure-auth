using Microsoft.EntityFrameworkCore;
using SecureAuth.Application.Abstractions;
using SecureAuth.Domain.Models;
using SecureAuth.Infrastructure.Persistence.ApplicationDbContext;

namespace  SecureAuth.Infrastructure.Presistence.Repositories
{
    internal sealed class EmailVerificationRepository : IEmailVerificationRepository
    {
        private readonly AppDbContext _context;
        public EmailVerificationRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Add(EmailVerification emailVerification)
        {
            _context.EmailVerifications.Add(emailVerification);
        }

        public async Task<EmailVerification?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.EmailVerifications.FindAsync(id, cancellationToken);
        }
        public IQueryable<EmailVerification> GetAll()
        {
            return _context.EmailVerifications.AsQueryable();
        }
        public void Remove(EmailVerification emailVerification)
        {
            _context.EmailVerifications.Remove(emailVerification);
        }
        public async Task<EmailVerification?> GetVerificationByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.EmailVerifications.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }
    }
}