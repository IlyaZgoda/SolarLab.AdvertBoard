using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Categories;

namespace SolarLab.AdvertBoard.Application.Categories.GetById
{
    /// <summary>
    /// Запрос для получения категории по идентификатору.
    /// </summary>
    /// <param name="Id">Идентификатор категории.</param>
    public record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryResponse>;
}
