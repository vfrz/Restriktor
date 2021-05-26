using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Restriktor.Core
{
    public class MethodModel
    {
        public string Name { get; }

        public MethodSignatureModel Signature { get; }

        public TypeModel Type { get; }

        //TODO Ultra slow regex => need optimization before usage
        private readonly Regex _methodSignatureRegex = new(@"(?'type'(?:\S+))+\.(?:(?'name'\S+)\((?'signature'.*)\))");

        public MethodModel(string name, MethodSignatureModel signature, TypeModel type)
        {
            Name = name;
            Signature = signature;
            Type = type;
        }

        public MethodModel(string nameWithTypeAndSignature)
        {
            var signatureSplit = nameWithTypeAndSignature.Split("(");
            var typeNameSplit = signatureSplit[0].Split(TypeModel.Separator);

            Name = typeNameSplit.Last();
            Signature = signatureSplit[1][..^1];
            Type = string.Join(TypeModel.Separator, typeNameSplit.SkipLast(1));
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