using System;
using System.Collections.Immutable;
using System.Linq;

namespace Restriktor.Core
{
    public class NamespaceModel
    {
        private const string Separator = ".";

        public ImmutableArray<string> Parts { get; }

        public NamespaceModel(string[] parts)
        {
            if (parts.Any(string.IsNullOrWhiteSpace))
                throw new FormatException("A namespace part can't be null or whitespace");

            Parts = parts.ToImmutableArray();
        }

        public NamespaceModel(string ns) : this(ns.Split(Separator))
        {
        }

        public NamespaceModel Parent()
        {
            if (Parts.Length > 1)
                return new NamespaceModel(Parts.SkipLast(1).ToArray());

            return null;
        }

        public bool Match(NamespaceModel another, bool perfectMatch = false)
        {
            if (perfectMatch)
                return string.Equals(ToString(), another.ToString(), StringComparison.Ordinal);

            if (another.Parts.Length > Parts.Length)
                return false;

            for (var i = 0; i < another.Parts.Length; i++)
            {
                if (!string.Equals(Parts[i], another.Parts[i], StringComparison.Ordinal))
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            return string.Join(Separator, Parts);
        }

        public static implicit operator string(NamespaceModel ns) => ns?.ToString();

        public static implicit operator NamespaceModel(string ns) => ns is null ? null : new NamespaceModel(ns);
    }
}