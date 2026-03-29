namespace SecureAuth.Application.DTOs.User
{
    public sealed record UserSignOutRequestDto
    {
        public string RefreshToken { get; init; } = null!;
    }
}