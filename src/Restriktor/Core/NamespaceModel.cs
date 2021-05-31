using System;
using System.Collections.Immutable;
using System.Linq;
using Restriktor.Extensions;

namespace Restriktor.Core
{
    public class NamespaceModel
    {
        private const string Separator = ".";

        public ImmutableArray<string> Parts { get; }

        public bool IsGlobalNamespace => Parts.Length == 0;

        public NamespaceModel(string[] parts)
        {
            if (parts.Any(string.IsNullOrWhiteSpace))
                throw new FormatException("A namespace part can't be null or whitespace");

            Parts = parts.ToImmutableArray();
        }

        public static NamespaceModel Parse(string ns)
        {
            var parts = ns.SplitEmptyIfNull(Separator);
            var namespaceModel = new NamespaceModel(parts);
            return namespaceModel;
        }

        public NamespaceModel Parent()
        {
            if (Parts.Length > 1)
                return new NamespaceModel(Parts.SkipLast(1).ToArray());

            if (Parts.Length == 1)
                return new NamespaceModel(Array.Empty<string>());

            return null;
        }

        public bool Match(NamespaceModel another, bool perfectMatch = false)
        {
            if (perfectMatch && another.Parts.Length != Parts.Length)
                return false;

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
            if (IsGlobalNamespace)
                return "<global namespace>";

            return string.Join(Separator, Parts);
        }

        public static implicit operator NamespaceModel(string ns) => Parse(ns);
    }
}