using Microsoft.CodeAnalysis.CSharp;
using Restriktor.Core;

namespace Restriktor.Validation
{
    public class NamespaceRestrictedProblem : ValidationProblem
    {
        public NamespaceModel Namespace { get; }

        public NamespaceRestrictedProblem(NamespaceModel ns, CSharpSyntaxNode syntaxNode) : base(syntaxNode)
        {
            Namespace = ns;
        }

        public override string ToString()
        {
            return $"Denied namespace usage: '{Namespace}' at {FileLinePositionSpan.StartLinePosition}";
        }
    }
}