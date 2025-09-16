namespace SocialOffice.Application.DTOs.M04_UserInterest
{
    public class UserInterestDto
    {
        public Guid UserId { get; set; }
        public List<Guid> InterestIds { get; set; } = new();
    }
}
