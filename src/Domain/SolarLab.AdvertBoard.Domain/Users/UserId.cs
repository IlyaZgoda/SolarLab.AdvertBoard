using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Users
{
    /// <summary>
    /// Идентификатор пользователя как строго типизированный идентификатор.
    /// </summary>
    /// <param name="Id">Значение идентификатора.</param>
    public record UserId(Guid Id) : StronglyTypedId(Id);
}
