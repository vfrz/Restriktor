using System;

namespace Restriktor.Extensions
{
    public static class StringExtensions
    {
        public static string[] SplitOrEmptyArray(this string value, string separator)
        {
            return value is null ? Array.Empty<string>() : value.Split(separator);
        }
    }
}