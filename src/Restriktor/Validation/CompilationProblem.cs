using Microsoft.CodeAnalysis;

namespace Restriktor.Validation
{
    public class CompilationProblem : ValidationProblem
    {
        public Diagnostic Diagnostic { get; }

        public CompilationProblem(Diagnostic diagnostic) : base(diagnostic.Location.GetLineSpan())
        {
            Diagnostic = diagnostic;
        }

        public override string ToString()
        {
            return Diagnostic.ToString();
        }
    }
}