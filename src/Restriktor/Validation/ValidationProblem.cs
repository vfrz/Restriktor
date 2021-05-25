using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Restriktor.Extensions;
using Restriktor.Policies;

namespace Restriktor.Validation
{
    public abstract class ValidationProblem
    {
        public FileLinePositionSpan FileLinePositionSpan { get; }

        public ValidationProblem(FileLinePositionSpan fileLinePositionSpan)
        {
            FileLinePositionSpan = fileLinePositionSpan;
        }

        public ValidationProblem(CSharpSyntaxNode syntaxNode) : this(syntaxNode.GetFileLinePositionSpan())
        {
        }

        public static ValidationProblem FromPolicy(Policy policy, CSharpSyntaxNode syntaxNode)
        {
            if (policy is NamespacePolicy namespacePolicy)
                return new NamespaceRestrictedProblem(namespacePolicy.Namespace, syntaxNode);

            if (policy is TypePolicy typePolicy)
                return new TypeRestrictedProblem(typePolicy.Type, syntaxNode);
            
            throw new Exception($"Unknown policy type: {policy.GetType()}");
        }
    }
}