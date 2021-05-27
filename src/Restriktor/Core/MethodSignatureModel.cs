using System;
using System.Linq;
using System.Reflection;
using Restriktor.Extensions;

namespace Restriktor.Core
{
    public class MethodSignatureModel
    {
        internal const string WildcardCharacter = "*";

        internal const string ParametersSeparator = ",";

        public TypeModel[] Parameters { get; }

        public bool IsWildcard { get; }

        public MethodSignatureModel(TypeModel[] parameters, bool isWildcard = false)
        {
            Parameters = parameters ?? Array.Empty<TypeModel>();
            IsWildcard = isWildcard;
        }

        public static MethodSignatureModel Parse(string methodSignature)
        {
            var parameters = methodSignature.SplitWithEmptyOrNull(ParametersSeparator).TrimAll().Select(TypeModel.Parse).ToArray();
            var isWildcard = string.Equals(methodSignature, WildcardCharacter, StringComparison.Ordinal);

            return new MethodSignatureModel(parameters, isWildcard);
        }

        public static MethodSignatureModel FromParameterInfos(params ParameterInfo[] parameterInfos)
        {
            var parameters = parameterInfos?.Select(p => TypeModel.FromType(p.ParameterType)).ToArray();

            return new MethodSignatureModel(parameters);
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

            return string.Join(ParametersSeparator, Parameters.Select(p => p.ToString()));
        }

        public static implicit operator MethodSignatureModel(string methodSignature) => Parse(methodSignature);
    }
}