using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Restriktor.Extensions;

namespace Restriktor.Core
{
    public class NamespaceModel
    {
        private const string PartsSeparator = ".";

        public ReadOnlyCollection<string> Parts => Array.AsReadOnly(_parts);
        
        private readonly string[] _parts;

        public bool IsGlobalNamespace => _parts.Length == 0;

        public NamespaceModel(string[] parts)
        {
            if (parts?.Any() == true)
            {
                var regex = CSharpSpecificationRegexes.Identifier.WrapInStartAndEnd();
                foreach (var part in parts)
                    if (part is null || !regex.IsMatch(part))
                        throw new FormatException($"A part of the namespace isn't a valid identifier: '{part}'");
            }

            _parts = parts ?? Array.Empty<string>();
        }

        public static NamespaceModel Parse(string ns)
        {
            if (ns is null)
                return new NamespaceModel(null);
            
            var match = CSharpSpecificationRegexes.Namespace.Match(ns);

            if (!match.Success)
                throw new FormatException($"Can't parse namespace from value: '{ns}'");

            var parts = match.Groups.Cast<Group>()
                .Skip(1)
                .Where(group => group.Success)
                .SelectMany(group => group.Captures.Select(capture => capture.Value))
                .ToArray();
            
            var namespaceModel = new NamespaceModel(parts);
            return namespaceModel;
        }

        public NamespaceModel Parent()
        {
            if (IsGlobalNamespace)
                return null;

            if (_parts.Length > 1)
                return new NamespaceModel(_parts.SkipLast(1).ToArray());

            if (_parts.Length == 1)
                return new NamespaceModel(null);

            return null;
        }

        public bool Match(NamespaceModel another, bool perfectMatch = false)
        {
            if (perfectMatch && another._parts.Length != _parts.Length)
                return false;

            if (another._parts.Length > _parts.Length)
                return false;

            for (var i = 0; i < another._parts.Length; i++)
            {
                if (!string.Equals(_parts[i], another._parts[i], StringComparison.Ordinal))
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            if (IsGlobalNamespace)
                return "";

            return string.Join(PartsSeparator, _parts);
        }

        public static implicit operator NamespaceModel(string ns) => Parse(ns);
    }
}