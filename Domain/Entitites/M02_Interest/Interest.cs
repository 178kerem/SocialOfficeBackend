using SocialOffice.Domain.Entitites.M03_UserInterest;
using System.ComponentModel.DataAnnotations;

namespace SocialOffice.Domain.Entitites.M02_Interest
{
    public class Interest
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<UserInterest> UserInterest { get; set; } = new List<UserInterest>();
    }
}