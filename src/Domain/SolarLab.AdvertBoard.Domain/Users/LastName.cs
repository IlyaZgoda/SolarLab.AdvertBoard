using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.Domain.Users
{
    public record LastName
    {
        public const int MaxLength = 100;
        public string Value { get; init; }
        private static readonly Regex _regex = FirstName.NameRegex();

        private LastName(string value) =>
            Value = value;

        public static Result<LastName> Create(string value) =>
            Result.Create(value, LastNameErrors.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, LastNameErrors.Empty)
                .Ensure(Validation.SmallerThan(MaxLength), LastNameErrors.TooLong)
                .Ensure(Validation.IsMatchRegex(_regex), LastNameErrors.NotValid)    
                .Map(ln => new LastName(ln));

        public static explicit operator string(LastName lastName) => lastName.Value;
    }
}
