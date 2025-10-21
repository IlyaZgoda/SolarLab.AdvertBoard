using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Persistence.Read.Models;

namespace SolarLab.AdvertBoard.Persistence.Read
{
    /// <summary>
    /// Контекст базы данных для операций чтения (CQRS Read side).
    /// </summary>
    /// <remarks>
    /// Настроен с отключенным отслеживанием изменений (NoTracking) для оптимизации производительности.
    /// Используется исключительно для запросов, не требующих отслеживания сущностей.
    /// </remarks>
    public class ReadDbContext : DbContext
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ReadDbContext"/>.
        /// </summary>
        /// <param name="options">Параметры конфигурации контекста.</param>
        public ReadDbContext(DbContextOptions<ReadDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        /// <summary>
        /// Набор данных пользователей.
        /// </summary>
        public DbSet<UserReadModel> Users { get; set; } = null!;

        /// <summary>
        /// Набор данных категорий.
        /// </summary>
        public DbSet<CategoryReadModel> Categories { get; set; } = null!;

        /// <summary>
        /// Набор данных объявлений.
        /// </summary>
        public DbSet<AdvertReadModel> Adverts { get; set; } = null!;

        /// <summary>
        /// Набор данных изображений.
        /// </summary>
        public DbSet<AdvertImageReadModel> Images { get; set; } = null!;

        /// <summary>
        /// Набор данных комментариев.
        /// </summary>
        public DbSet<CommentReadModel> Comments { get; set; } = null!;

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserReadModel>(entity =>
            {
                entity.ToTable("Users").HasKey(u => u.Id);

                entity.Property(u => u.FirstName).HasColumnName("FirstName");
                entity.Property(u => u.LastName).HasColumnName("LastName");
                entity.Property(u => u.MiddleName).HasColumnName("MiddleName");
                entity.Property(u => u.ContactEmail).HasColumnName("ContactEmail");
                entity.Property(u => u.PhoneNumber).HasColumnName("PhoneNumber");
                entity.Property(u => u.IdentityId).HasColumnName("IdentityId");
                entity.Property(u => u.CreatedAt).HasColumnName("CreatedAt");
            });

            builder.Entity<AdvertReadModel>(entity =>
            {
                entity.ToTable("Adverts").HasKey(a => a.Id);

                entity.Property(a => a.Title).HasColumnName("Title");
                entity.Property(a => a.Description).HasColumnName("Description");
                entity.Property(a => a.Price).HasColumnName("Price").HasColumnType("numeric(11,2)");
                entity.Property(a => a.Status).HasColumnName("Status").HasConversion<string>();
                entity.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                entity.Property(a => a.PublishedAt).HasColumnName("PublishedAt");
                entity.Property(a => a.UpdatedAt).HasColumnName("UpdatedAt");

                entity.HasOne(a => a.Author)
                    .WithMany(u => u.Adverts)
                    .HasForeignKey(a => a.AuthorId);

                entity.HasOne(a => a.Category)
                    .WithMany()
                    .HasForeignKey(a => a.CategoryId);
            });

            builder.Entity<AdvertImageReadModel>(entity =>
            {
                entity.ToTable("Images").HasKey(i => i.Id);

                entity.Property(i => i.FileName).HasColumnName("FileName");
                entity.Property(i => i.ContentType).HasColumnName("ContentType");
                entity.Property(i => i.Content).HasColumnName("Content");
                entity.Property(i => i.CreatedAt).HasColumnName("CreatedAt");

                entity.HasOne(i => i.Advert)
                    .WithMany(a => a.Images)
                    .HasForeignKey(i => i.AdvertId);
            });

            builder.Entity<CategoryReadModel>(entity =>
            {
                entity.ToTable("Categories").HasKey(c => c.Id);
                entity.Property(c => c.Title).HasColumnName("Title");
                entity.Property(c => c.ParentId).HasColumnName("ParentId");
            });

            builder.Entity<CommentReadModel>(entity =>
            {
                entity.ToTable("Comments").HasKey(c => c.Id);
                entity.Property(c => c.Text).HasColumnName("Text");
                entity.Property(c => c.CreatedAt).HasColumnName("CreatedAt");
                entity.Property(c => c.UpdatedAt).HasColumnName("UpdatedAt");

                entity.HasOne(c => c.Advert)
                     .WithMany(a => a.Comments)       
                     .HasForeignKey(c => c.AdvertId);

                entity.HasOne(c => c.Author)
                     .WithMany(u => u.Comments)      
                     .HasForeignKey(c => c.AuthorId);
            });
        }
    }
}
