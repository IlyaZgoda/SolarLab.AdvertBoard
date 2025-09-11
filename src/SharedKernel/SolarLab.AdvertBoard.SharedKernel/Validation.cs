using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.SharedKernel
{
    public static class Validation
    {
        public static Func<string, bool> IsNotNullOrEmpty => value => !string.IsNullOrWhiteSpace(value);

        public static Func<string, bool> IsMatchRegex(Regex regex) => s => regex.IsMatch(s);
        public static Func<string, bool> SmallerThan(int value) => s => s.Length <= value;
        public static Func<string, bool> BiggerThan(int value) => s => s.Length >= value;
        public static Func<string, bool> HasDigits => s => s.Any(c => char.IsDigit(c));
        public static Func<string, bool> HasLowercaseLetters => s => s.Any(c => char.IsLower(c));
        public static Func<string, bool> HasUppercaseLetters => s => s.Any(c => char.IsUpper(c));
        public static Func<string, bool> HasSpecialCharacters => 
            s => !(HasDigits(s) || HasLowercaseLetters(s) || HasUppercaseLetters(s));
        
    }
}
