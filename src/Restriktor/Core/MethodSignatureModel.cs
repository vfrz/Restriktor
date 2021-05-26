using System;
using System.Linq;
using Restriktor.Extensions;

namespace Restriktor.Core
{
    public class MethodSignatureModel
    {
        internal const string WildcardCharacter = "*";
        
        internal const string Separator = ",";
        
        public TypeModel[] Parameters { get; }
        
        public bool IsWildcard { get; }

        public MethodSignatureModel(TypeModel[] parameters, bool isWildcard = false)
        {
            Parameters = parameters ?? Array.Empty<TypeModel>();
            IsWildcard = isWildcard;
        }

        public MethodSignatureModel(string parameters)
        {
            Parameters = parameters.SplitWithEmptyOrNull(Separator).TrimAll().Select(type => new TypeModel(type)).ToArray();
            IsWildcard = string.Equals(parameters, WildcardCharacter, StringComparison.Ordinal);
        }

        public bool Match(MethodSignatureModel another, bool perfectMatch = false)
        {
            if (IsWildcard && !perfectMatch)
                return true;

            if (Parameters.Length != another.Parameters.Length)
                return false;

            for (var i = 0; i < Parameters.Length; i++)
            {
                if (!Parameters[i].Match(another.Parameters[i]))
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            if (IsWildcard)
                return WildcardCharacter;

            return string.Join(Separator, Parameters.Select(p => p.ToString()));
        }
        
        public static implicit operator string(MethodSignatureModel methodSignature) => methodSignature?.ToString();

        public static implicit operator MethodSignatureModel(string methodSignature) => new(methodSignature);
    }
}