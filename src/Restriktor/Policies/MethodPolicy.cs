using Restriktor.Core;

namespace Restriktor.Policies
{
    public class MethodPolicy : Policy
    {
        public MethodModel Method { get; }

        public MethodPolicy(MethodModel method, PolicyType policyType) : base(policyType)
        {
            Method = method;
        }
    }
}