
using System.ComponentModel.DataAnnotations;

namespace SecureAuth.Infrastructure.Email
{
    public sealed class SmtpOptions 
    {
        [Required]
        public string Host { get; set; } = null!;
        [Required]
        public int Port { get; set; }
        [Required]
        public string User { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string FromEmail { get; set; } = null!;
        public string FromName { get; set; } = null!;
    }
}