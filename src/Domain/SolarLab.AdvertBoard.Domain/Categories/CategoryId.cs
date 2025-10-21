using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Categories
{
    /// <summary>
    /// Идентификатор категории как строго типизированный идентификатор.
    /// </summary>
    /// <param name="Id">Значение идентификатора.</param>
    public record CategoryId(Guid Id) : StronglyTypedId(Id), IValueObject;
}
