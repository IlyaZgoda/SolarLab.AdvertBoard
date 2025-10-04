namespace SolarLab.AdvertBoard.Contracts.Adverts
{
    public record UpdateAdvertDraftRequest(Guid? CategoryId, string? Title, string? Description, decimal? Price);
}
