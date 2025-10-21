using SolarLab.AdvertBoard.Domain.Users.Events;
using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Users
{
    /// <summary>
    /// Представляет пользователя в системе.
    /// </summary>
    public class User : AggregateRoot
    {
        /// <summary>
        /// Идентификатор пользователя в доменной модели.
        /// </summary>
        public UserId Id { get; init; } = null!;

        /// <summary>
        /// Идентификатор пользователя в системе аутентификации (Identity).
        /// </summary>
        public string IdentityId { get; init; } = null!;

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public FirstName FirstName { get; private set; } = null!;

        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public LastName LastName { get; private set; } = null!;

        /// <summary>
        /// Отчество пользователя (опционально).
        /// </summary>
        public MiddleName? MiddleName { get; private set; }

        /// <summary>
        /// Контактный email пользователя.
        /// </summary>
        public ContactEmail ContactEmail { get; private set; } = null!;

        /// <summary>
        /// Номер телефона пользователя (опционально).
        /// </summary>
        public PhoneNumber? PhoneNumber { get; private set; }

        /// <summary>
        /// Дата и время регистрации пользователя.
        /// </summary>
        public DateTime CreatedAt { get; init; }

        /// <summary>
        /// Полное имя пользователя в формате "Фамилия Имя Отчество".
        /// </summary>
        public string FullName => $"{LastName.Value} {FirstName.Value} {MiddleName?.Value}";

        /// <summary>
        /// Приватный конструктор для EF Core.
        /// </summary>
        private User() { }

        /// <summary>
        /// Приватный конструктор для создания пользователя.
        /// </summary>
        /// <param name="identityId">Идентификатор из системы аутентификации.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="middleName">Отчество.</param>
        /// <param name="contactEmail">Контактный email.</param>
        /// <param name="phoneNumber">Номер телефона.</param>
        private User(
            string identityId,
            FirstName firstName,
            LastName lastName,
            MiddleName? middleName,
            ContactEmail contactEmail,
            PhoneNumber? phoneNumber)
        {
            Id = new UserId(Guid.NewGuid());
            IdentityId = identityId;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            ContactEmail = contactEmail;
            PhoneNumber = phoneNumber;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Создает нового пользователя.
        /// </summary>
        /// <param name="identityId">Идентификатор из системы аутентификации.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="middleName">Отчество.</param>
        /// <param name="contactEmail">Контактный email.</param>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <returns>Новый экземпляр пользователя.</returns>
        /// <remarks>
        /// Генерирует доменное событие <see cref="UserRegisteredDomainEvent"/> после создания пользователя.
        /// </remarks>
        public static User Create(
            string identityId,
            FirstName firstName,
            LastName lastName,
            MiddleName? middleName,
            ContactEmail contactEmail,
            PhoneNumber? phoneNumber)
        {
            var user = new User(identityId, firstName, lastName, middleName, contactEmail, phoneNumber);

            user.Raise(new UserRegisteredDomainEvent(user.Id, user.IdentityId));

            return user;
        }
    }
}
