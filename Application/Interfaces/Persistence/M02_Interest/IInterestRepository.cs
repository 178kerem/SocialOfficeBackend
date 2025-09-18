using SocialOffice.Application.Interfaces.Persistence.Common;
using SocialOffice.Domain.Entitites.M02_Interest;

namespace SocialOffice.Application.Interfaces.Persistence.M02_Interest
{
    public interface IInterestRepository : IAsyncRepository<Interest>
    {
      
        Task<Interest?> GetByNameAsync(string name);
        
    }
}
