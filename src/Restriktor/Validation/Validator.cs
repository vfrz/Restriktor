using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Restriktor.Core;
using Restriktor.Extensions;
using Restriktor.Policies;

namespace Restriktor.Validation
{
    public class Validator : CSharpSyntaxWalker
    {
        private readonly PolicyGroup _policyGroup;

        private ValidationResult _result;

        private SemanticModel _semanticModel;

        public Validator(PolicyGroup policyGroup)
        {
            _policyGroup = policyGroup;
        }

        public ValidationResult Validate(string code)
        {
            _result = new ValidationResult();

            var trustedAssemblies = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES"))
                ?.Split(Path.PathSeparator)
                .Select(assemblyPath => MetadataReference.CreateFromFile(assemblyPath));

            if (trustedAssemblies is null)
                throw new Exception("Failed to fetch trusted assemblies");

            var assemblies = new List<MetadataReference>();
            assemblies.AddRange(trustedAssemblies);

            var syntaxTree = CSharpSyntaxTree.ParseText(code, CSharpParseOptions.Default);

            var compilation = CSharpCompilation.Create("Restriktor",
                new[] {syntaxTree},
                assemblies,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var diagnostics = compilation.GetDiagnostics();

            _result.Problems.AddRange(diagnostics
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(diagnostic => new CompilationProblem(diagnostic)));

            _semanticModel = compilation.GetSemanticModel(syntaxTree);

            Visit(syntaxTree.GetRoot());

            return _result;
        }

        private void ValidateTypeInfo(TypeInfo typeInfo, CSharpSyntaxNode node)
        {
            var typeModel = typeInfo.Type.ContainingNamespace.IsGlobalNamespace
                ? new TypeModel(typeInfo.Type.Name, null)
                : new TypeModel(typeInfo.Type.Name, typeInfo.Type.ContainingNamespace?.ToString());

            var policy = _policyGroup.GetPolicyForType(typeModel);

            if (policy.PolicyType == PolicyType.Deny)
                _result.Problems.Add(ValidationProblem.FromPolicy(policy, node));
        }

        public override void VisitAttribute(AttributeSyntax node)
        {
            var typeInfo = _semanticModel.GetTypeInfo(node);

            if (typeInfo.Type is null)
                throw new Exception($"Failed to fetch type info from attribute: {node.ToString()} at {node.GetFileLinePositionSpan().StartLinePosition}");

            ValidateTypeInfo(typeInfo, node);

            base.VisitAttribute(node);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (_semanticModel.GetSymbolInfo(node).Symbol is IMethodSymbol methodSymbol)
            {
                var parameters = methodSymbol.Parameters.Select(p => $"{p.Type.ContainingNamespace}.{p.Type.Name}");
                var method = $"{methodSymbol.ContainingType}.{methodSymbol.Name}({string.Join(",", parameters)})";

                var policy = _policyGroup.GetPolicyForMethod(method);

                if (policy.PolicyType == PolicyType.Deny)
                    _result.Problems.Add(ValidationProblem.FromPolicy(policy, node));
            }
            else
            {
                throw new Exception($"Failed to fetch method symbol of invocation expression: {node.ToString()} at {node.GetFileLinePositionSpan().StartLinePosition}");
            }

            base.VisitInvocationExpression(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var typeInfo = _semanticModel.GetTypeInfo(node.ReturnType);

            if (typeInfo.Type is null)
                throw new Exception($"Failed to fetch return type info from method declaration: {node.ToString()} at {node.GetFileLinePositionSpan().StartLinePosition}");

            ValidateTypeInfo(typeInfo, node);

            base.VisitMethodDeclaration(node);
        }

        public override void VisitParameter(ParameterSyntax node)
        {
            var typeInfo = _semanticModel.GetTypeInfo(node.Type);
            
            if (typeInfo.Type is null)
                throw new Exception($"Failed to fetch type info from parameter: {node.ToString()} at {node.GetFileLinePositionSpan().StartLinePosition}");

            ValidateTypeInfo(typeInfo, node);

            base.VisitParameter(node);
        }

        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            var policy = _policyGroup.GetPolicyForNamespace(node.Name.ToString(), true);

            if (policy.PolicyType == PolicyType.Deny)
                _result.Problems.Add(ValidationProblem.FromPolicy(policy, node));

            base.VisitUsingDirective(node);
        }

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            var typeInfo = _semanticModel.GetTypeInfo(node.Type);

            if (typeInfo.Type is null)
                throw new Exception($"Failed to fetch type info from variable declaration: {node.ToString()} at {node.GetFileLinePositionSpan().StartLinePosition}");

            ValidateTypeInfo(typeInfo, node);

            base.VisitVariableDeclaration(node);
        }

        public override void VisitSimpleBaseType(SimpleBaseTypeSyntax node)
        {
            var typeInfo = _semanticModel.GetTypeInfo(node.Type);

            if (typeInfo.Type is null)
                throw new Exception($"Failed to fetch type info from simple base type: {node.ToString()} at {node.GetFileLinePositionSpan().StartLinePosition}");

            ValidateTypeInfo(typeInfo, node);

            base.VisitSimpleBaseType(node);
        }
    }
}