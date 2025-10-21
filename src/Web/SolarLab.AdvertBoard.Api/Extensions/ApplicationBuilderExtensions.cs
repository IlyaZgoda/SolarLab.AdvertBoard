using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Persistence;
using SolarLab.AdvertBoard.Persistence.Seeders;
using System.Threading.Tasks;

namespace SolarLab.AdvertBoard.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Применяет миграции.
        /// </summary>
        /// <param name="app">Билдер приложения.</param>
        /// <returns>Билдер приложения.</returns>
        public static async Task ApplyMigrationsAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();
        }

        /// <summary>
        /// Сидит начальные данные.
        /// </summary>
        /// <param name="app">Билдер приложения.</param>
        /// <returns>Билдер приложения.</returns>
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<CategorySeeder>();

            await seeder.SeedAsync();
        }
    }
}
