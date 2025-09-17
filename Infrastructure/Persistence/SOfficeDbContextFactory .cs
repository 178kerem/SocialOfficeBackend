using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SocialOffice.Infrastructure.Persistence
{
    public class SOfficeDbContextFactory : IDesignTimeDbContextFactory<SOfficeDbContext>
    {
        public SOfficeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SOfficeDbContext>();

            // PostgreSQL connection string
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=SosyalOfis;Username=postgres;Password=your_password");

            return new SOfficeDbContext(optionsBuilder.Options);
        }
    }
}
