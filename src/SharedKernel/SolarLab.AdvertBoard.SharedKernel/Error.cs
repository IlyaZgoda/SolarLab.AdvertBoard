namespace SolarLab.AdvertBoard.SharedKernel
{
    /// <summary>
    /// Record для представления ошибок в системе.
    /// </summary>
    /// <param name="Code">Код ошибки.</param>
    /// <param name="Description">Описание ошибки.</param>
    public record Error(string Code, string Description)
    {
        /// <summary>
        /// Представляет отсутствие ошибки.
        /// </summary>
        public static readonly Error None = new(string.Empty, string.Empty);

        /// <summary>
        /// Представляет ошибку null значения.
        /// </summary>
        public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");

        /// <summary>
        /// Неявное преобразование ошибки в строку (возвращает код ошибки).
        /// </summary>
        /// <param name="error">Ошибка для преобразования.</param>
        /// <returns>Код ошибки или пустая строка если ошибка null.</returns>
        public static implicit operator string(Error error) => error?.Code ?? string.Empty;
    }
}
