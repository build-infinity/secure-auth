namespace SecureAuth.Domain.Models
{
    public class UserRole
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime CreatedOnOtc { get; set; }
    }
}