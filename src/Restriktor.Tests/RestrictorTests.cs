using NFluent;
using Xunit;

namespace Restriktor.Tests
{
    public class RestrictorTests
    {
        [Fact]
        public void Validate_EmptyCode_Valid()
        {
            var restrictor = new Restrictor();

            var validationResult = restrictor.Validate("");

            Check.That(validationResult.IsValid).IsTrue();
            Check.That(validationResult.Problems).CountIs(0);
        }
    }
}