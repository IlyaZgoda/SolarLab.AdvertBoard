using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.ReadProviders;
using SolarLab.AdvertBoard.Contracts.Images;
using SolarLab.AdvertBoard.Domain.AdvertImages;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Images.GetById
{
    public class GetImageByIdQueryHandler(
        IImageReadProvider imageReadProvider, 
        IUserIdentifierProvider userIdentifierProvider) : IQueryHandler<GetImageByIdQuery, ImageResponse>
    {
        public async Task<Result<ImageResponse>> Handle(GetImageByIdQuery request, CancellationToken cancellationToken)
        {
            var image = await imageReadProvider.GetImageById(new AdvertImageId(request.Id), userIdentifierProvider.IdentityUserId);

            if (image.HasNoValue)
            {
                return Result.Failure<ImageResponse>(AdvertImageErrors.NotFound);
            }

            return Result.Success(image.Value);
        }
    }
}
