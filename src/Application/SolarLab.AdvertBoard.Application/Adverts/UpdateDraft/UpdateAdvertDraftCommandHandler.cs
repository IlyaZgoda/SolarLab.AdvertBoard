using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Application.Adverts.UpdateDraft
{
    /// <summary>
    /// Обработчик команды <see cref="UpdateAdvertDraftCommand"/>.
    /// </summary>
    /// <param name="userIdentifierProvider">Провайдер дял получения идентификатора текущего аутентифицированного пользователя.</param>
    /// <param name="userRepository">Репозиторий для работы с пользователями.</param>
    /// <param name="advertRepository">Репозиторий для работы с объявлениями.</param>
    /// <param name="categoryRepository">Репозиторий для работы с категориями.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    public class UpdateAdvertDraftCommandHandler(
        IUserIdentifierProvider userIdentifierProvider, 
        IUserRepository userRepository, 
        IAdvertRepository advertRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork) 
        : ICommandHandler<UpdateAdvertDraftCommand>
    {
        /// <inheritdoc/>
        public async Task<Result> Handle(UpdateAdvertDraftCommand request, CancellationToken cancellationToken)
        {
            var advert = await advertRepository.GetByIdAsync(new AdvertId(request.DraftId));

            if (advert.HasNoValue)
            {
                return Result.Failure(AdvertErrors.NotFound);
            }

            if (!await userRepository.IsOwner(new UserId(advert.Value.AuthorId), userIdentifierProvider.IdentityUserId))
            {
                return Result.Failure(AdvertErrors.NotFound);
            }

            if (request.CategoryId != null)
            {
                var category = await categoryRepository.GetByIdAsync(new CategoryId((Guid)request.CategoryId));

                if (category.HasNoValue)
                {
                    return Result.Failure(CategoryErrors.NotFound);
                }

                if (!category.Value.CanHostAdverts)
                {
                    return Result.Failure(CategoryErrors.CantHostAdverts);
                }
            }

            var categoryId = request.CategoryId is not null 
                ? new CategoryId(request.CategoryId!.Value) 
                : null; 

            var titleResult = request.Title is null 
                ? Result.Success<AdvertTitle?>(null) 
                : AdvertTitle.Create(request.Title)
                .MapNullable(t => t);

            var descriptionResult = request.Description is null
                ? Result.Success<AdvertDescription?>(null)
                : AdvertDescription.Create(request.Description)
                .MapNullable(d => d);

            var priceResult = request.Price is null
                ? Result.Success<Price?>(null)
                : Price.Create((decimal)request.Price)
                .MapNullable(p => p);

            var advertData = Result.FirstFailureOrSuccess(titleResult, descriptionResult, priceResult);

            if (advertData.IsFailure)
            {
                return Result.Failure(advertData.Error);
            }

            advert.Value.UpdateDraft(
                categoryId,
                titleResult.Value,
                descriptionResult.Value,
                priceResult.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
