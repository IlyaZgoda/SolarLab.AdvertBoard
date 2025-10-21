namespace SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions
{
    public static partial class ResultExtensions
    {
        /// <summary>
        /// Выполняет привязку (bind) асинхронной операции к результату.
        /// </summary>
        /// <typeparam name="TIn">Тип входного значения.</typeparam>
        /// <param name="result">Исходный результат.</param>
        /// <param name="func">Функция для выполнения над значением.</param>
        /// <returns>
        /// Результат выполнения функции если исходный результат успешен;
        /// иначе возвращает неудачу с оригинальной ошибкой.
        /// </returns>
        public static async Task<Result> Bind<TIn>(this Result<TIn> result, Func<TIn, Task<Result>> func) =>
            result.IsSuccess ? await func(result.Value) : Result.Failure(result.Error);

        /// <summary>
        /// Выполняет привязку асинхронной операции с преобразованием типа.
        /// </summary>
        /// <typeparam name="TIn">Тип входного значения.</typeparam>
        /// <typeparam name="TOut">Тип выходного значения (ссылочный тип).</typeparam>
        /// <param name="result">Исходный результат.</param>
        /// <param name="func">Функция для выполнения над значением.</param>
        /// <returns>
        /// Результат выполнения функции если исходный результат успешен;
        /// иначе возвращает неудачу с оригинальной ошибкой.
        /// </returns>
        public static async Task<Result<TOut>> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> func) 
            where TOut: class =>
            result.IsSuccess? await func(result.Value) : Result.Failure<TOut>(result.Error);

        /// <summary>
        /// Выполняет привязку асинхронной операции к результату без значения.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения.</typeparam>
        /// <param name="result">Исходный результат без значения.</param>
        /// <param name="func">Функция для выполнения.</param>
        /// <returns>
        /// Результат выполнения функции если исходный результат успешен;
        /// иначе возвращает неудачу с оригинальной ошибкой.
        /// </returns>
        public static async Task<Result<T>> Bind<T>(this Result result, Func<Task<Result<T>>> func)
        {
            if (result.IsFailure) return Result.Failure<T>(result.Error);
            return await func();
        }

        /// <summary>
        /// Выполняет привязку асинхронной операции к результату без значения.
        /// </summary>
        /// <param name="result">Исходный результат без значения.</param>
        /// <param name="func">Функция для выполнения.</param>
        /// <returns>
        /// Результат выполнения функции если исходный результат успешен;
        /// иначе возвращает оригинальную неудачу.
        /// </returns>
        public static async Task<Result> Bind(this Result result, Func<Task<Result>> func)
        {
            if (result.IsFailure) return result;
            return await func();
        }
    }
}
