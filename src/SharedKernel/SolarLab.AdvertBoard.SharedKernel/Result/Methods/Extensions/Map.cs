namespace SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions
{
    public static partial class ResultExtensions
    {
        /// <summary>
        /// Преобразует значение успешного результата в другой тип.
        /// </summary>
        /// <typeparam name="TIn">Тип входного значения.</typeparam>
        /// <typeparam name="TOut">Тип выходного значения (ссылочный тип).</typeparam>
        /// <param name="result">Исходный результат.</param>
        /// <param name="predicate">Функция преобразования.</param>
        /// <returns>
        /// Успешный результат с преобразованным значением если исходный успешен;
        /// иначе неудачу с оригинальной ошибкой.
        /// </returns>
        public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> predicate)
            where TOut : class =>
            result.IsSuccess ? predicate(result.Value) : Result.Failure<TOut>(result.Error);

        /// <summary>
        /// Преобразует значение успешного результата в nullable тип.
        /// </summary>
        /// <typeparam name="TIn">Тип входного значения.</typeparam>
        /// <typeparam name="TOut">Тип выходного значения (ссылочный тип).</typeparam>
        /// <param name="result">Исходный результат.</param>
        /// <param name="map">Функция преобразования.</param>
        /// <returns>
        /// Успешный результат с преобразованным nullable значением если исходный успешен;
        /// иначе неудачу с оригинальной ошибкой.
        /// </returns>
        public static Result<TOut?> MapNullable<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> map)
            where TOut : class =>
            result.IsSuccess ? Result.Success<TOut?>(map(result.Value)) : Result.Failure<TOut?>(result.Error);
    }
}
