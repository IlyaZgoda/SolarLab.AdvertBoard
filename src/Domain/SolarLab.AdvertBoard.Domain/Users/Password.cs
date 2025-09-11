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
            Result.Create(value, UserErrors.Password.Empty)
            .Ensure(Validation.IsNotNullOrEmpty, UserErrors.Password.Empty)
            .Ensure(Validation.SmallerThan(MaxLength), UserErrors.Password.TooLong)
            .Ensure(Validation.BiggerThan(MinLength), UserErrors.Password.TooShort)
            .Ensure(Validation.HasLowercaseLetters, UserErrors.Password.MissingLowercaseLetters)
            .Ensure(Validation.HasUppercaseLetters, UserErrors.Password.MissingUppercaseLetters)
            .Ensure(Validation.HasDigits, UserErrors.Password.MissingDigits)
            .Ensure(Validation.HasSpecialCharacters, UserErrors.Password.MissingSpecialCharacters)
            .Ensure(Validation.IsMatchRegex(_regex), UserErrors.Password.NotValid)
            .Map(p => new Password(value));

        [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,128}$")]
        private static partial Regex PasswordRegex();
    }
}
