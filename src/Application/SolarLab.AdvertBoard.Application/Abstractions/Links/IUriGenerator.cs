using SolarLab.AdvertBoard.Contracts.Links;

namespace SolarLab.AdvertBoard.Application.Abstractions.Links
{
    /// <summary>
    /// Генератор URI.
    /// </summary>
    public interface IUriGenerator
    {
        /// <summary>
        /// Генерирует URI для подтверждения email.
        /// </summary>
        /// <param name="confirmationUriRequest">Запрос с данными для генерации URI подтверждения.</param>
        /// <returns>URI для подтверждения email.</returns>
        string GenerateEmailConfirmationUri(ConfirmationUriRequest confirmationUriRequest);
    }
}
