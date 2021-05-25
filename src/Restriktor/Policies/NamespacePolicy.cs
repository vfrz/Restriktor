using Restriktor.Core;

namespace Restriktor.Policies
{
    public class NamespacePolicy : Policy
    {
        public NamespaceModel Namespace { get; }

        public NamespacePolicy(NamespaceModel ns, PolicyType policyType) : base(policyType)
        {
            Namespace = ns;
        }
    }
}