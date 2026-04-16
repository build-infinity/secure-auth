using SecureAuth.Domain.Models;

namespace SecureAuth.Application.Abstractions
{
    public interface IRefreshTokenRepository
    {
        void Add(RefreshToken refreshToken);
        Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken);
        Task<bool> TryRevokeAsync(string tokenHash, CancellationToken cancellationToken);
    }
}