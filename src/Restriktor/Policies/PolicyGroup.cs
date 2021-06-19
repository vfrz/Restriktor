using System;
using System.Collections.Generic;
using System.Linq;
using Restriktor.Core;

namespace Restriktor.Policies
{
    public class PolicyGroup
    {
        public PolicyType DefaultPolicyType { get; set; }

        internal readonly List<Policy> Policies;

        internal PolicyGroup(PolicyType defaultPolicyType = PolicyType.Deny)
        {
            DefaultPolicyType = defaultPolicyType;

            Policies = new List<Policy>();
        }

        public PolicyGroup AddNamespacePolicy(NamespaceModel ns, PolicyType policyType)
        {
            var existingExplicitPolicy = GetNamespaceExplicitPolicy(ns, true);

            if (existingExplicitPolicy is not null)
                throw new Exception($"A policy for namespace '{ns}' already exists");

            Policies.Add(new NamespacePolicy(ns, policyType));

            return this;
        }

        public PolicyGroup AllowNamespace(NamespaceModel ns) => AddNamespacePolicy(ns, PolicyType.Allow);

        public PolicyGroup DenyNamespace(NamespaceModel ns) => AddNamespacePolicy(ns, PolicyType.Deny);

        public Policy GetNamespacePolicy(NamespaceModel ns, bool perfectMatch = false)
        {
            var explicitPolicy = GetNamespaceExplicitPolicy(ns, perfectMatch);

            if (explicitPolicy is not null)
                return explicitPolicy;

            return new NamespacePolicy(ns, DefaultPolicyType)
            {
                IsExplicit = false
            };
        }

        public Policy GetNamespaceExplicitPolicy(NamespaceModel ns, bool perfectMatch = false)
        {
            if (perfectMatch)
                return Policies.OfType<NamespacePolicy>().SingleOrDefault(p => p.Namespace.Match(ns, true));

            var currentNamespace = ns;

            do
            {
                var policy = Policies
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
            var existingExplicitPolicy = GetTypeExplicitPolicy(type);

            if (existingExplicitPolicy is not null)
                throw new Exception($"A policy for type: {type} already exists");

            Policies.Add(new TypePolicy(type, policyType));

            return this;
        }

        public PolicyGroup AllowType(TypeModel type) => AddTypePolicy(type, PolicyType.Allow);
        
        public PolicyGroup DenyType(TypeModel type) => AddTypePolicy(type, PolicyType.Deny);

        public Policy GetTypePolicy(TypeModel type)
        {
            var explicitTypePolicy = GetTypeExplicitPolicy(type);

            if (explicitTypePolicy is not null)
                return explicitTypePolicy;

            if (type.Namespace is not null)
            {
                var namespacePolicy = GetNamespacePolicy(type.Namespace);
                if (namespacePolicy?.IsExplicit == true && namespacePolicy.PolicyType != PolicyType.Neutral)
                    return namespacePolicy;
            }

            return new TypePolicy(type, DefaultPolicyType)
            {
                IsExplicit = false
            };
        }

        public Policy GetTypeExplicitPolicy(TypeModel type)
        {
            var policy = Policies
                .OfType<TypePolicy>()
                .SingleOrDefault(p => p.Type.Match(type));

            return policy;
        }

        public PolicyGroup AddMethodPolicy(MethodModel method, PolicyType policyType)
        {
            var existingExplicitPolicy = GetMethodExplicitPolicy(method);

            if (existingExplicitPolicy is not null)
                throw new Exception($"A policy for method: {method} already exists");

            Policies.Add(new MethodPolicy(method, policyType));

            return this;
        }

        public PolicyGroup AllowMethod(MethodModel method) => AddMethodPolicy(method, PolicyType.Allow);
        
        public PolicyGroup DenyMethod(MethodModel method) => AddMethodPolicy(method, PolicyType.Deny);

        public Policy GetMethodPolicy(MethodModel method)
        {
            var explicitMethodPolicy = GetMethodExplicitPolicy(method);

            if (explicitMethodPolicy is not null)
                return explicitMethodPolicy;

            var typePolicy = GetTypePolicy(method.Type);
            if (typePolicy?.IsExplicit == true && typePolicy.PolicyType != PolicyType.Neutral)
                return typePolicy;

            return new MethodPolicy(method, DefaultPolicyType)
            {
                IsExplicit = false
            };
        }
        
        public Policy GetMethodExplicitPolicy(MethodModel method)
        {
            var policy = Policies
                .OfType<MethodPolicy>()
                .SingleOrDefault(p => p.Method.Match(method));

            return policy;
        }
    }
}