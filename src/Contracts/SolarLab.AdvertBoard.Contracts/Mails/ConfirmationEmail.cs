namespace SolarLab.AdvertBoard.Contracts.Mails
{
    /// <summary>
    /// DTO для отправки почты с ссылкой на подтверждение.
    /// </summary>
    /// <param name="To">Кому.</param>
    /// <param name="Uri">Ссылка на подтверждение.</param>
    public record ConfirmationEmail(string To, string Uri);
}
