using System;
using System.Linq;

namespace Restriktor.Core
{
    public class TypeModel
    {
        internal const string Separator = ".";

        public string Name { get; }

        public NamespaceModel Namespace { get; }

        public TypeModel(string name, NamespaceModel ns)
        {
            Name = name;
            Namespace = ns;
        }

        public TypeModel(string nameWithNamespace)
        {
            var split = nameWithNamespace.Split(Separator);
            Name = split.Last();
            Namespace = new NamespaceModel(split.SkipLast(1).ToArray());
        }

        public TypeModel(Type type) : this(type.Name, type.Namespace)
        {
        }

        public bool Match(TypeModel another)
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

            return string.Equals(Name, another.Name, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            if (Namespace is not null)
                return $"{Namespace}{Separator}{Name}";

            return Name;
        }

        public static implicit operator string(TypeModel typeModel) => typeModel?.ToString();

        public static implicit operator TypeModel(string type)
        {
            var parts = type.Split(Separator);

            var name = parts.Last();

            if (parts.Length > 1)
                return new TypeModel(name, new NamespaceModel(parts.SkipLast(1).ToArray()));

            return new TypeModel(name, null);
        }

        public static implicit operator TypeModel(Type type) => new(type.Name, type.Namespace);
    }
}