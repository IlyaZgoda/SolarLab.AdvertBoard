using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.Domain.Users
{
    public partial record Password
    {
        public string Value { get; init; }
        public const int MaxLength = 128;
        public const int MinLength = 6;
        private static readonly Regex _regex = PasswordRegex();

        private Password(string value) => Value = value;

        public static Result<Password> Create(string value) =>
            Result.Create(value, PasswordErrors.Empty)
            .Ensure(Validation.IsNotNullOrEmpty, PasswordErrors.Empty)
            .Ensure(Validation.SmallerThan(MaxLength), PasswordErrors.TooLong)
            .Ensure(Validation.BiggerThan(MinLength), PasswordErrors.TooShort)
            .Ensure(Validation.HasLowercaseLetters, PasswordErrors.MissingLowercaseLetters)
            .Ensure(Validation.HasUppercaseLetters, PasswordErrors.MissingUppercaseLetters)
            .Ensure(Validation.HasDigits, PasswordErrors.MissingDigits)
            .Ensure(Validation.HasSpecialCharacters, PasswordErrors.MissingSpecialCharacters)
            .Ensure(Validation.IsMatchRegex(_regex), PasswordErrors.NotValid)
            .Map(p => new Password(value));

        [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,128}$")]
        private static partial Regex PasswordRegex();
    }
}
