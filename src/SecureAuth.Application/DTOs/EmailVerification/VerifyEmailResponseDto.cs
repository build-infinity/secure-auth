namespace SecureAuth.Application.DTOs.EmailVerification 
{
    public sealed record VerifyEmailResponseDto
    {
        public string RegistrationToken { get; init;} = null!;
        public DateTime ExpiresOnUtc { get; init; }
    }
}