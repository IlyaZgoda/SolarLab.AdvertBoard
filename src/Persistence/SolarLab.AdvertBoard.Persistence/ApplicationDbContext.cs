using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.AdvertImages;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.Persistence
{
    /// <summary>
    /// Контекст базы данных для операций записи (CQRS Write side).
    /// </summary>
    /// <param name="options">Параметры конфигурации контекста.</param>
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext(options)
    {
        /// <summary>
        /// Набор данных пользователей.
        /// </summary>
        public DbSet<User> AppUsers { get; set; }

        /// <summary>
        /// Набор данных категорий.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Набор данных объявлений.
        /// </summary>
        public DbSet<Advert> Adverts { get; set; }

        /// <summary>
        /// Набор данных изображений.
        /// </summary>
        public DbSet<AdvertImage> Images { get; set; }

        /// <summary>
        /// Набор данных комментариев.
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
