using SocialOffice.Domain.Entitites.Shared;
namespace SocialOffice.Domain.Entitites.M01_User
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime? LastLoginAt { get; set; }
        public string? LastLoginIp { get; set; }
    }
}