using SecureAuth.Domain.Models;

namespace SecureAuth.Application.Abstractions
{
    public interface IRoleRepository
    {
        Task<Role?> GetByName(string roleName, CancellationToken cancellationToken);
    }
}