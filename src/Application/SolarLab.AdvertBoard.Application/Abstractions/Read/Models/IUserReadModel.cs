namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Models
{
    /// <summary>
    /// Read-модель для пользователя.
    /// </summary>
    /// <remarks>
    /// Используется для представления данных в read-слое (CQRS).
    /// Содержит профиль пользователя и связанные сущности.
    /// </remarks>
    public interface IUserReadModel
    {
        /// <summary>
        /// Идентификатор пользователя в доменной модели.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Идентификатор пользователя в системе аутентификации.
        /// </summary>
        public string IdentityId { get; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// Отчество пользователя.
        /// </summary>
        public string? MiddleName { get; }

        /// <summary>
        /// Контактный email пользователя.
        /// </summary>
        public string ContactEmail { get; }

        /// <summary>
        /// Номер телефона пользователя.
        /// </summary>
        public string? PhoneNumber { get; }

        /// <summary>
        /// Дата и время регистрации пользователя.
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Полное имя пользователя в формате "Фамилия Имя Отчество".
        /// </summary>
        public string FullName => $"{LastName} {FirstName} {MiddleName}";

        /// <summary>
        /// Коллекция объявлений пользователя.
        /// </summary>
        public IReadOnlyList<IAdvertReadModel> Adverts { get; }

        /// <summary>
        /// Коллекция комментариев пользователя.
        /// </summary>
        public IReadOnlyList<ICommentReadModel> Comments { get; }
    }
}
