using Microsoft.EntityFrameworkCore;
using SocialOffice.Application.Interfaces.Persistence.M02_Interest;
using SocialOffice.Domain.Entitites.M02_Interest;
using SocialOffice.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialOffice.Infrastructure.Persistence.Repositories.M02_Interest
{
    public class InterestRepository : Repository<Interest>, IInterestRepository
    {
        public InterestRepository(SOfficeDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Interest?> GetByNameAsync(string name)
        {
            return await _dbContext.Set<Interest>()
                .FirstOrDefaultAsync(i => i.Name == name);
        }
    }
}
