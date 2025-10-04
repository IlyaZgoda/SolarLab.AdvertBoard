namespace SolarLab.AdvertBoard.Contracts.Adverts
{
    public record CreateAdvertDraftRequest(Guid CategoryId, string Title, string Description, decimal Price);
}
