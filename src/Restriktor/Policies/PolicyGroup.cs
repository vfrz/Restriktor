using System;
using System.Collections.Generic;
using System.Linq;
using Restriktor.Core;

namespace Restriktor.Policies
{
    public class PolicyGroup
    {
        public PolicyType DefaultPolicyType { get; set; }

        public IReadOnlyCollection<Policy> Policies => _policies.AsReadOnly();

        private readonly List<Policy> _policies;

        public PolicyGroup(PolicyType defaultPolicyType = PolicyType.Deny)
        {
            DefaultPolicyType = defaultPolicyType;

            _policies = new List<Policy>();
        }

        public PolicyGroup AddNamespacePolicy(NamespaceModel ns, PolicyType policyType)
        {
            var existingExplicitPolicy = GetExplicitNamespacePolicy(ns, true);

            if (existingExplicitPolicy is not null)
                throw new Exception($"A policy for namespace '{ns}' already exists");

            _policies.Add(new NamespacePolicy(ns, policyType));

            return this;
        }

        public PolicyGroup AllowNamespace(NamespaceModel ns) => AddNamespacePolicy(ns, PolicyType.Allow);

        public PolicyGroup DenyNamespace(NamespaceModel ns) => AddNamespacePolicy(ns, PolicyType.Deny);

        public Policy GetPolicyForNamespace(NamespaceModel ns, bool perfectMatch = false)
        {
            var explicitPolicy = GetExplicitNamespacePolicy(ns, perfectMatch);

            if (explicitPolicy is not null)
                return explicitPolicy;

            return new NamespacePolicy(ns, DefaultPolicyType)
            {
                IsExplicit = false
            };
        }

        public Policy GetExplicitNamespacePolicy(NamespaceModel ns, bool perfectMatch = false)
        {
            if (perfectMatch)
                return _policies.OfType<NamespacePolicy>().SingleOrDefault(p => p.Namespace.Match(ns, true));

            var currentNamespace = ns;

            do
            {
                var policy = _policies
                    .OfType<NamespacePolicy>()
                    .SingleOrDefault(p => p.Namespace.Match(currentNamespace, true));

                if (policy is not null)
                    return policy;

                currentNamespace = currentNamespace.Parent();
            } while (currentNamespace is not null);

            return null;
        }

        public PolicyGroup AddTypePolicy(TypeModel type, PolicyType policyType)
        {
            var existingExplicitPolicy = GetExplicitTypePolicy(type);

            if (existingExplicitPolicy is not null)
                throw new Exception($"A policy for type: {type} already exists");

            _policies.Add(new TypePolicy(type, policyType));

            return this;
        }

        public PolicyGroup AllowType(TypeModel type) => AddTypePolicy(type, PolicyType.Allow);
        
        public PolicyGroup DenyType(TypeModel type) => AddTypePolicy(type, PolicyType.Deny);

        public Policy GetPolicyForType(TypeModel type)
        {
            var explicitTypePolicy = GetExplicitTypePolicy(type);

            if (explicitTypePolicy is not null)
                return explicitTypePolicy;

            if (type.Namespace is not null)
            {
                var namespacePolicy = GetPolicyForNamespace(type.Namespace);
                if (namespacePolicy?.IsExplicit == true)
                    return namespacePolicy;
            }

            return new TypePolicy(type, DefaultPolicyType)
            {
                IsExplicit = false
            };
        }

        public Policy GetExplicitTypePolicy(TypeModel type)
        {
            var policy = _policies
                .OfType<TypePolicy>()
                .SingleOrDefault(p => p.Type.Match(type));

            return policy;
        }

        public PolicyGroup AddMethodPolicy(MethodModel method, PolicyType policyType)
        {
            var existingExplicitPolicy = GetExplicitMethodPolicy(method);

            if (existingExplicitPolicy is not null)
                throw new Exception($"A policy for method: {method} already exists");

            _policies.Add(new MethodPolicy(method, policyType));

            return this;
        }

        public PolicyGroup AllowMethod(MethodModel method) => AddMethodPolicy(method, PolicyType.Allow);
        
        public PolicyGroup DenyMethod(MethodModel method) => AddMethodPolicy(method, PolicyType.Deny);

        public Policy GetMethodPolicy(MethodModel method)
        {
            var explicitMethodPolicy = GetExplicitMethodPolicy(method);

            if (explicitMethodPolicy is not null)
                return explicitMethodPolicy;

            return GetPolicyForType(method.Type);
        }
        
        public Policy GetExplicitMethodPolicy(MethodModel method)
        {
            var policy = _policies
                .OfType<MethodPolicy>()
                .SingleOrDefault(p => p.Method.Match(method));

            return policy;
        }
    }
}