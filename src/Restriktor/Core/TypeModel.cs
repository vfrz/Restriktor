using System;
using System.Linq;

namespace Restriktor.Core
{
    public class TypeModel
    {
        internal const string Separator = ".";

        public string Name { get; }

        public NamespaceModel Namespace { get; }
        
        public GenericTypesModel GenericTypes { get; }

        public TypeModel(string name, NamespaceModel ns, GenericTypesModel genericTypes = null)
        {
            Name = name;
            Namespace = ns;
            GenericTypes = genericTypes;
        }

        public static TypeModel Parse(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException($"Failed to parse {nameof(TypeModel)} because argument {nameof(type)} is empty");

            var split = type.Split(Separator);
            var typeModel = new TypeModel(split.Last(), new NamespaceModel(split.SkipLast(1).ToArray()));
            return typeModel;
        }

        public static TypeModel FromType(Type type)
        {
            return Parse(type.FullName);
        }

        public bool Match(TypeModel another, bool perfectMatch = false)
        {
            if (Namespace is null)
            {
                if (another.Namespace is not null)
                    return false;
            }
            else if (!Namespace.Match(another.Namespace, true))
            {
                return false;
            }

            if (!string.Equals(Name, another.Name, StringComparison.Ordinal))
                return false;

            if (GenericTypes is not null)
            {
                if (another.GenericTypes is null)
                    return false;

                if (!GenericTypes.Match(another.GenericTypes, perfectMatch))
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            if (Namespace is not null)
                return $"{Namespace}{Separator}{Name}";

            return Name;
        }

        public static implicit operator TypeModel(string type) => Parse(type);
    }
}