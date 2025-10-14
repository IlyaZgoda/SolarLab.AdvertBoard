using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.Domain.Users
{
    public partial record PhoneNumber
    {
        public const int MaxLength = 15; 
        public string? Value { get; init; }

        private static readonly Regex _regex = PhoneNumberRegex();

        private PhoneNumber(string value) => Value = value;

        public static Result<PhoneNumber?> Create(string? value) =>
            string.IsNullOrWhiteSpace(value)
            ? Result.Success<PhoneNumber?>(null)
            : Result.Create(value, UserErrors.PhoneNumber.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, UserErrors.PhoneNumber.Empty)
                .Ensure(Validation.SmallerThan(MaxLength), UserErrors.PhoneNumber.TooLong)
                .Ensure(Validation.IsMatchRegex(_regex), UserErrors.PhoneNumber.NotValid)
                .MapNullable(pn => new PhoneNumber(pn));

        public static explicit operator string(PhoneNumber phoneNumber) => phoneNumber?.Value ?? string.Empty;

        [GeneratedRegex(@"^\+\d{11,14}$")]
        public static partial Regex PhoneNumberRegex();
    }
}
