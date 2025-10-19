using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;
using SolarLab.AdvertBoard.Application.Abstractions.Read.Providers;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Persistence.Builders;
using SolarLab.AdvertBoard.Persistence.Extensions;
using SolarLab.AdvertBoard.Persistence.Read.Models;
using SolarLab.AdvertBoard.SharedKernel.Maybe;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Persistence.Read.Providers
{
    public class AdvertReadProvider(ReadDbContext context) : IAdvertReadProvider
    {
        public async Task<Maybe<AdvertDraftDetailsResponse>> GetAdvertDraftDetailsByIdAsync(Specification<IAdvertReadModel> spec)
        {
            var efSpec = spec.ToEf<AdvertReadModel, IAdvertReadModel>();

            return await context.Adverts
                .Include(a => a.Category)
                .Include(a => a.Author)
                .Where(efSpec)
                .ToAdvertDraftDetails()
                .SingleOrDefaultAsync();
        }

        public async Task<Maybe<PublishedAdvertDetailsResponse>> GetPublishedAdvertDetailsByIdAsync(Specification<IAdvertReadModel> spec)
        {
            var efSpec = spec.ToEf<AdvertReadModel, IAdvertReadModel>();

            return await context.Adverts
                .Include(a => a.Category)
                .Include(a => a.Author)
                .Where(efSpec)
                .ToPublishedAdvertDetails()
                .SingleOrDefaultAsync();
        }

        public async Task<PaginationCollection<PublishedAdvertItem>> GetPublishedAdvertsByFilterAsync(AdvertFilterRequest filter)
        {
            var queryBuilder = new AdvertQueryBuilder(context);

            return await queryBuilder
                .WithCategory()
                .WithAuthor()
                .WherePublished()
                .FilterByCategory(filter.CategoryId)
                .FilterByAuthor(filter.AuthorId)
                .FilterByPriceRange(filter.MinPrice, filter.MaxPrice)
                .Search(filter.SearchText)
                .Sort(filter.SortBy, filter.SortDescending)
                .Build()
                .ToPublishedAdvertItem()
                .ToPagedAsync(filter.Page, filter.PageSize);
        }

        public async Task<PaginationCollection<AdvertDraftItem>> GetUserAdvertDrafts(
            int page, 
            int pageSize, 
            Specification<IAdvertReadModel> spec)
        {
            var efSpec = spec.ToEf<AdvertReadModel, IAdvertReadModel>();

            var query = context.Adverts
                .Include(a => a.Category)
                .Include(a => a.Author)
                .Where(efSpec)
                .OrderByDescending(a => a.CreatedAt);

            return await query
                .ToAdvertDraftItem()
                .ToPagedAsync(page, pageSize);
        }

        public async Task<PaginationCollection<PublishedAdvertItem>> GetUserPublishedAdverts(
            int page, 
            int pageSize, 
            Specification<IAdvertReadModel> spec)
        {
            var efSpec = spec.ToEf<AdvertReadModel, IAdvertReadModel>();

            var query = context.Adverts
                .Include(a => a.Category)
                .Include(a => a.Author)
                .Where(efSpec)
                .OrderByDescending(a => a.PublishedAt);

            return await query
                .ToPublishedAdvertItem()
                .ToPagedAsync(page, pageSize);
        }
    }
}
