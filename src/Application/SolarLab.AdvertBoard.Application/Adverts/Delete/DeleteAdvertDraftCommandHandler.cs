using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Adverts.Delete
{
    public class DeleteAdvertDraftCommandHandler(
        IAdvertRepository advertRepository, 
        IUnitOfWork unitOfWork, 
        IUserIdentifierProvider userIdentifierProvider, 
        IUserRepository userRepository) : ICommandHandler<DeleteAdvertDraftCommand>
    {
        public async Task<Result> Handle(DeleteAdvertDraftCommand request, CancellationToken cancellationToken)
        {
            var draft = await advertRepository.GetByIdAsync(new AdvertId(request.Id));

            if (draft.HasNoValue)
            {
                return Result.Failure(AdvertErrors.NotFound);
            }

            if (!await userRepository.IsOwner(new UserId(draft.Value.AuthorId), userIdentifierProvider.IdentityUserId))
            {
                return Result.Failure(AdvertErrors.NotFound);
            }

            if (draft.Value.Status != AdvertStatus.Draft)
            {
                return Result.Failure(AdvertErrors.CantDeleteNonDraftAdvert);
            }

            advertRepository.Delete(draft.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
