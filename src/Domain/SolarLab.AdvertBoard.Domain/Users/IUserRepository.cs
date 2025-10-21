using SolarLab.AdvertBoard.SharedKernel.Maybe;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Domain.Users
{
    /// <summary>
    /// Репозиторий для работы с пользователями.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Добавляет нового пользователя.
        /// </summary>
        /// <param name="user">Пользователь для добавления.</param>
        void Add(User user);

        /// <summary>
        /// Обновляет существующего пользователя.
        /// </summary>
        /// <param name="user">Пользователь для обновления.</param>
        void Update(User user);

        /// <summary>
        /// Получает пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Пользователь если найден, иначе <see cref="Maybe{T}.None"/>.</returns>
        Task<Maybe<User>> GetByIdAsync(UserId id);

        /// <summary>
        /// Получает пользователя по спецификации.
        /// </summary>
        /// <param name="specification">Спецификация для фильтрации.</param>
        /// <returns>Пользователь если найден, иначе <see cref="Maybe{T}.None"/>.</returns>
        Task<Maybe<User>> GetBySpecificationAsync(Specification<User> specification);

        /// <summary>
        /// Проверяет, является ли пользователь владельцем ресурса.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя в доменной модели.</param>
        /// <param name="identityId">Идентификатор пользователя в системе аутентификации.</param>
        /// <returns>true если пользователь является владельцем, иначе false.</returns>
        Task<bool> IsOwner(UserId userId, string identityId);
    }
}
