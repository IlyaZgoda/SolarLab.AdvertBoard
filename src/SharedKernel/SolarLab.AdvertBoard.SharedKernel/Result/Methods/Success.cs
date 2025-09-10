namespace SolarLab.AdvertBoard.SharedKernel.Result
{
    public partial class Result
    {
        public static Result Success() =>
            new(true, Error.None);

        public static Result<TValue> Success<TValue>(TValue value) =>
            new(value, true, Error.None);

    }
}
