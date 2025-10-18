using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Abstractions.ReadProviders;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Contracts.Users;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Persistence.Extensions;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Persistence.Read.Providers
{
    public class AdvertReadProvider(ApplicationDbContext context) : IAdvertReadProvider
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

            return await sortedBaseQuery.Select(q => new PublishedAdvertItem(
                q.Advert.Id,
                q.Advert.Title.Value,
                q.Advert.Description.Value,
                q.Advert.Price.Value,
                q.Advert.CategoryId,
                q.Category.Title.Value,
                q.Advert.AuthorId,
                q.User.FullName,
                q.Advert.PublishedAt.Value)).ToPagedAsync(filter.Page, filter.PageSize);
        }

        public async Task<PaginationCollection<AdvertDraftItem>> GetUserAdvertDrafts(string identityId, int page, int pageSize)
        {
            return await (from advert in context.Adverts.AsNoTracking()
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
                                 advert.AuthorId)).ToPagedAsync(page, pageSize);
        }

        public async Task<PaginationCollection<PublishedAdvertItem>> GetUserPublishedAdverts(string identityId, int page, int pageSize)
        {
            return await (from advert in context.Adverts.AsNoTracking()
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
                                 advert.PublishedAt.Value)).ToPagedAsync(page, pageSize);
        }
    }
}
