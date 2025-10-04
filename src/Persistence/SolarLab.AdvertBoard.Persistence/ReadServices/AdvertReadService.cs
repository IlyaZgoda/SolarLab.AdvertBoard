using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Abstractions.ReadServices;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Persistence.ReadServices
{
    public class AdvertReadService(ApplicationDbContext context) : IAdvertReadService
    {
        public async Task<Maybe<AdvertDraftResponse>> GetAdvertDraftDetailsByIdAsync(AdvertId id)
        {
            return await (from advert in context.Adverts.AsNoTracking()
                          join category in context.Categories.AsNoTracking()
                          on advert.CategoryId equals category.Id
                          where advert.Id == id.Id && advert.Status == AdvertStatus.Draft
                          select new AdvertDraftResponse(
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
        }
    }
}
