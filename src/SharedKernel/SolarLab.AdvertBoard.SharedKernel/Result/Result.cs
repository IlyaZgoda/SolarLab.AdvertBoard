namespace SolarLab.AdvertBoard.SharedKernel.Result
{
    /// <summary>
    /// Представляет результат операции, который может быть успешным или содержать ошибку.
    /// </summary>
    public partial class Result
    {
        /// <summary>
        /// Получает значение, указывающее, является ли результат операции успешным.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Получает значение, указывающее, является ли результат операции неуспешным.
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Получает ошибку, связанную с результатом операции.
        /// </summary>
        public Error Error { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Result"/>.
        /// </summary>
        /// <param name="isSuccess">Признак успешности операции.</param>
        /// <param name="error">Объект ошибки.</param>
        /// <exception cref="ArgumentException">
        /// Вызывается когда:
        /// - операция успешна, но указана ошибка, отличная от <see cref="Error.None"/>
        /// - операция неуспешна, но указана ошибка <see cref="Error.None"/>
        /// </exception>
        protected internal Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }
    }
}
