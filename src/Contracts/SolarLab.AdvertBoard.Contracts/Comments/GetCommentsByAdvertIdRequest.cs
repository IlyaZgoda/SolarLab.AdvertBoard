namespace SolarLab.AdvertBoard.Contracts.Comments
{
    public record GetCommentsByAdvertIdRequest(int Page = 1, int PageSize = 20);
}
