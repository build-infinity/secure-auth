
using System.ComponentModel.DataAnnotations;

namespace  SecureAuth.Application.DTOs.RefreshToken
{
   public sealed record RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; init; } = null!;
    }
}