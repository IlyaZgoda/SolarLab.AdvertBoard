namespace SolarLab.AdvertBoard.Contracts.Categories
{
    public record CategoryResponse(Guid Id, Guid? ParentId, string Title);
}
