using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Restriktor.Core
{
    public class MethodModel
    {
        private static readonly Regex MethodSignatureRegex = new(@"(?<type>\S+)\.(?<name>\S+)\((?<signature>.*)\)");

        public string Name { get; }

        public MethodSignatureModel Signature { get; }

        public TypeModel Type { get; }

        public MethodModel(string name, MethodSignatureModel signature, TypeModel type)
        {
            Name = name;
            Signature = signature;
            Type = type;
        }

        public static MethodModel Parse(string method)
        {
            var regexMatch = MethodSignatureRegex.Match(method);

            if (!regexMatch.Success)
                throw new Exception();

            var methodModel = new MethodModel(regexMatch.Groups["name"].Value, regexMatch.Groups["signature"].Value, regexMatch.Groups["type"].Value);
            return methodModel;
        }

        public static MethodModel FromMethodInfo(MethodInfo methodInfo)
        {
            var name = methodInfo.Name;
            var signature = MethodSignatureModel.FromParameterInfos(methodInfo.GetParameters());
            var typeModel = TypeModel.FromType(methodInfo.DeclaringType);

            return new MethodModel(name, signature, typeModel);
        }

        public bool Match(MethodModel another, bool perfectMatch = false)
        {
            if (!Type.Match(another.Type))
                return false;

            if (!string.Equals(Name, another.Name, StringComparison.Ordinal))
                return false;

            return Signature.Match(another.Signature, perfectMatch);
        }

        public override string ToString()
        {
            return $"{Type}.{Name}({Signature})";
        }

        public static implicit operator MethodModel(string method) => Parse(method);
    }
}