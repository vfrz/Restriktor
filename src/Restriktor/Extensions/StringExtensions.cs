using System;

namespace Restriktor.Extensions
{
    public static class StringExtensions
    {
        public static string[] SplitWithEmptyOrNull(this string value, string separator)
        {
            if (string.IsNullOrEmpty(value))
                return Array.Empty<string>();

            return value.Split(separator);
        }
    }
}