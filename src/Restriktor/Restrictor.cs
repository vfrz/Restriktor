using Restriktor.Policies;
using Restriktor.Validation;

namespace Restriktor
{
    public class Restrictor
    {
        public PolicyGroup Policies { get; }

        public Restrictor()
        {
            Policies = new PolicyGroup();
        }

        public ValidationResult Validate(string code)
        {
            var validator = new Validator(Policies);

            var result = validator.Validate(code);
            
            return result;
        }
    }
}