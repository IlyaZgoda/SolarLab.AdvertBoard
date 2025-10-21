namespace SolarLab.AdvertBoard.Infrastructure.Emails
{
    /// <summary>
    /// Опции SMTP сервера.
    /// </summary>
    public class SmtpOptions 
    {
        /// <summary>
        /// Хост.
        /// </summary>
        public string Host { get; set; } = null!;

        /// <summary>
        /// Порт.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Флаг для включения SSL.
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Username { get; set; } = null!;

        /// <summary>
        /// Отображаемое имя в письме.
        /// </summary>
        public string DisplayName { get; set; } = null!;

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// Почта отправителя.
        /// </summary>
        public string From { get; set; } = null!;

    }
}
