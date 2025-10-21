namespace SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions
{
    public static partial class ResultExtensions
    {
        /// <summary>
        /// Выполняет соответствующее действие в зависимости от результата.
        /// </summary>
        /// <typeparam name="T">Тип значения.</typeparam>
        /// <param name="result">Результат для обработки.</param>
        /// <param name="onSuccess">Действие при успехе.</param>
        /// <param name="onFailure">Действие при неудаче.</param>
        public static void Match<T>(this Result<T> result, Action<T> onSuccess, Action<Error> onFailure)
        {
            if (result.IsSuccess && onSuccess != null)
            {
                onSuccess(result.Value);
            }
            else if (result.IsFailure && onFailure != null)
            {
                onFailure(result.Error);
            }
        }

        /// <summary>
        /// Асинхронно выполняет соответствующую функцию в зависимости от результата.
        /// </summary>
        /// <typeparam name="TIn">Тип входного значения.</typeparam>
        /// <typeparam name="TOut">Тип возвращаемого значения.</typeparam>
        /// <param name="resultTask">Асинхронная задача с результатом.</param>
        /// <param name="onSuccess">Функция при успехе.</param>
        /// <param name="onFailure">Функция при неудаче.</param>
        /// <returns>
        /// Результат выполнения соответствующей функции.
        /// </returns>
        public static async Task<TOut> Match<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, TOut> onSuccess, Func<Error, TOut> onFailure)
        {
            var result = await resultTask.ConfigureAwait(false);

            return result.IsSuccess
                ? onSuccess(result.Value)
                : onFailure(result.Error);
        }

        /// <summary>
        /// Асинхронно выполняет соответствующую функцию для результата без значения.
        /// </summary>
        /// <typeparam name="TOut">Тип возвращаемого значения.</typeparam>
        /// <param name="resultTask">Асинхронная задача с результатом.</param>
        /// <param name="onSuccess">Функция при успехе.</param>
        /// <param name="onFailure">Функция при неудаче.</param>
        /// <returns>
        /// Результат выполнения соответствующей функции.
        /// </returns>
        public static async Task<TOut> Match<TOut>(this Task<Result> resultTask, Func<TOut> onSuccess, Func<Error, TOut> onFailure)
        {
            var result = await resultTask.ConfigureAwait(false);

            return result.IsSuccess
                ? onSuccess()
                : onFailure(result.Error);
        }
    }
}
