using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Categories;

namespace SolarLab.AdvertBoard.Application.Categories.GetById
{
    public record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryResponse>;
}
