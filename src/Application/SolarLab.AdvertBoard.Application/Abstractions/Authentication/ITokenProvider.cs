namespace SolarLab.AdvertBoard.Application.Abstractions.Authentication
{
    public interface ITokenProvider
    {
        string Create(string id, string email);
    }
}
