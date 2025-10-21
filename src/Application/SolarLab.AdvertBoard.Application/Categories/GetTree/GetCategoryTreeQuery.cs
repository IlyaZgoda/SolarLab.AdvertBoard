using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Categories;

namespace SolarLab.AdvertBoard.Application.Categories.GetTree
{
    /// <summary>
    /// Запрос для получения дерева категорий по идентификатору.
    /// </summary>
    public record GetCategoryTreeQuery() : IQuery<CategoryTreeResponse>;
}
