using Restriktor.Core;

namespace Restriktor.Policies
{
    public class TypePolicy : Policy
    {
        public TypeModel Type { get; }
        
        public TypePolicy(TypeModel type, PolicyType policyType) : base(policyType)
        {
            Type = type;
        }
    }
}