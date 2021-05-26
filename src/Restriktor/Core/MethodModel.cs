using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Restriktor.Core
{
    public class MethodModel
    {
        public string Name { get; }

        public MethodSignatureModel Signature { get; }

        public TypeModel Type { get; }

        private readonly Regex _methodSignatureRegex = new(@"(?<type>\S+)\.(?<name>\S+)\((?<signature>.*)\)");

        public MethodModel(string name, MethodSignatureModel signature, TypeModel type)
        {
            Name = name;
            Signature = signature;
            Type = type;
        }

        public MethodModel(string nameWithTypeAndSignature)
        {
            var regexMatch = _methodSignatureRegex.Match(nameWithTypeAndSignature);

            if (!regexMatch.Success)
                throw new Exception();

            Name = regexMatch.Groups["name"].Value;
            Signature = regexMatch.Groups["signature"].Value;
            Type = regexMatch.Groups["type"].Value;
        }

        public MethodModel(MethodInfo methodInfo)
        {
            throw new NotImplementedException();
        }

        public bool Match(MethodModel another)
        {
            if (!Type.Match(another.Type))
                return false;

            if (!string.Equals(Name, another.Name, StringComparison.Ordinal))
                return false;

            return Signature.Match(another.Signature);
        }

        public override string ToString()
        {
            return $"{Type}.{Name}({Signature})";
        }
    }
}