namespace SolarLab.AdvertBoard.SharedKernel.Result
{
    public partial class Result
    {
        /// <summary>
        /// Создает результат операции для ссылочных типов с проверкой на null.
        /// </summary>
        /// <typeparam name="TValue">Тип возвращаемого значения (ссылочный тип).</typeparam>
        /// <param name="value">Значение для возврата. Может быть null.</param>
        /// <param name="error">Ошибка, которая будет использована если <paramref name="value"/> равен null.</param>
        /// <returns>
        /// Успешный <see cref="Result{TValue}"/> содержащий значение, если <paramref name="value"/> не null;
        /// в противном случае - неуспешный результат с указанной ошибкой.
        /// </returns>
        public static Result<TValue> Create<TValue>(TValue? value, Error error)
            where TValue : class =>
            value is null ? Failure<TValue>(error) : Success(value);

        /// <summary>
        /// Создает успешный результат операции для значимых типов.
        /// </summary>
        /// <typeparam name="TValue">Тип возвращаемого значения (значимый тип).</typeparam>
        /// <param name="value">Значение для возврата.</param>
        /// <param name="error">Параметр ошибки (игнорируется для значимых типов).</param>
        /// <returns>
        /// Всегда возвращает успешный <see cref="Result{TValue}"/> содержащий значение.
        /// </returns>
        public static Result<TValue> CreateStruct<TValue>(TValue value, Error error)
            where TValue : struct => Success(value);
    }
}
