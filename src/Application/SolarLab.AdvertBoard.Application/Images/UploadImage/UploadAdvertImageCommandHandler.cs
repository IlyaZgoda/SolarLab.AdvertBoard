using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Images;
using SolarLab.AdvertBoard.Domain.AdvertImages;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Images.UploadImage
{
    public class UploadAdvertImageCommandHandler(IAdvertRepository advertRepository, IUnitOfWork unitOfWork) 
        : ICommandHandler<UploadAdvertImageCommand, ImageIdResponse>
    {
        public async Task<Result<ImageIdResponse>> Handle(UploadAdvertImageCommand request, CancellationToken cancellationToken)
        {
            var advert = await advertRepository.GetByIdAsync(new AdvertId(request.AdvertId));

            if (advert.HasNoValue)
            {
                return Result.Failure<ImageIdResponse>(AdvertErrors.NotFound);
            }

            var fileName = ImageFileName.Create(request.FileName);
            var contentType = ImageContentType.Create(request.ContentType);
            var content = ImageContent.Create(request.Content);

            var imageDataResult = Result.FirstFailureOrSuccess(fileName, contentType, content);

            if (imageDataResult.IsFailure)
            {
                return Result.Failure<ImageIdResponse>(imageDataResult.Error);
            }

            var imageId = advert.Value.AddImage(fileName.Value, contentType.Value, content.Value);

            advertRepository.Update(advert.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new ImageIdResponse(imageId.Value));
        }
    }
}
