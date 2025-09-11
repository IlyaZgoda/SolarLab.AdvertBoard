using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.Domain.Users
{
    public partial record Email
    {
        public const int MaxLength = 320; 
        public string Value { get; init; }

        private static readonly Regex _regex = EmailRegex();

        private Email(string value) => Value = value;

        public static Result<Email> Create(string value) =>
            Result.Create(value, UserErrors.Email.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, UserErrors.Email.Empty)
                .Ensure(Validation.SmallerThan(MaxLength), UserErrors.Email.TooLong)
                .Ensure(Validation.IsMatchRegex(_regex), UserErrors.Email.NotValid)
                .Map(e => new Email(e));

        public static explicit operator string(Email email) => email.Value;

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        private static partial Regex EmailRegex();
    }
}
