using SecureAuth.Domain.Models;

namespace SecureAuth.Application.Abstractions
{
    public interface IEmailVerificationRepository
    {
        void Add(EmailVerification emailVerification);
        Task<EmailVerification?> GetByIdAsync(Guid id);
        IQueryable<EmailVerification> GetAll();
        void Remove(EmailVerification emailVerification);
        Task<EmailVerification?> GetVerificationByEmailAsync(string email);
    }
}