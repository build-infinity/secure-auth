using System.ComponentModel.DataAnnotations;

namespace SecureAuth.Application.DTOs.EmailVerification
{
    public sealed record EmailOtpRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = null!;
    }
}