namespace SolarLab.AdvertBoard.Application.Abstractions
{
    /// <summary>
    /// Интерфейс Unit of Work для управления транзакциями и сохранения изменений в базе данных.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Асинхронно сохраняет все изменения, сделанные в контексте базы данных.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Количество затронутых записей в базе данных.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
