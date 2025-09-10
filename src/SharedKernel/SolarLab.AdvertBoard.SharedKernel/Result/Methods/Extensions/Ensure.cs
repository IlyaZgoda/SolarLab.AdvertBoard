﻿namespace SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions
{
    public static partial class ResultExtensions
    {
        public static Result<TValue> Ensure<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Error error)
        {
            if (result.IsFailure)
                return result;

            return result.IsSuccess && predicate(result.Value!) ? result : Result.Failure<TValue>(error);
        }
    }
}
