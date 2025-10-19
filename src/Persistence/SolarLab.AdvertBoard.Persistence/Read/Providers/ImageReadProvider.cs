using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Abstractions.Read.Providers;
using SolarLab.AdvertBoard.Contracts.Images;
using SolarLab.AdvertBoard.Domain.AdvertImages;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Persistence.Read.Providers
{
    public class ImageReadProvider(ReadDbContext context) : IImageReadProvider
    {
        public async Task<Maybe<ImageResponse>> GetImageById(AdvertImageId id, string identityId)
        {
            var image = await context.Images
              .Include(i => i.Advert)
                  .ThenInclude(a => a.Author)
              .FirstOrDefaultAsync(i => i.Id == id);

            if (image == null) return Maybe<ImageResponse>.None;

            bool isOwner = image.Advert.Author.IdentityId == identityId;
            if (image.Advert.Status == AdvertStatus.Published || isOwner)
            {
                return new ImageResponse(image.Content, image.ContentType, image.FileName);
            }

            return Maybe<ImageResponse>.None;
        }
    }
}
