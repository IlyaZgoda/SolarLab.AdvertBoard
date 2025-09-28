using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Categories;

namespace SolarLab.AdvertBoard.Application.Categories.GetTree
{
    public record GetCategoryTreeQuery() : IQuery<CategoryTreeResponse>;
}
