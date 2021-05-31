using Microsoft.CodeAnalysis.CSharp;
using Restriktor.Core;

namespace Restriktor.Validation
{
    public class TypeRestrictedProblem : ValidationProblem
    {
        public TypeModel Type { get; }

        public TypeRestrictedProblem(TypeModel type, CSharpSyntaxNode syntaxNode) : base(syntaxNode)
        {
            Type = type;
        }
        
        public override string ToString()
        {
            return $"({FileLinePositionSpan.StartLinePosition}): Denied type usage: '{Type}'";
        }
    }
}