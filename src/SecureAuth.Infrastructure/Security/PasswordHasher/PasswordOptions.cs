using System.ComponentModel.DataAnnotations;

namespace SecureAuth.Infrastructure.Security.PasswordHasher
{
    public sealed class PasswordOptions
    {
        [Required]
        public Algorithm HashAlgorithm { get; set; }
        [Required]
        public int HashSize { get; set; }
        [Required]
        public int SaltSize { get; set; }
        [Required]
        public int Iterations { get; set; }
    }
}