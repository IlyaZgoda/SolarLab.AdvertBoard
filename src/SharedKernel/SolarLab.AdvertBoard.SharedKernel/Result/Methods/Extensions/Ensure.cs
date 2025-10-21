namespace SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions
{
    public static partial class ResultExtensions
    {
        /// <summary>
        /// Проверяет условие для значения в успешном результате.
        /// </summary>
        /// <typeparam name="TValue">Тип значения.</typeparam>
        /// <param name="result">Исходный результат.</param>
        /// <param name="predicate">Условие для проверки.</param>
        /// <param name="error">Ошибка в случае невыполнения условия.</param>
        /// <returns>
        /// Исходный результат если условие выполнено;
        /// неудачу с указанной ошибкой если условие не выполнено;
        /// оригинальную неудачу если исходный результат неуспешен.
        /// </returns>
        public static Result<TValue> Ensure<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Error error)
        {
            if (result.IsFailure)
                return result;

            return result.IsSuccess && predicate(result.Value!) ? result : Result.Failure<TValue>(error);
        }
    }
}
