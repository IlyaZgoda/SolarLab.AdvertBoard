namespace SolarLab.AdvertBoard.SharedKernel.Maybe.Extensions
{
    public static partial class MaybeExtensions
    {
        public static async Task<Maybe<TOut>> Bind<TIn, TOut>(this Maybe<TIn> maybe, Func<TIn, Task<Maybe<TOut>>> func) =>
           maybe.HasValue ? await func(maybe.Value) : Maybe<TOut>.None;

    }
}
