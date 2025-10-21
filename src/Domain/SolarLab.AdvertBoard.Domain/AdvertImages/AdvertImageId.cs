using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.AdvertImages
{
    /// <summary>
    /// Идентификатор изображения объявления как строго типизированный идентификатор.
    /// </summary>
    /// <param name="Id">Значение идентификатора.</param>
    public record AdvertImageId(Guid Id) : StronglyTypedId(Id);
}
