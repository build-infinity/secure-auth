using System.ComponentModel.DataAnnotations;

namespace SecureAuth.Application.DTOs.User 
{
    public sealed record UserSignUpRequestDto
    {
        [Required]
        public string FirstName { get; init; } = null!;
        [Required]
        public string LastName { get; init; } = null!;
        [Required]
        [MinLength(8)]
        public string Password { get; init; } = null!;
    }
}