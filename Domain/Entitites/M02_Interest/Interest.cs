using SocialOffice.Domain.Entitites.M03_UserInterest;
using SocialOffice.Domain.Entitites.Shared;
using System.ComponentModel.DataAnnotations;

namespace SocialOffice.Domain.Entitites.M02_Interest
{
    public class Interest : BaseEntity
    {
        
        public string Name { get; set; } = string.Empty;

        public ICollection<UserInterest> UserInterest { get; set; } = new List<UserInterest>();
    }
}