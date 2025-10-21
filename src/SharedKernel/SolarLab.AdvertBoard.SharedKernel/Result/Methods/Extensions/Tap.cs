namespace SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions
{
    public static partial class ResultExtensions
    {
        /// <summary>
        /// Выполняет побочное действие без изменения результата (tap-операция).
        /// </summary>
        /// <typeparam name="T">Тип значения.</typeparam>
        /// <param name="result">Исходный результат.</param>
        /// <param name="onSuccess">Действие при успехе.</param>
        /// <param name="onFailure">Действие при неудаче.</param>
        /// <returns>Исходный результат без изменений.</returns>
        public static Result<T> Tap<T>(this Result<T> result, Action<T> onSuccess, Action<Error> onFailure)
        {
            result.Match(onSuccess, onFailure);
            return result;
        }

        /// <summary>
        /// Асинхронно выполняет побочное действие без изменения результата.
        /// </summary>
        /// <typeparam name="T">Тип значения.</typeparam>
        /// <param name="resultTask">Асинхронная задача с результатом.</param>
        /// <param name="onSuccess">Действие при успехе.</param>
        /// <param name="onFailure">Действие при неудаче.</param>
        /// <returns>Исходный результат без изменений.</returns>
        public static async Task<Result<T>> Tap<T>(this Task<Result<T>> resultTask, Action<T> onSuccess, Action<Error> onFailure)
        {
            var result = await resultTask;
            result.Match(onSuccess, onFailure);
            return result;
        }
    }
}
