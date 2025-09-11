namespace SolarLab.AdvertBoard.Domain.Exceptions
{
    public class DomainException(string message, Exception? inner = default) : Exception(message, inner);
}
