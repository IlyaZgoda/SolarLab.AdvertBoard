namespace SolarLab.AdvertBoard.SharedKernel.Maybe.Extensions
{
    public static partial class MaybeExtensions
    {
        public static async Task<TOut> Match<TIn, TOut>(
            this Task<Maybe<TIn>> resultTask,
            Func<TIn, TOut> onSuccess,
            Func<TOut> onFailure)
        {
            Maybe<TIn> maybe = await resultTask;

            return maybe.HasValue ? onSuccess(maybe.Value) : onFailure();
        }

    }
}
