using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.AdvertImages
{
    public record ImageContent : IValueObject
    {
        public byte[] Value { get; init; }

        private ImageContent(byte[] value) =>
            Value = value;

        private static bool IsValidImageSignature(byte[] data)
        {
            // JPEG
            if (data.Length > 3 && data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF)
                return true;

            // PNG
            if (data.Length > 7 && data[0] == 0x89 && data[1] == 0x50 &&
                data[2] == 0x4E && data[3] == 0x47 && data[4] == 0x0D &&
                data[5] == 0x0A && data[6] == 0x1A && data[7] == 0x0A)
                return true;

            return false;
        }

        public static Result<ImageContent> Create(byte[] data) =>
            Result.Create(data, AdvertImageErrors.Content.Empty)
                .Ensure(v => v is not null && v.Length > 0, AdvertImageErrors.Content.Empty)
                .Ensure(v => IsValidImageSignature(data), AdvertImageErrors.Content.Invalid)
                .Map(v => new ImageContent(v));

        public static explicit operator byte[](ImageContent imageContent) => imageContent.Value;
    }
}
