using SolarLab.AdvertBoard.Contracts.Users;

namespace SolarLab.AdvertBoard.Contracts.Adverts
{
    public record PublishedAdvertDetailsResponse(
        Guid Id,
        string Title, 
        string Description, 
        decimal Price, 
        Guid CategoryId, 
        string CategoryTitle, 
        DateTime PublishedAt, 
        Guid AuthorId, 
        UserContactInfoResponse AuthorContacts);
}
