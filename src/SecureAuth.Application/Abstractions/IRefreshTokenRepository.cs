using SecureAuth.Domain.Models;

namespace SecureAuth.Application.Abstractions
{
    public interface IRefreshTokenRepository
    {
        void Add(RefreshToken refreshToken);
        Task<RefreshToken?> GetByIdAsync(Guid id);
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
        Task<bool> TryRevokeAsync(string tokenHash);
    }
}