using System;

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
                throw new FormatException($"Failed to parse {nameof(TypeModel)} because argument {nameof(type)} is empty");

            var match = CSharpSpecificationRegexes.Type.Match(type);

            if (!match.Success || !match.Groups["Type"].Success)
                throw new FormatException($"Failed to parse {nameof(TypeModel)} from value: '{type}'");

            var typeName = match.Groups["Type"].Value;

            var namespaceModel = match.Groups["Namespace"].Success ? NamespaceModel.Parse(match.Groups["Namespace"].Value) : new NamespaceModel(null);
            
            var typeModel = new TypeModel(typeName, namespaceModel);
            return typeModel;
        }

        public static TypeModel FromType(Type type)
        {
            //TODO Take type.DeclaringType into account
            
            var typeName = type.Name;
            var namespaceModel = NamespaceModel.Parse(type.Namespace);
            
            return new TypeModel(typeName, namespaceModel);
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
            if (Namespace is not null && !Namespace.IsGlobalNamespace)
                return $"{Namespace}{Separator}{Name}";

            return Name;
        }

        public static implicit operator TypeModel(string type) => Parse(type);
    }
}