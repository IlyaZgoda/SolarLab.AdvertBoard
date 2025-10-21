using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.SharedKernel
{
    /// <summary>
    /// Статический класс с валидационными функциями.
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// Проверяет, что строка не является null, пустой или состоящей только из пробелов.
        /// </summary>
        public static Func<string, bool> IsNotNullOrEmpty => value => !string.IsNullOrWhiteSpace(value);

        /// <summary>
        /// Создает функцию проверки соответствия строки регулярному выражению.
        /// </summary>
        /// <param name="regex">Регулярное выражение для проверки.</param>
        /// <returns>Функция валидации.</returns>
        public static Func<string, bool> IsMatchRegex(Regex regex) => s => regex.IsMatch(s);

        /// <summary>
        /// Создает функцию проверки, что длина строки меньше или равна указанному значению.
        /// </summary>
        /// <param name="value">Максимальная длина строки.</param>
        /// <returns>Функция валидации.</returns>
        public static Func<string, bool> SmallerThan(int value) => s => s.Length <= value;

        /// <summary>
        /// Создает функцию проверки, что длина строки больше или равна указанному значению.
        /// </summary>
        /// <param name="value">Минимальная длина строки.</param>
        /// <returns>Функция валидации.</returns>
        public static Func<string, bool> BiggerThan(int value) => s => s.Length >= value;

        /// <summary>
        /// Проверяет, что строка содержит хотя бы одну цифру.
        /// </summary>
        public static Func<string, bool> HasDigits => s => s.Any(c => char.IsDigit(c));

        /// <summary>
        /// Проверяет, что строка содержит хотя бы одну строчную букву.
        /// </summary>
        public static Func<string, bool> HasLowercaseLetters => s => s.Any(c => char.IsLower(c));

        /// <summary>
        /// Проверяет, что строка содержит хотя бы одну прописную букву.
        /// </summary>
        public static Func<string, bool> HasUppercaseLetters => s => s.Any(c => char.IsUpper(c));

        /// <summary>
        /// Проверяет, что строка содержит специальные символы.
        /// </summary>
        public static Func<string, bool> HasSpecialCharacters =>
            s => !(HasDigits(s) || HasLowercaseLetters(s) || HasUppercaseLetters(s));

    }
}
