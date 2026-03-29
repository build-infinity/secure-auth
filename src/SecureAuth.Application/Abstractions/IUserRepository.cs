using SecureAuth.Domain.Models;

namespace SecureAuth.Application.Abstractions
{
    public interface IUserRepository
    {
        void Add(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByNormalizedEmailAsync(string normalizedEmail);
        IQueryable<User> GetAll();
        void Remove(User user);
        Task<bool> EmailExistsAsync(string email);
    }
}