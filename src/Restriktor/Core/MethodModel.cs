using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Restriktor.Core
{
    public class MethodModel
    {
        private static readonly Regex MethodRegex = new(@"(?<type>\S+)\.(?<name>\S+)\((?<parameters>.*)\)");

        public string Name { get; }

        public MethodParametersModel Parameters { get; }

        public TypeModel Type { get; }
        
        public GenericTypesModel GenericTypes { get; }

        public MethodModel(string name, MethodParametersModel parameters, TypeModel type, GenericTypesModel genericTypes = null)
        {
            Name = name;
            Parameters = parameters;
            Type = type;
            GenericTypes = genericTypes;
        }

        public static MethodModel Parse(string method)
        {
            var regexMatch = MethodRegex.Match(method);

            if (!regexMatch.Success)
                throw new Exception();

            var methodModel = new MethodModel(regexMatch.Groups["name"].Value, regexMatch.Groups["parameters"].Value, regexMatch.Groups["type"].Value);
            return methodModel;
        }

        public static MethodModel FromMethodInfo(MethodInfo methodInfo)
        {
            var name = methodInfo.Name;
            var parameters = MethodParametersModel.FromParameterInfos(methodInfo.GetParameters());
            var typeModel = TypeModel.FromType(methodInfo.DeclaringType);

            return new MethodModel(name, parameters, typeModel);
        }

        public bool Match(MethodModel another, bool perfectMatch = false)
        {
            if (!Type.Match(another.Type))
                return false;

            if (!string.Equals(Name, another.Name, StringComparison.Ordinal))
                return false;
            
            if (GenericTypes is not null)
            {
                if (another.GenericTypes is null)
                    return false;

                if (!GenericTypes.Match(another.GenericTypes, perfectMatch))
                    return false;
            }

            return Parameters.Match(another.Parameters, perfectMatch);
        }

        public override string ToString()
        {
            return $"{Type}.{Name}({Parameters})";
        }

        public static implicit operator MethodModel(string method) => Parse(method);
    }
}