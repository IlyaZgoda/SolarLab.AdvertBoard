using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Users.Specifications;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Adverts.CreateDraft
{
    /// <summary>
    /// Обработчик команды <see cref="CreateAdvertDraftCommand"/>.
    /// </summary>
    /// <param name="userIdentifierProvider">Провайдер для получения идентификатора текущего аутентифицированного пользователя</param>
    /// <param name="userRepository">Репозиторий для работы с пользователями</param>
    /// <param name="advertRepository">Репозиторий для работы с объявлениями</param>
    /// <param name="categoryRepository">Репозиторий для работы с категориями</param>
    /// <param name="unitOfWork">Unit of work</param>
    public class CreateAdvertDraftCommandHandler(
        IUserIdentifierProvider userIdentifierProvider, 
        IUserRepository userRepository, 
        IAdvertRepository advertRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<CreateAdvertDraftCommand, AdvertIdResponse>
    {
        /// <inheritdoc/>
        public async Task<Result<AdvertIdResponse>> Handle(CreateAdvertDraftCommand request, CancellationToken cancellationToken)
        {
            var identityId = userIdentifierProvider.IdentityUserId;
            var user = await userRepository.GetBySpecificationAsync(new UserWithIdentitySpecification(identityId));

            if (user.HasNoValue)
            {
                return Result.Failure<AdvertIdResponse>(UserErrors.NotFound);
            }

            var categoryId = new CategoryId(request.CategoryId);

            var category = await categoryRepository.GetByIdAsync(categoryId);

            if (category.HasNoValue)
            {
                return Result.Failure<AdvertIdResponse>(CategoryErrors.NotFound);
            }

            if (!category.Value.CanHostAdverts)
            {
                return Result.Failure<AdvertIdResponse>(CategoryErrors.CantHostAdverts);
            }

            var title = AdvertTitle.Create(request.Title);
            var description = AdvertDescription.Create(request.Description);
            var price = Price.Create(request.Price);

            var advertDataResult = Result.FirstFailureOrSuccess(title, description, price);

            if (advertDataResult.IsFailure)
            {
                return Result.Failure<AdvertIdResponse>(advertDataResult.Error);
            }

            var advert = Advert.CreateDraft(user.Value.Id, categoryId, title.Value, description.Value, price.Value);

            advertRepository.Add(advert);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AdvertIdResponse(advert.Id);
        }
    }
}
