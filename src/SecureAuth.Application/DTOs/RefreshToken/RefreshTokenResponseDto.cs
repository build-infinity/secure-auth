namespace SecureAuth.Application.DTOs.RefreshToken
{
    public sealed record RefreshTokenResponseDto
    {
        public string AccessToken { get; init;} = null!;
        public DateTime AccessTokenExpiresOnUtc { get; init; }
        public string RefreshToken { get; init; } = null!;
        public DateTime RefreshTokenExpiresOnUtc { get; init; } 
    }
}