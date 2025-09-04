using Microsoft.EntityFrameworkCore;
using SocialOffice.Domain.Entitites.M01_User;
using SocialOffice.Infrastructure.Persistence.Repositories.Common;
using SocialOffice.Application.Interfaces.Persistence.M01_User;
namespace SocialOffice.Infrastructure.Persistence.Repositories.M01_User
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SOfficeDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users
                .Where(u => u.Email == email && !u.IsDeleted)

                .FirstOrDefaultAsync();
        }



        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Users

                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }
    }
}
