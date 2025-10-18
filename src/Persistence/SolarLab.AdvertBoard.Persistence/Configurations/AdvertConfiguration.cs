using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarLab.AdvertBoard.Domain.AdvertImages;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.Persistence.Configurations
{
    internal class AdvertConfiguration : IEntityTypeConfiguration<Advert>
    {
        public void Configure(EntityTypeBuilder<Advert> builder)
        {
            builder.ToTable("Adverts");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasConversion(
                advertId => advertId.Id,
                value => new AdvertId(value));

            builder.Property(a => a.AuthorId)
                .HasConversion(
                userId => userId.Id,
                value => new UserId(value));

            builder.Property(a => a.CategoryId)
                .HasConversion(
                categoryId => categoryId.Id,
                value => new CategoryId(value));

            builder.Property(a => a.Title)
                .HasConversion(
                c => c.Value,
                value => AdvertTitle.Create(value).Value)
                .IsRequired()
                .HasMaxLength(AdvertTitle.MaxLength)  
                .HasColumnName("Title");

            builder.Property(a => a.Description)
                .HasConversion(
                c => c.Value,
                value => AdvertDescription.Create(value).Value)
                .IsRequired()
                .HasMaxLength(AdvertDescription.MaxLength)
                .HasColumnName("Description");

            builder.Property(a => a.Price)
                .HasConversion(
                c => c.Value,
                value => Price.Create(value).Value)
                .IsRequired()
                .HasColumnType("numeric(11,2)");

            builder.Property(a => a.Status)
               .HasConversion<string>()
               .HasMaxLength(20)
               .IsRequired();

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.PublishedAt)
                .IsRequired(false);

            builder.Property(a => a.UpdatedAt)
                .IsRequired(false);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.AuthorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasMany(a => a.Images)
               .WithOne()
               .HasForeignKey(i => i.AdvertId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(a => a.Images)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(a => a.AuthorId);
            builder.HasIndex(a => a.CategoryId);
            builder.HasIndex(a => a.Status);
        }
    }
}
