namespace SolarLab.AdvertBoard.SharedKernel.Result
{
    public partial class Result
    {
        /// <summary>
        /// Создает неуспешный результат без возвращаемого значения.
        /// </summary>
        /// <param name="error">Ошибка, описывающая причину неудачи.</param>
        /// <returns>Неуспешный результат с указанной ошибкой.</returns>
        public static Result Failure(Error error) =>
            new(false, error);

        /// <summary>
        /// Создает неуспешный результат с возвращаемым значением.
        /// </summary>
        /// <typeparam name="TValue">Тип возвращаемого значения.</typeparam>
        /// <param name="error">Ошибка, описывающая причину неудачи.</param>
        /// <returns>Неуспешный результат с указанной ошибкой.</returns>
        public static Result<TValue> Failure<TValue>(Error error) =>
            new(default!, false, error);
    }
}
