using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Restriktor.Extensions;

namespace Restriktor.Core
{
    public class MethodParametersModel
    {
        internal const string WildcardCharacter = "*";

        internal const string ParametersSeparator = ",";

        public ReadOnlyCollection<TypeModel> Parameters => Array.AsReadOnly(_parameters);
        
        private readonly TypeModel[] _parameters;

        public bool IsWildcard { get; }

        public MethodParametersModel(TypeModel[] parameters, bool isWildcard = false)
        {
            _parameters = parameters ?? Array.Empty<TypeModel>();
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

            if (_parameters.Length != another._parameters.Length)
                return false;

            for (var i = 0; i < _parameters.Length; i++)
            {
                if (!_parameters[i].Match(another._parameters[i]))
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            if (IsWildcard)
                return WildcardCharacter;

            return string.Join(ParametersSeparator, _parameters.Select(p => p.ToString()));
        }

        public static implicit operator MethodParametersModel(string methodParameters) => Parse(methodParameters);
    }
}