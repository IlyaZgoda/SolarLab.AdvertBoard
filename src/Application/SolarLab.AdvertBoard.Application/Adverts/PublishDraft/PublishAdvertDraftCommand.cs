using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Adverts.PublishDraft
{
    public record PublishAdvertDraftCommand(Guid Id) : ICommand;

    public class PublishAdvertDraftCommandHandler(
        IAdvertRepository advertRepository,
        IUnitOfWork unitOfWork,
        IUserIdentifierProvider userIdentifierProvider,
        IUserRepository userRepository) : ICommandHandler<PublishAdvertDraftCommand>
    {
        public async Task<Result> Handle(PublishAdvertDraftCommand request, CancellationToken cancellationToken)
        {
            var advert = await advertRepository.GetByIdAsync(new AdvertId(request.Id));

            if (advert.HasNoValue 
                || await userRepository.GetIdentityIdByUserIdAsync(new UserId(advert.Value.AuthorId))
                != userIdentifierProvider.IdentityUserId)
            {
                return Result.Failure(AdvertErrors.NotFound);
            }

            advert.Value.Publish();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
