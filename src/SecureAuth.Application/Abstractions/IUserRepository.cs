using SecureAuth.Domain.Models;

namespace SecureAuth.Application.Abstractions
{
    public interface IUserRepository
    {
        void Add(User user);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<User?> GetByEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
        IQueryable<User> GetAll();
        void Remove(User user);
        Task<bool> ExistsByEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
    }
}