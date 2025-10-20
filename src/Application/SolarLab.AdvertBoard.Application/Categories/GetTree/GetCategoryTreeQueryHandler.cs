using Microsoft.Extensions.Logging;
using SolarLab.AdvertBoard.Application.Abstractions.Caching;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Categories;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel.Result;
using System.Text.Json;

namespace SolarLab.AdvertBoard.Application.Categories.GetTree
{
    public class GetCategoryTreeQueryHandler(
        ICategoryRepository categoryRepository, 
        ICacheProvider cache, 
        ILogger<GetCategoryTreeQueryHandler> logger)
        : IQueryHandler<GetCategoryTreeQuery, CategoryTreeResponse>
    {
        private const string CacheKey = "category_tree";

        public async Task<Result<CategoryTreeResponse>> Handle(GetCategoryTreeQuery request, CancellationToken cancellationToken)
        {
            var cached = await cache.GetAsync<CategoryTreeResponse>(CacheKey, cancellationToken);

            if (cached is not null)
            {
                logger.LogInformation("Category tree returned from cache");
                return cached;
            }

            logger.LogInformation("Cache miss. Loading categories from database...");

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

            var response = new CategoryTreeResponse(roots);

            await cache.SetAsync(CacheKey, response, TimeSpan.FromHours(6), cancellationToken);

            logger.LogInformation("Category tree cached for 6 hours");

            return response;
        }
    }
}
