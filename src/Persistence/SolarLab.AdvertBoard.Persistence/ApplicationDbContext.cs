using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext(options)
    {
        public DbSet<User> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
