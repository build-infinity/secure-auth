namespace SecureAuth.Application.DTOs.User
{
    public sealed record UserSignInResponseDto
    {
        public string AccessToken { get; init;} = null!;
        public DateTime AccessTokenExpiresOnUtc { get; init; }
        public string RefreshToken { get; init; } = null!;
        public DateTime RefreshTokenExpiresOnUtc { get; init; } 
    }
}