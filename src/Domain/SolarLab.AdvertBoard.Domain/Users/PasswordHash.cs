using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.Users
{
    public record PasswordHash
    {
        public string Value { get; init; }

        private PasswordHash(string value) => Value = value;

        public static Result<PasswordHash> Create(string value) =>
            Result.Create(value, UserErrors.PasswordHash.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, UserErrors.PasswordHash.Empty)
                .Map(ph => new PasswordHash(ph));

        public static explicit operator string(PasswordHash hash) => hash.Value;

    }
}
