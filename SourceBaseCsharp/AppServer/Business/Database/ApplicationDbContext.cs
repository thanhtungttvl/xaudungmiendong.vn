using AppServer.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppServer.Business.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasData(ApplicationSeedData.Users);

            base.OnModelCreating(modelBuilder);
        }
    }
}
