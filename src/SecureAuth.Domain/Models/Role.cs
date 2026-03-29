namespace SecureAuth.Domain.Models
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}