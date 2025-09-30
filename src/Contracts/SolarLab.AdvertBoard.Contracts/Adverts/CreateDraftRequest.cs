namespace SolarLab.AdvertBoard.Contracts.Adverts
{
    public record CreateDraftRequest(Guid CategoryId, string Title, string Description, decimal Price);
}
