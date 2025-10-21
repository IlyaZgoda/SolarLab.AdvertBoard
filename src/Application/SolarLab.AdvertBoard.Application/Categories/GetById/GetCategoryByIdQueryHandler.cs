using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Categories;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel.Maybe;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Categories.GetById
{
    /// <summary>
    /// Обработчик запроса <see cref="GetCategoryByIdQuery"/>.
    /// </summary>
    /// <param name="categoryRepository">Репозиторий для работы с категориями.</param>
    public class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository) 
        : IQueryHandler<GetCategoryByIdQuery, CategoryResponse>
    {
        /// <inheritdoc/>
        public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            Maybe<Category> category = await categoryRepository.GetByIdAsync(new CategoryId(request.Id));

            if (category.HasNoValue)
            {
                return Result.Failure<CategoryResponse>(CategoryErrors.NotFound);
            }

            return new CategoryResponse(category.Value.Id, category.Value.ParentId?.Id, category.Value.Title.Value);
        }
    }
}
