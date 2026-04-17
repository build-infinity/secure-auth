namespace SecureAuth.Domain.Models
{
    public class EmailVerification 
    {
        public Guid VerificationId { get; set; }
        public string NormalizedEmail { get; set; } = null!;
        public string OtpHash { get; set; } = null!;
        public int AttemptCount { get; set; }
        public DateTime OtpExpiresOnUtc { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}