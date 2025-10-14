using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.Domain.Users
{
    public record MiddleName
    {
        public const int MaxLength = 100;
        public string? Value { get; init; }
        private static readonly Regex _regex = FirstName.NameRegex();

        private MiddleName(string? value) =>
            Value = value;

        public static Result<MiddleName?> Create(string? value) =>
            string.IsNullOrWhiteSpace(value) 
            ? Result.Success<MiddleName?>(null) 
            : Result.Create(value, UserErrors.MiddleName.Empty)
                .Ensure(Validation.SmallerThan(MaxLength), UserErrors.MiddleName.TooLong)
                .Ensure(Validation.IsMatchRegex(_regex), UserErrors.MiddleName.NotValid)
                .MapNullable(mn => new MiddleName(mn));

        public static explicit operator string(MiddleName middleName) => middleName?.Value ?? string.Empty;
    }
}
