using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.Categories
{
    public record CategoryTitle
    {
        public const int MaxLength = 50;
        public const int MinLength = 3;
        public string Value { get; init; }

        private CategoryTitle(string value) =>
            Value = value;

        public static Result<CategoryTitle> Create(string value) =>
            Result.Create(value, CategoryErrors.Title.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, CategoryErrors.Title.Empty)
                .Ensure(Validation.BiggerThan(MinLength), CategoryErrors.Title.TooShort)
                .Ensure(Validation.SmallerThan(MaxLength), CategoryErrors.Title.TooLong)
                .Map(ct => new CategoryTitle(ct));

        public static explicit operator string(CategoryTitle categoryTitle) => categoryTitle.Value;
    }
}
