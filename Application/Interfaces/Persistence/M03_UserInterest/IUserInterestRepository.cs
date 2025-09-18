using SocialOffice.Application.Interfaces.Persistence.Common;
using SocialOffice.Domain.Entitites.M03_UserInterest;

namespace SocialOffice.Application.Interfaces.Persistence.M03_UserInterest
{
    public interface IUserInterestRepository : IAsyncRepository<UserInterest>
    {
        Task<IEnumerable<UserInterest>> GetByUserIdAsync(Guid userId);
        Task<UserInterest?> GetByUserAndInterestAsync(Guid userId, Guid interestId);
    }
}
