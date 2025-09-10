﻿namespace SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions
{
    public static partial class ResultExtensions
    {
        public static void Match<T>(this Result<T> result, Action<T> onSuccess, Action<Error> onFailure)
        {
            if (result.IsSuccess && onSuccess != null)
            {
                onSuccess(result.Value);
            }
            else if (result.IsFailure && onFailure != null)
            {
                onFailure(result.Error);
            }
        }

        public static async Task<TOut> Match<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, TOut> onSuccess, Func<Error, TOut> onFailure)
        {
            var result = await resultTask.ConfigureAwait(false);

            return result.IsSuccess
                ? onSuccess(result.Value)
                : onFailure(result.Error);
        }

        public static async Task<TOut> Match<TOut>(this Task<Result> resultTask, Func<TOut> onSuccess, Func<Error, TOut> onFailure)
        {
            var result = await resultTask.ConfigureAwait(false);

            return result.IsSuccess
                ? onSuccess()
                : onFailure(result.Error);
        }
    }
}
