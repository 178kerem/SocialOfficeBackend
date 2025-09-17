using Microsoft.EntityFrameworkCore;
using SocialOffice.Domain.Entitites.M01_User;
using SocialOffice.Domain.Entitites.M02_Interest;
using SocialOffice.Domain.Entitites.M03_UserInterest;

namespace SocialOffice.Infrastructure.Persistence
{
    public class SOfficeDbContext : DbContext
    {
        public SOfficeDbContext(DbContextOptions<SOfficeDbContext> options) : base(options) { }

        // DbSets
        public DbSet<User> Users => Set<User>();
        public DbSet<Interest> Interests => Set<Interest>();
        public DbSet<UserInterest> UserInterests => Set<UserInterest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations in the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SOfficeDbContext).Assembly);

            // UserInterest composite key ve ilişkiler
            modelBuilder.Entity<UserInterest>(entity =>
            {
                entity.HasKey(ui => new { ui.UserId, ui.InterestId });

                entity.HasOne(ui => ui.User)
                      .WithMany(u => u.UserInterests)
                      .HasForeignKey(ui => ui.UserId);

                entity.HasOne(ui => ui.Interest)
                      .WithMany(i => i.UserInterest)
                      .HasForeignKey(ui => ui.InterestId);
            });
        }
    }
}
