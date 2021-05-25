using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Restriktor.Extensions
{
    public static class CSharpSyntaxNodeExtensions
    {
        public static FileLinePositionSpan GetFileLinePositionSpan(this CSharpSyntaxNode node)
        {
            return node.SyntaxTree.GetLineSpan(node.Span);
        }
    }
}