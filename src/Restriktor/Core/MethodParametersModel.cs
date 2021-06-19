using System;
using System.Linq;
using System.Reflection;
using Restriktor.Extensions;

namespace Restriktor.Core
{
    public class MethodParametersModel
    {
        internal const string WildcardCharacter = "*";

        internal const string ParametersSeparator = ",";

        public TypeModel[] Parameters { get; }

        public bool IsWildcard { get; }

        public MethodParametersModel(TypeModel[] parameters, bool isWildcard = false)
        {
            Parameters = parameters ?? Array.Empty<TypeModel>();
            IsWildcard = isWildcard;
        }

        public static MethodParametersModel Parse(string methodParameters)
        {
            if (string.IsNullOrWhiteSpace(methodParameters))
                return new MethodParametersModel(null);
            
            var isWildcard = string.Equals(methodParameters, WildcardCharacter, StringComparison.Ordinal);
            
            if (isWildcard)
                return new MethodParametersModel(null, true);
            
            var parameters = methodParameters.SplitOrEmptyArray(ParametersSeparator).TrimAll().Select(TypeModel.Parse).ToArray();

            return new MethodParametersModel(parameters);
        }

        public static MethodParametersModel FromParameterInfos(params ParameterInfo[] parameterInfos)
        {
            var parameters = parameterInfos?.Select(p => TypeModel.FromType(p.ParameterType)).ToArray();

            return new MethodParametersModel(parameters);
        }

        public bool Match(MethodParametersModel another, bool perfectMatch = false)
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

        public static implicit operator MethodParametersModel(string methodParameters) => Parse(methodParameters);
    }
}