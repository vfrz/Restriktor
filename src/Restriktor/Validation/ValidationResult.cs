using System.Collections.Generic;
using System.Text;

namespace Restriktor.Validation
{
    public class ValidationResult
    {
        public bool IsValid => Problems.Count == 0;

        public List<ValidationProblem> Problems { get; }

        public ValidationResult()
        {
            Problems = new List<ValidationProblem>();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append($"Validation is {(IsValid ? "valid" : "invalid, ")}");

            if (!IsValid)
            {
                builder.AppendLine($"{Problems.Count} problem(s) detected:");
                foreach (var problem in Problems)
                {
                    builder.AppendLine($"  - {problem}");
                }
            }
            
            return builder.ToString();
        }
    }
}