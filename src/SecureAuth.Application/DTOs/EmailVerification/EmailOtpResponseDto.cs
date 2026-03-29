namespace SecureAuth.Application.DTOs.EmailVerification
{
    public sealed record EmailOtpResponseDto
    {
        public Guid VerificationId { get; init; }
        public DateTime OtpExpiresAtUtc { get; init; }
    }
}