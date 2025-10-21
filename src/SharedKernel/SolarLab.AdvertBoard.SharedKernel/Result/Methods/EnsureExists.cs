namespace SolarLab.AdvertBoard.SharedKernel.Result
{
    public partial class Result
    {
        /// <summary>
        /// Обеспечивает существование сущности или создает новую при отсутствии.
        /// </summary>
        /// <typeparam name="T">Тип сущности.</typeparam>
        /// <param name="getFunc">Функция для получения существующей сущности.</param>
        /// <param name="createFunc">Функция для создания новой сущности.</param>
        /// <returns>
        /// Результат с существующей сущностью, если она найдена; 
        /// иначе результат создания новой сущности.
        /// </returns>
        public static async Task<Result<T>> EnsureExistOrCreate<T>(Func<Task<Result<T>>> getFunc, Func<Task<Result<T>>> createFunc)
        {
            var result = await getFunc();

            if (result.IsSuccess)
                return result;

            return await createFunc();
        }
    }
}
