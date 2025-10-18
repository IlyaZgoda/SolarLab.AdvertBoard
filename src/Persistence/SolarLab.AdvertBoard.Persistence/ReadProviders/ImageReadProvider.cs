using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Abstractions.ReadProviders;
using SolarLab.AdvertBoard.Contracts.Images;
using SolarLab.AdvertBoard.Domain.AdvertImages;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Persistence.ReadProviders
{
    public class ImageReadProvider(ApplicationDbContext context) : IImageReadProvider
    {
        public async Task<Maybe<ImageResponse>> GetImageById(AdvertImageId id, string identityId)
        {
            var query = from image in context.Images.AsNoTracking()
                        join advert in context.Adverts.AsNoTracking()
                            on image.AdvertId equals advert.Id
                        join user in context.AppUsers.AsNoTracking()
                            on advert.AuthorId equals user.Id
                        where image.Id == id
                        select new
                        {
                            Image = image,
                            AdvertStatus = advert.Status,
                            IsOwner = user.IdentityId == identityId
                        };

            var result = await query.FirstOrDefaultAsync();

            if (result?.AdvertStatus == AdvertStatus.Published || result?.IsOwner == true)
            {
                return new ImageResponse(
                    result.Image.Content.Value,
                    result.Image.ContentType.Value,
                    result.Image.FileName.Value
                );
            }

            return Maybe<ImageResponse>.None;
        }
    }
}
