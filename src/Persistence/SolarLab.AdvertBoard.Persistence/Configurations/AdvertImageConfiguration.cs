using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarLab.AdvertBoard.Domain.AdvertImages;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;

namespace SolarLab.AdvertBoard.Persistence.Configurations
{
    internal class AdvertImageConfiguration : IEntityTypeConfiguration<AdvertImage>
    {
        public void Configure(EntityTypeBuilder<AdvertImage> builder)
        {
            builder.ToTable("Images");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasConversion(
                imageId => imageId.Id,
                value => new AdvertImageId(value));

            builder.Property(a => a.AdvertId)
                .HasConversion(
                advertId => advertId.Id,
                value => new AdvertId(value));

            builder.Property(a => a.FileName)
                .HasConversion(
                c => c.Value,
                value => ImageFileName.Create(value).Value)
                .IsRequired()
                .HasColumnName("FileName");

            builder.Property(a => a.ContentType)
                .HasConversion(
                c => c.Value,
                value => ImageContentType.Create(value).Value)
                .IsRequired()
                .HasColumnName("ContentType");

            builder.Property(a => a.Content)
                .HasConversion(
                c => c.Value,
                value => ImageContent.Create(value).Value)
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .IsRequired();
        }
    }
}
