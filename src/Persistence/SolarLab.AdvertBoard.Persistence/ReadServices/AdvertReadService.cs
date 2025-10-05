using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Abstractions.ReadServices;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Users;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Maybe;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<Maybe<PublishedAdvertsResponse>> GetPublishedAdvertsByFilterAsync(int page, int pageSize)
        {
            var baseQuery = (from advert in context.Adverts.AsNoTracking()
                                   join category in context.Categories.AsNoTracking()
                                   on advert.CategoryId equals category.Id
                                   join user in context.AppUsers.AsNoTracking()
                                   on advert.AuthorId equals user.Id
                                   where advert.Status == AdvertStatus.Published
                                   orderby advert.PublishedAt descending
                                   select new PublishedAdvertItem(
                                       advert.Id, 
                                       advert.Title.Value, 
                                       advert.Description.Value, 
                                       advert.CategoryId, 
                                       category.Title.Value, 
                                       advert.AuthorId, 
                                       user.FullName, 
                                       advert.PublishedAt.Value)).AsQueryable();

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var response = new PublishedAdvertsResponse(items, page, pageSize, totalCount, (int)Math.Ceiling(totalCount / (double)pageSize));

            return response;
        }

        public async Task<Maybe<AdvertDraftsResponse>> GetUserAdvertDrafts(string identityId, int page, int pageSize)
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
                                 advert.AuthorId)).AsQueryable();

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var response = new AdvertDraftsResponse(items, page, pageSize, totalCount, (int)Math.Ceiling(totalCount / (double)pageSize));

            return response;
        }
    }
}
