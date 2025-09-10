namespace SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions
{
    public static partial class ResultExtensions
    {
        public static Result<T> Tap<T>(this Result<T> result, Action<T> onSuccess, Action<Error> onFailure)
        {
            result.Match(onSuccess, onFailure);
            return result;
        }

        public static async Task<Result<T>> Tap<T>(this Task<Result<T>> resultTask, Action<T> onSuccess, Action<Error> onFailure)
        {
            var result = await resultTask;
            result.Match(onSuccess, onFailure);
            return result;
        }
    }
}
