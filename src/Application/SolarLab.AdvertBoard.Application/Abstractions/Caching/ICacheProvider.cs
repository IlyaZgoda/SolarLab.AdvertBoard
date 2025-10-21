namespace SolarLab.AdvertBoard.Application.Abstractions.Caching
{
    /// <summary>
    /// Провайдер для работы с распределенным кэшем.
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Получает значение из кэша по ключу.
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого значения.</typeparam>
        /// <param name="key">Ключ кэша.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Кэшированное значение или null, если значение не найдено.</returns>
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Сохраняет значение в кэш с указанным ключом.
        /// </summary>
        /// <typeparam name="T">Тип сохраняемого значения.</typeparam>
        /// <param name="key">Ключ кэша.</param>
        /// <param name="value">Значение для кэширования.</param>
        /// <param name="expiration">Время жизни кэша. Если null, используется значение по умолчанию.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет значение из кэша по ключу.
        /// </summary>
        /// <param name="key">Ключ кэша.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    }
}
