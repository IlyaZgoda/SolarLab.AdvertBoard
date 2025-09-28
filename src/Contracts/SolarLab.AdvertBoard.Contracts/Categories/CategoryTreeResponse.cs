namespace SolarLab.AdvertBoard.Contracts.Categories
{
    public record CategoryTreeResponse(IReadOnlyList<CategoryNode> Categories);

    public record CategoryNode(Guid Id, string Title, List<CategoryNode> Children);
}
