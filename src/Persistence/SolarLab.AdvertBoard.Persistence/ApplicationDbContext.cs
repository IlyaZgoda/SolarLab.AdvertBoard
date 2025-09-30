using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext(options)
    {
        public DbSet<User> AppUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Advert> Adverts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
