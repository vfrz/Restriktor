using System;
using System.Collections.Immutable;
using System.Linq;
using Restriktor.Extensions;

namespace Restriktor.Core
{
    public class NamespaceModel
    {
        private const string PartSeparator = ".";

        public ImmutableArray<string> Parts { get; }

        public bool IsGlobalNamespace => Parts.IsDefaultOrEmpty;

        public NamespaceModel(string[] parts)
        {
            if (parts?.Any() == true)
            {
                var regex = CSharpSpecificationRegexes.Identifier.WrapInStartAndEnd();
                foreach (var part in parts)
                    if (part is null || !regex.IsMatch(part))
                        throw new FormatException($"A part of the namespace isn't a valid identifier: '{part}'");
            }
            
            Parts = parts?.ToImmutableArray() ?? new ImmutableArray<string>();
        }

        public static NamespaceModel Parse(string ns)
        {
            var parts = ns.SplitOrEmptyArray(PartSeparator);
            var namespaceModel = new NamespaceModel(parts);
            return namespaceModel;
        }

        public NamespaceModel Parent()
        {
            if (IsGlobalNamespace)
                return null;
            
            if (Parts.Length > 1)
                return new NamespaceModel(Parts.SkipLast(1).ToArray());

            if (Parts.Length == 1)
                return new NamespaceModel(null);

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
                return "";

            return string.Join(PartSeparator, Parts);
        }

        public static implicit operator NamespaceModel(string ns) => Parse(ns);
    }
}