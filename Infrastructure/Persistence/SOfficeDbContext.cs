using Microsoft.EntityFrameworkCore;
using SocialOffice.Domain.Entitites.M01_User;
namespace SocialOffice.Infrastructure.Persistence;

public class SOfficeDbContext : DbContext
{
    public SOfficeDbContext(DbContextOptions<SOfficeDbContext> options) : base(options) { }

    // 01 - User 
    public DbSet<User> Users => Set<User>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SOfficeDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}