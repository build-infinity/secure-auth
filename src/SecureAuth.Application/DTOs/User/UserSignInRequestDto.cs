using System.ComponentModel.DataAnnotations;

namespace SecureAuth.Application.DTOs.User
{
    public sealed record UserSignInRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = null!;
        [Required]
        public string Password { get; init; } = null!;
    }
}