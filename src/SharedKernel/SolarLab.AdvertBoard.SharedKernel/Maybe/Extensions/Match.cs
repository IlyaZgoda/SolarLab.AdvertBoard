namespace SolarLab.AdvertBoard.SharedKernel.Maybe.Extensions
{
    public static partial class MaybeExtensions
    {
        /// <summary>
        /// Асинхронно выполняет соответствующую функцию в зависимости от наличия значения.
        /// </summary>
        /// <typeparam name="TIn">Тип входного значения.</typeparam>
        /// <typeparam name="TOut">Тип возвращаемого значения.</typeparam>
        /// <param name="resultTask">Асинхронная задача с контейнером Maybe.</param>
        /// <param name="onSuccess">Функция для выполнения при наличии значения.</param>
        /// <param name="onFailure">Функция для выполнения при отсутствии значения.</param>
        /// <returns>
        /// Результат выполнения <paramref name="onSuccess"/> если значение присутствует;
        /// иначе результат выполнения <paramref name="onFailure"/>.
        /// </returns>
        public static async Task<TOut> Match<TIn, TOut>(
            this Task<Maybe<TIn>> resultTask,
            Func<TIn, TOut> onSuccess,
            Func<TOut> onFailure)
        {
            Maybe<TIn> maybe = await resultTask;

            return maybe.HasValue ? onSuccess(maybe.Value) : onFailure();
        }

    }
}
