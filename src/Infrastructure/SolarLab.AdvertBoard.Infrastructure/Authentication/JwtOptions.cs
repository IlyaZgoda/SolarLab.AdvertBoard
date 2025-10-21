namespace SolarLab.AdvertBoard.Infrastructure.Authentication
{
    /// <summary>
    /// Опции JWT.
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Секрет.
        /// </summary>
        public string Secret { get; set; } = null!;

        /// <summary>
        /// Издатель.
        /// </summary>
        public string Issuer { get; set; } = null!;

        /// <summary>
        /// Аудитория.
        /// </summary>
        public string Audience { get; set; } = null!;

        /// <summary>
        /// Время истечения.
        /// </summary>
        public double ExpirationInMinutes { get; set; }
    }
}
