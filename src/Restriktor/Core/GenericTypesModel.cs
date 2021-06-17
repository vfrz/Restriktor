using System;
using System.Linq;
using Restriktor.Extensions;

namespace Restriktor.Core
{
    public class GenericTypesModel
    {
        internal const string WildcardCharacter = "*";

        internal const string TypesSeparator = ",";

        public TypeModel[] Types { get; }

        public bool IsWildcard { get; }

        public GenericTypesModel(TypeModel[] types, bool isWildcard = false)
        {
            Types = types ?? Array.Empty<TypeModel>();
            IsWildcard = isWildcard;
        }

        public static GenericTypesModel Parse(string genericTypes)
        {
            var types = genericTypes.SplitOrEmptyArray(TypesSeparator).TrimAll().Select(TypeModel.Parse).ToArray();
            var isWildcard = string.Equals(genericTypes, WildcardCharacter, StringComparison.Ordinal);

            return new GenericTypesModel(types, isWildcard);
        }
        
        public static GenericTypesModel FromType(Type type)
        {
            if (!type.IsGenericType)
                throw new Exception($"Type {type} is not generic");
            
            var parameters = type.GetGenericArguments()?.Select(TypeModel.FromType).ToArray();

            return new GenericTypesModel(parameters);
        }

        public bool Match(GenericTypesModel another, bool perfectMatch = false)
        {
            if (IsWildcard && !perfectMatch)
                return true;

            if (perfectMatch && another.Types.Length != Types.Length)
                return false;

            if (another.Types.Length > Types.Length)
                return false;

            for (var i = 0; i < another.Types.Length; i++)
            {
                if (!Types[i].Match(another.Types[i]))
                    return false;
            }

            return true;
        }
    }
}