namespace SolarLab.AdvertBoard.SharedKernel.Result
{
    public partial class Result
    {
        /// <summary>
        /// Создает успешный результат без возвращаемого значения.
        /// </summary>
        /// <returns>Успешный результат без значения.</returns>
        public static Result Success() =>
            new(true, Error.None);

        /// <summary>
        /// Создает успешный результат с возвращаемым значением.
        /// </summary>
        /// <typeparam name="TValue">Тип возвращаемого значения.</typeparam>
        /// <param name="value">Значение для возврата.</param>
        /// <returns>Успешный результат с указанным значением.</returns>
        public static Result<TValue> Success<TValue>(TValue value) =>
            new(value, true, Error.None);

    }
}
