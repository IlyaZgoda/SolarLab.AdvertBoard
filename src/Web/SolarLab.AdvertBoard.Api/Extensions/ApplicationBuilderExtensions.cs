using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Persistence;
using SolarLab.AdvertBoard.Persistence.Seeders;
using System.Threading.Tasks;

namespace SolarLab.AdvertBoard.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task ApplyMigrationsAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();
        }

        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<CategorySeeder>();

            await seeder.SeedAsync();
        }
    }
}
