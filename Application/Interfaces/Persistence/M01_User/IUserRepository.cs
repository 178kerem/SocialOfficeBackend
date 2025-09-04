using SocialOffice.Application.Interfaces.Persistence.Common;
using SocialOffice.Domain.Entitites.M01_User;
namespace SocialOffice.Application.Interfaces.Persistence.M01_User
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
    }
}