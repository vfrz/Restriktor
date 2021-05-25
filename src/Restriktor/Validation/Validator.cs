using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Restriktor.Core;
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

            _semanticModel = compilation.GetSemanticModel(syntaxTree);

            Visit(syntaxTree.GetRoot());

            return _result;
        }

        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            var policy = _policyGroup.GetPolicyForNamespace(node.Name.ToString());

            if (policy.PolicyType == PolicyType.Deny)
                _result.Problems.Add(ValidationProblem.FromPolicy(policy, node));

            base.VisitUsingDirective(node);
        }

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            var typeInfo = _semanticModel.GetTypeInfo(node.Type);

            if (typeInfo.Type is null)
                throw new Exception("");

            var typeModel = typeInfo.Type.ContainingNamespace.IsGlobalNamespace
                ? new TypeModel(typeInfo.Type.Name, null)
                : new TypeModel(typeInfo.Type.Name, typeInfo.Type.ContainingNamespace?.ToString());

            var policy = _policyGroup.GetPolicyForType(typeModel);

            if (policy.PolicyType == PolicyType.Deny)
                _result.Problems.Add(ValidationProblem.FromPolicy(policy, node));

            base.VisitVariableDeclaration(node);
        }
    }
}