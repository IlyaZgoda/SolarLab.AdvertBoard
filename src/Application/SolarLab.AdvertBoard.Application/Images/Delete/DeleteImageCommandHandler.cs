using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Domain.AdvertImages;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Images.Delete
{
    public class DeleteImageCommandHandler(
        IAdvertRepository advertRepository, 
        IUnitOfWork unitOfWork, 
        IUserRepository userRepository,
        IUserIdentifierProvider userIdentifierProvider) : ICommandHandler<DeleteImageCommand>
    {
        public async Task<Result> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
        {
            var advert = await advertRepository.GetByIdAsync(new AdvertId(request.AdvertId));

            if (advert.HasNoValue)
            {
                return Result.Failure(AdvertErrors.NotFound);
            }

            if (!await userRepository.IsOwner(advert.Value.AuthorId, userIdentifierProvider.IdentityUserId))
            {
                return Result.Failure(AdvertErrors.NotFound);
            }

            var result = advert.Value.DeleteImage(new AdvertImageId(request.Id));

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            advertRepository.Update(advert.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
