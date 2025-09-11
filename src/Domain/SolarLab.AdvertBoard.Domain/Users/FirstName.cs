using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.Domain.Users
{
    public partial record FirstName
    {
        public const int MaxLength = 100;
        public string Value { get; init; }
        private static readonly Regex _regex = NameRegex();

        private FirstName(string value) =>
            Value = value;

        public static Result<FirstName> Create(string value) =>
            Result.Create(value, FirstNameErrors.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, FirstNameErrors.Empty)
                .Ensure(Validation.SmallerThan(MaxLength), FirstNameErrors.TooLong)
                .Ensure(Validation.IsMatchRegex(_regex), FirstNameErrors.NotValid)
                .Map(fn => new FirstName(fn));

        public static explicit operator string(FirstName firstName) => firstName.Value;

        [GeneratedRegex(@"^(?=.{1,100}$)[\p{L}'-]+(\s[\p{L}'-]+)*$")]
        public static partial Regex NameRegex();
    }
}
