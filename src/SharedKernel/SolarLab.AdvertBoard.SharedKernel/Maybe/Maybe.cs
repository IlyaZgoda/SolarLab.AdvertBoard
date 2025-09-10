namespace SolarLab.AdvertBoard.SharedKernel.Maybe
{
    public sealed record Maybe<T>(T? Value)
    {
        public bool HasValue => Value is not null;
        public bool HasNoValue => !HasValue;

        public T GetValue() => HasValue
            ? Value!
            : throw new InvalidOperationException("The value can not be accessed because it does not exist");

        public static Maybe<T> None => new(default);
        public static Maybe<T> From(T value) => new(value);

        public static implicit operator Maybe<T>(T value) => From(value);
        public static explicit operator T(Maybe<T> maybe) => maybe.GetValue();

        public override string ToString() =>
            HasValue ? $"Maybe({Value})" : "Maybe(None)";
    }
}
