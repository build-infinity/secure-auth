namespace SecureAuth.Application.Common
{
    public sealed record TokenResult
    {
        public string Token { get; init; } = null!;
        public DateTime ExpiresOnUtc { get; init; }
    }
}