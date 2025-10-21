using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Adverts
{
    /// <summary>
    /// Идентификатор объявления как строго типизированный идентификатор.
    /// </summary>
    /// <param name="Id">Значение идентификатора.</param>
    public record AdvertId(Guid Id) : StronglyTypedId(Id);
}
