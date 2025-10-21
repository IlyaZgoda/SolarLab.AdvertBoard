namespace SolarLab.AdvertBoard.SharedKernel.Result
{
    /// <summary>
    /// Представляет результат операции, содержащий возвращаемое значение типа <typeparamref name="TValue"/>.
    /// Наследует от <see cref="Result"/> и добавляет функциональность для работы со значением.
    /// </summary>
    /// <typeparam name="TValue">Тип возвращаемого значения.</typeparam>
    public partial class Result<TValue> : Result
    {
        private readonly TValue _value = default!;

        /// <summary>
        /// Получает значение результата операции.
        /// </summary>
        /// <value>
        /// Значение типа <typeparamref name="TValue"/> в случае успешного выполнения операции.
        /// </value>
        /// <exception cref="InvalidOperationException">
        /// Вызывается при попытке доступа к значению неуспешного результата.
        /// </exception>
        public TValue Value =>
            IsSuccess ?
            _value :
            throw new InvalidOperationException("The value of a failure result can not be accessed.");

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Result{TValue}"/>.
        /// </summary>
        /// <param name="value">Возвращаемое значение операции.</param>
        /// <param name="isSuccess">Признак успешности операции.</param>
        /// <param name="error">Объект ошибки.</param>
        protected internal Result(TValue value, bool isSuccess, Error error)
            : base(isSuccess, error) =>
            _value = value;

        /// <summary>
        /// Определяет неявное преобразование значения типа <typeparamref name="TValue"/> в <see cref="Result{TValue}"/>.
        /// </summary>
        /// <param name="value">Значение для преобразования.</param>
        /// <returns>
        /// Успешный <see cref="Result{TValue}"/> содержащий значение, если <paramref name="value"/> не <c>null</c>;
        /// в противном случае - неуспешный результат с ошибкой <see cref="Error.NullValue"/>.
        /// </returns>
        public static implicit operator Result<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    }
}
