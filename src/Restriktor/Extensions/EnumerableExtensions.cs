using System.Collections.Generic;
using System.Linq;

namespace Restriktor.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<string> TrimAll(this IEnumerable<string> enumerable)
        {
            return enumerable.Select(str => str.Trim());
        }
    }
}