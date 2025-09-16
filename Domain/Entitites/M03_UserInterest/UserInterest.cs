using SocialOffice.Domain.Entitites.M01_User;
using SocialOffice.Domain.Entitites.M02_Interest;

namespace SocialOffice.Domain.Entitites.M03_UserInterest
{
    public class UserInterest
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid InterestId { get; set; }
        public Interest Interest { get; set; } = null!;
    }
}