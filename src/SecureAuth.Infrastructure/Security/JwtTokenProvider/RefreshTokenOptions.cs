using System.ComponentModel.DataAnnotations;

namespace SecureAuth.Infrastructure.Security
{
    public sealed class RefreshTokenOptions
    {
        [Range(7,30)]
        public int LifeTimeDays { get; set; }
        [Range(32,64)]
        public int SizeInBytes { get; set; }
    }
}
