using System.ComponentModel.DataAnnotations;

namespace SecureAuth.Infrastructure.Security.JwtTokenProvider
{
    public sealed class JwtOptions 
    {
        [Required]
        public HmacAlgorithm HmacAlgorithm { get; set; }
        [Required]
        public string Issuer { get; set; } = null!;
        [Required]
        public string Audience { get; set; } = null!;
        [Required]
        [MinLength(32)]
        public string AccessKey { get; set; } = null!;
        public int AccessExpiresInMinutes { get; set; }
        [Required]
        [MinLength(32)]
        public string RegistrationKey { get; set;}  = null!;
        [Required]
        public int RegistrationExpiresInMinutes { get; set; }
    }
}