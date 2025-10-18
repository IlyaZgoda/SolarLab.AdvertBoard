using SolarLab.AdvertBoard.Domain.Adverts;

namespace SolarLab.AdvertBoard.Domain.AdvertImages
{
    public class AdvertImage
    {
        public AdvertImageId Id { get; init; } = null!;

        public AdvertId AdvertId { get; init; } = null!;

        public ImageFileName FileName { get; init; } = null!;

        public ImageContentType ContentType { get; init; } = null!;

        public ImageContent Content { get; init; } = null!;

        public DateTime CreatedAt { get; init; }

        public static AdvertImage Create(AdvertId advertId, ImageFileName fileName, ImageContentType contentType, ImageContent content) =>
            new()
            {
                Id = new AdvertImageId(Guid.NewGuid()),
                AdvertId = advertId,
                FileName = fileName,
                ContentType = contentType,
                Content = content,
                CreatedAt = DateTime.UtcNow,  
            };
    }
}
