using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Abstractions.ReadServices;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Contracts.Users;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Persistence.ReadServices
{
    public class AdvertReadService(ApplicationDbContext context) : IAdvertReadService
    {
        public async Task<Maybe<AdvertDraftDetailsResponse>> GetAdvertDraftDetailsByIdAsync(AdvertId id) =>
            await (from advert in context.Adverts.AsNoTracking()
                          join category in context.Categories.AsNoTracking()
                          on advert.CategoryId equals category.Id
                          where advert.Id == id.Id && advert.Status == AdvertStatus.Draft
                          select new AdvertDraftDetailsResponse(
                              advert.Id,
                              advert.Title.Value,             
                              advert.Description.Value,       
                              advert.Price.Value,  
                              advert.CategoryId,
                              category.Title.Value,
                              advert.Status.ToString(),       
                              advert.CreatedAt,               
                              advert.UpdatedAt,
                              advert.AuthorId
                          )).SingleOrDefaultAsync();

        public async Task<Maybe<PublishedAdvertDetailsResponse>> GetPublishedAdvertDetailsByIdAsync(AdvertId id) =>
            await (from advert in context.Adverts.AsNoTracking()
                   join category in context.Categories.AsNoTracking()
                   on advert.CategoryId equals category.Id
                   join user in context.AppUsers.AsNoTracking()
                   on advert.AuthorId equals user.Id
                   where advert.Id == id.Id && advert.Status == AdvertStatus.Published
                   select new PublishedAdvertDetailsResponse(
                       advert.Id,
                       advert.Title.Value,
                       advert.Description.Value,
                       advert.Price.Value,
                       advert.CategoryId,
                       category.Title.Value,
                       advert.PublishedAt!.Value,
                       advert.AuthorId,
                       new UserContactInfoResponse(
                           user.FullName, 
                           user.ContactEmail.Value, 
                           user.PhoneNumber.Value))).SingleOrDefaultAsync();

        public record AdvertView
        {
            public Advert Advert { get; set; } = null!;
            public Category Category { get; set; } = null!;
            public User User { get; set; } = null!;
        }

        public async Task<PaginationCollection<PublishedAdvertItem>> GetPublishedAdvertsByFilterAsync(AdvertFilterRequest filter)
        {
            var baseQuery = (from advert in context.Adverts.AsNoTracking()
                             join category in context.Categories.AsNoTracking()
                             on advert.CategoryId equals category.Id
                             join user in context.AppUsers.AsNoTracking()
                             on advert.AuthorId equals user.Id
                             where advert.Status == AdvertStatus.Published
                             select new { Advert=advert, Category=category, User=user });

            if (filter.CategoryId.HasValue)
                baseQuery = baseQuery.Where(x => x.Advert.CategoryId == filter.CategoryId.Value);

            if (filter.AuthorId.HasValue)
                baseQuery = baseQuery.Where(x => x.Advert.AuthorId == filter.AuthorId.Value);

            if (filter.MinPrice.HasValue)
                baseQuery = baseQuery.Where(x => (decimal)x.Advert.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                baseQuery = baseQuery.Where(x => (decimal)x.Advert.Price <= filter.MaxPrice.Value);

            if (!string.IsNullOrEmpty(filter.SearchText))
                baseQuery = baseQuery.Where(x =>
                    ((string)x.Advert.Title).Contains(filter.SearchText) ||
                    ((string)x.Advert.Description).Contains(filter.SearchText));

            var sortedBaseQuery = filter.SortBy?.ToLower() switch
            {
                "price" => filter.SortDescending
                    ? baseQuery.OrderByDescending(x => x.Advert.Price)
                    : baseQuery.OrderBy(x => x.Advert.Price),
                "title" => filter.SortDescending
                    ? baseQuery.OrderByDescending(x => x.Advert.Title)
                    : baseQuery.OrderBy(x => x.Advert.Title),
                "publishedat" => filter.SortDescending
                    ? baseQuery.OrderByDescending(x => x.Advert.PublishedAt)
                    : baseQuery.OrderBy(x => x.Advert.PublishedAt),
                _ => baseQuery.OrderByDescending(x => x.Advert.PublishedAt)
            };

            var projection = sortedBaseQuery.Select(q => new PublishedAdvertItem(
                q.Advert.Id,
                q.Advert.Title.Value,
                q.Advert.Description.Value,
                q.Advert.Price.Value,
                q.Advert.CategoryId,
                q.Category.Title.Value,
                q.Advert.AuthorId,
                q.User.FullName,
                q.Advert.PublishedAt.Value));

            var totalCount = await projection.CountAsync();

            var items = await projection
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var response = new PaginationCollection<PublishedAdvertItem>(
                items, 
                filter.Page, 
                filter.PageSize, 
                totalCount, 
                (int)Math.Ceiling(totalCount / (double)filter.PageSize));

            return response;
        }

        public async Task<PaginationCollection<AdvertDraftItem>> GetUserAdvertDrafts(string identityId, int page, int pageSize)
        {
            var baseQuery = (from advert in context.Adverts.AsNoTracking()
                             join category in context.Categories.AsNoTracking()
                             on advert.CategoryId equals category.Id
                             join user in context.AppUsers.AsNoTracking()
                             on advert.AuthorId equals user.Id
                             where user.IdentityId == identityId && advert.Status == AdvertStatus.Draft
                             orderby advert.PublishedAt descending
                             select new AdvertDraftItem(
                                 advert.Id,
                                 advert.Title.Value,
                                 advert.Description.Value,
                                 advert.Price.Value,
                                 advert.CategoryId,
                                 category.Title.Value,
                                 advert.CreatedAt,
                                 advert.UpdatedAt.Value,
                                 advert.AuthorId));

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var response = new PaginationCollection<AdvertDraftItem>(
                items, 
                page, 
                pageSize, 
                totalCount, 
                (int)Math.Ceiling(totalCount / (double)pageSize));

            return response;
        }

        public async Task<PaginationCollection<PublishedAdvertItem>> GetUserPublishedAdverts(string identityId, int page, int pageSize)
        {
            var baseQuery = (from advert in context.Adverts.AsNoTracking()
                             join category in context.Categories.AsNoTracking()
                             on advert.CategoryId equals category.Id
                             join user in context.AppUsers.AsNoTracking()
                             on advert.AuthorId equals user.Id
                             where user.IdentityId == identityId && advert.Status == AdvertStatus.Published
                             orderby advert.PublishedAt descending
                             select new PublishedAdvertItem(
                                 advert.Id,
                                 advert.Title.Value,
                                 advert.Description.Value,
                                 advert.Price.Value,
                                 advert.CategoryId,
                                 category.Title.Value,
                                 advert.AuthorId,
                                 user.FullName,
                                 advert.PublishedAt.Value)).AsQueryable();

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var response = new PaginationCollection<PublishedAdvertItem>(
                items, 
                page, 
                pageSize, 
                totalCount, 
                (int)Math.Ceiling(totalCount / (double)pageSize));

            return response;
        }
    }
}
