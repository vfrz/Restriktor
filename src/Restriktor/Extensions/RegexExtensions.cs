using System.Text.RegularExpressions;

namespace Restriktor.Extensions
{
    internal static class RegexExtensions
    {
        public static Regex WrapInStartAndEnd(this Regex regex)
        {
            return new($"^{regex}$", regex.Options, regex.MatchTimeout);
        }
    }
}