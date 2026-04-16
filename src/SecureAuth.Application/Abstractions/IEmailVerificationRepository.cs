using SecureAuth.Domain.Models;

namespace SecureAuth.Application.Abstractions
{
    public interface IEmailVerificationRepository
    {
        void Add(EmailVerification emailVerification);
        Task<EmailVerification?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        IQueryable<EmailVerification> GetAll();
        void Remove(EmailVerification emailVerification);
        Task<EmailVerification?> GetVerificationByEmailAsync(string email, CancellationToken cancellationToken);
    }
}