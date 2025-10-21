namespace SolarLab.AdvertBoard.SharedKernel.Maybe.Extensions
{
    public static partial class MaybeExtensions
    {
        /// <summary>
        /// Выполняет привязку асинхронной операции к контейнеру Maybe.
        /// </summary>
        /// <typeparam name="TIn">Тип входного значения.</typeparam>
        /// <typeparam name="TOut">Тип выходного значения.</typeparam>
        /// <param name="maybe">Исходный контейнер.</param>
        /// <param name="func">Асинхронная функция для выполнения над значением.</param>
        /// <returns>
        /// Результат выполнения функции если контейнер содержит значение;
        /// иначе пустой контейнер <see cref="Maybe{TOut}.None"/>.
        /// </returns>
        public static async Task<Maybe<TOut>> Bind<TIn, TOut>(this Maybe<TIn> maybe, Func<TIn, Task<Maybe<TOut>>> func) =>
           maybe.HasValue ? await func(maybe.Value) : Maybe<TOut>.None;

    }
}
