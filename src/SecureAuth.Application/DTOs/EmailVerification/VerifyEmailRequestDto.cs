using System.ComponentModel.DataAnnotations;

namespace SecureAuth.Application.DTOs.EmailVerification 
{
    public sealed record VerifyEmailRequestDto
    {
        [Required]
        public Guid VerificationId { get; init; }
        [Required]
        [Length(6,6)]
        public string Otp { get; init; } = null!;
    }
}