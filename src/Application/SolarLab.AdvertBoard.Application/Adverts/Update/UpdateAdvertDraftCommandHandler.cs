using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Application.Adverts.Update
{
    public class UpdateAdvertDraftCommandHandler(
        IUserIdentifierProvider userIdentifierProvider, 
        IUserRepository userRepository, 
        IAdvertRepository advertRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork) 
        : ICommandHandler<UpdateAdvertDraftCommand>
    {
        public async Task<Result> Handle(UpdateAdvertDraftCommand request, CancellationToken cancellationToken)
        {
            var identityId = userIdentifierProvider.IdentityUserId;
            var user = await userRepository.GetByUserIdentityIdAsync(identityId);
            var advert = await advertRepository.GetById(new AdvertId(request.DraftId));

            if (user.HasNoValue)
            {
                return Result.Failure<AdvertIdResponse>(UserErrors.NotFound);
            }

            if (advert.HasNoValue || advert.Value.AuthorId != user.Value.Id)
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
                Console.WriteLine(category.Value.Childrens.Count);
                Console.WriteLine(category.Value.CanHostAdverts);
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
