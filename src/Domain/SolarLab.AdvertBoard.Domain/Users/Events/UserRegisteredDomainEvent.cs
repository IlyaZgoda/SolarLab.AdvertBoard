using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Users.Events
{
    /// <summary>
    /// Доменное событие регистрации нового пользователя.
    /// </summary>
    /// <param name="UserId">Идентификатор пользователя в доменной модели.</param>
    /// <param name="IdentityId">Идентификатор пользователя в системе аутентификации.</param>
    public record UserRegisteredDomainEvent(UserId UserId, string IdentityId) : IDomainEvent;
}
