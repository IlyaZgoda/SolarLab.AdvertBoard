namespace SolarLab.AdvertBoard.Domain.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при нарушении бизнес-правил доменной модели.
    /// </summary>
    /// <param name="message">Сообщение об ошибке, описывающее нарушенное бизнес-правило.</param>
    /// <param name="inner">Внутреннее исключение, если применимо.</param>
    public class DomainException(string message, Exception? inner = default) : Exception(message, inner);
}
