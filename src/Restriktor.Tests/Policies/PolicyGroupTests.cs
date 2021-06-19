using System;
using NFluent;
using Restriktor.Policies;
using Xunit;

namespace Restriktor.Tests.Policies
{
    public class PolicyGroupTests
    {
        [Fact]
        public void AllowNamespace_OnePartNamespace_Ok()
        {
            var policyGroup = new PolicyGroup();

            policyGroup.AllowNamespace("System");

            Check.That(policyGroup.Policies).CountIs(1);
            Check.That(policyGroup.Policies[0].IsExplicit).IsTrue();
            Check.That(policyGroup.Policies[0].PolicyType).HasSameValueAs(PolicyType.Allow);
            Check.That(policyGroup.Policies[0]).IsInstanceOf<NamespacePolicy>();
            Check.That(((NamespacePolicy) policyGroup.Policies[0]).Namespace.ToString()).HasSameValueAs("System");
        }

        [Fact]
        public void AllowNamespace_ExistingPolicy_Throws()
        {
            var policyGroup = new PolicyGroup();

            policyGroup.AllowNamespace("System");

            Check.ThatCode(() =>
            {
                policyGroup.AllowNamespace("System");
            }).Throws<Exception>();
        }

        [Fact]
        public void GetNamespacePolicy_DefaultPolicy_DenyImplicit()
        {
            var policyGroup = new PolicyGroup();

            var policy = policyGroup.GetNamespacePolicy("System");

            Check.That(policy.IsExplicit).IsFalse();
            Check.That(policy.PolicyType).HasSameValueAs(PolicyType.Deny);
        }

        [Fact]
        public void GetNamespacePolicy_DefaultPolicy_AllowImplicit()
        {
            var policyGroup = new PolicyGroup(PolicyType.Allow);

            var policy = policyGroup.GetNamespacePolicy("System");

            Check.That(policy.IsExplicit).IsFalse();
            Check.That(policy.PolicyType).HasSameValueAs(PolicyType.Allow);
        }
    }
}