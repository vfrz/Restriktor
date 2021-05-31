using Microsoft.CodeAnalysis.CSharp;
using Restriktor.Core;

namespace Restriktor.Validation
{
    public class MethodRestrictedProblem : ValidationProblem
    {
        public MethodModel Method { get; }

        public MethodRestrictedProblem(MethodModel method, CSharpSyntaxNode syntaxNode) : base(syntaxNode)
        {
            Method = method;
        }

        public override string ToString()
        {
            return $"({FileLinePositionSpan.StartLinePosition}): Denied method usage: '{Method}'";
        }
    }
}