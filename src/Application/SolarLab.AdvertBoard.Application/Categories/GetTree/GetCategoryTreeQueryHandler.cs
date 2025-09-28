using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Categories;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Categories.GetTree
{
    public class GetCategoryTreeQueryHandler(ICategoryRepository categoryRepository)
        : IQueryHandler<GetCategoryTreeQuery, CategoryTreeResponse>
    {
        public async Task<Result<CategoryTreeResponse>> Handle(GetCategoryTreeQuery request, CancellationToken cancellationToken)
        {
            var categories = await categoryRepository.GetAllAsync();

            if (categories.Count == 0)
            {
                return Result.Failure<CategoryTreeResponse>(CategoryErrors.NotFound);
            }

            var nodeMap = categories.ToDictionary(c => c.Id, c => new CategoryNode(c.Id, c.Title.Value, []));

            foreach (var category in categories.Where(c => c.ParentId is not null))
            {
                if (nodeMap.TryGetValue(category.ParentId!, out var parent))
                {
                    parent.Children.Add(nodeMap[category.Id]);
                }
            }

            var roots = categories
                .Where(c => c.ParentId is null)
                .Select(c => nodeMap[c.Id])
                .ToList();

            return new CategoryTreeResponse(roots);
        }
    }
}
