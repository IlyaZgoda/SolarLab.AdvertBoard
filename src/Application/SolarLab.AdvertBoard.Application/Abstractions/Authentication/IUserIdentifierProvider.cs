namespace SolarLab.AdvertBoard.Application.Abstractions.Authentication
{
    public interface IUserIdentifierProvider
    {
        string IdentityUserId { get; }
    }
}
