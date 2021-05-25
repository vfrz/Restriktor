namespace Restriktor.Policies
{
    public class MemberPolicy : Policy
    {
        public string Member { get; }

        public MemberPolicy(string member, PolicyType policyType) : base(policyType)
        {
            Member = member;
        }
    }
}