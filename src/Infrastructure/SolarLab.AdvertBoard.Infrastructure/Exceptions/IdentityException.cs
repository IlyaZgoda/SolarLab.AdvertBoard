namespace SolarLab.AdvertBoard.Infrastructure.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при возникновении ошибки в Identity.
    /// </summary>
    /// <param name="message">Сообщение об ошибке из Identity.</param>
    public class IdentityException(string message) : Exception(message) {}
}
