namespace SecureAuth.Domain.Models
{
    public class RefreshToken
    {
        public Guid TokenId { get; set; }
        public string TokenHash { get; set; } = null!;
        public DateTime ExpiresOnUtc { get; set; }
        public bool IsRevoked { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime CreatedOnUtc { get; set; }
    }
}