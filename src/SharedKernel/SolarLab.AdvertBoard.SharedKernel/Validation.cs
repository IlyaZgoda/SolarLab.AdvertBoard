using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.SharedKernel
{
    public static class Validation
    {
        public static Func<string, bool> IsNotNullOrEmpty => value => !string.IsNullOrWhiteSpace(value);

        public static Func<string, bool> IsMatchRegex(Regex regex) => value => regex.IsMatch(value);
    }
}
