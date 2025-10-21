namespace SolarLab.AdvertBoard.SharedKernel.Maybe
{
    /// <summary>
    /// Представляет контейнер для значения, которое может отсутствовать (optional value).
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    public sealed record Maybe<T>
    {
        /// <summary>
        /// Внутреннее хранимое значение.
        /// </summary>
        public T? _value;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="value">Значение, которое может быть null.</param>
        public Maybe(T? value)
        {
            _value = value;
        }

        /// <summary>
        /// Получает значение, указывающее, содержит ли контейнер значение.
        /// </summary>
        /// <value>
        /// <c>true</c> если контейнер содержит значение; иначе <c>false</c>.
        /// </value>
        public bool HasValue => _value is not null;

        /// <summary>
        /// Получает значение, указывающее, что контейнер не содержит значения.
        /// </summary>
        /// <value>
        /// <c>true</c> если контейнер не содержит значения; иначе <c>false</c>.
        /// </value>
        public bool HasNoValue => !HasValue;

        /// <summary>
        /// Получает значение контейнера.
        /// </summary>
        /// <value>
        /// Значение типа <typeparamref name="T"/> если оно присутствует.
        /// </value>
        /// <exception cref="InvalidOperationException">
        /// Вызывается при попытке доступа к значению пустого контейнера.
        /// </exception>
        public T Value => HasValue
            ? _value!
            : throw new InvalidOperationException("The Value can not be accessed because it does not exist");


        /// <summary>
        /// Получает экземпляр <see cref="Maybe{T}"/> без значения.
        /// </summary>
        /// <value>Пустой контейнер.</value>
        public static Maybe<T> None => new(default);

        /// <summary>
        /// Создает экземпляр <see cref="Maybe{T}"/> из значения.
        /// </summary>
        /// <param name="value">Значение, которое может быть null.</param>
        /// <returns>
        /// Контейнер со значением если <paramref name="value"/> не null;
        /// иначе пустой контейнер.
        /// </returns>
        public static Maybe<T> From(T? value) => new(value);

        /// <summary>
        /// Неявное преобразование значения в <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="value">Значение для преобразования.</param>
        /// <returns>
        /// Контейнер со значением если <paramref name="value"/> не null;
        /// иначе пустой контейнер.
        /// </returns>
        public static implicit operator Maybe<T>(T? value) => From(value);

        /// <summary>
        /// Явное преобразование <see cref="Maybe{T}"/> в значение.
        /// </summary>
        /// <param name="maybe">Контейнер для преобразования.</param>
        /// <returns>Значение контейнера.</returns>
        /// <exception cref="InvalidOperationException">
        /// Вызывается при попытке преобразования пустого контейнера.
        /// </exception>
        public static explicit operator T(Maybe<T> maybe) => maybe.Value;
    }
}
