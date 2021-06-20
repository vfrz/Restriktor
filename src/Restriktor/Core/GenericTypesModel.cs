using System;
using System.Collections.ObjectModel;
using System.Linq;
using Restriktor.Extensions;

namespace Restriktor.Core
{
    public class GenericTypesModel
    {
        internal const string WildcardCharacter = "*";

        internal const string TypesSeparator = ",";

        public ReadOnlyCollection<TypeModel> Types => Array.AsReadOnly(_types);

        private readonly TypeModel[] _types;

        public bool IsWildcard { get; }

        public GenericTypesModel(TypeModel[] types, bool isWildcard = false)
        {
            _types = types ?? Array.Empty<TypeModel>();
            IsWildcard = isWildcard;
        }

        public static GenericTypesModel Parse(string genericTypes)
        {
            if (string.IsNullOrWhiteSpace(genericTypes))
                throw new FormatException($"Can't parse generic types from: '{genericTypes}'");

            var isWildcard = string.Equals(genericTypes, WildcardCharacter, StringComparison.Ordinal);

            if (isWildcard)
                return new GenericTypesModel(null, true);

            var types = genericTypes.SplitOrEmptyArray(TypesSeparator).TrimAll().Select(TypeModel.Parse).ToArray();

            return new GenericTypesModel(types);
        }

        public static GenericTypesModel FromType(Type type)
        {
            if (!type.IsGenericType)
                throw new ArgumentException($"Type {type} is not generic");

            var genericArguments = type.GetGenericArguments();

            if (genericArguments.All(arg => arg.IsGenericParameter))
                return new GenericTypesModel(null, true);

            var parameters = genericArguments.Select(TypeModel.FromType).ToArray();

            return new GenericTypesModel(parameters);
        }

        public bool Match(GenericTypesModel another, bool perfectMatch = false)
        {
            if (IsWildcard && !perfectMatch)
                return true;

            if (perfectMatch && another._types.Length != _types.Length)
                return false;

            if (another._types.Length > _types.Length)
                return false;

            for (var i = 0; i < another._types.Length; i++)
            {
                if (!_types[i].Match(another._types[i]))
                    return false;
            }

            return true;
        }
    }
}