namespace Restriktor.Policies
{
    public abstract class Policy
    {
        public PolicyType PolicyType { get; set; }

        public bool IsExplicit { get; internal init; } = true;

        public Policy(PolicyType policyType)
        {
            PolicyType = policyType;
        }
    }
}