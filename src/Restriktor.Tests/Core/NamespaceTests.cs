using NFluent;
using Restriktor.Core;
using Xunit;

namespace Restriktor.Tests.Core
{
    public class NamespaceTests
    {
        [Theory]
        [InlineData("System", "System", true, true)]
        [InlineData("System.Console", "System.Console", true, true)]
        [InlineData("System", "System.Console", true, false)]
        [InlineData("System.Console", "System", true, false)]
        [InlineData("System", "Restriktor", true, false)]
        [InlineData("System", "System", false, true)]
        [InlineData("System.Console", "System.Console", false, true)]
        [InlineData("System", "System.Console", false, false)]
        [InlineData("System.Console", "System", false, true)]
        public void Match_Nominal(string baseNamespace, string anotherNamespace, bool perfectMatch, bool expectedMatch)
        {
            Check.That(new NamespaceModel(baseNamespace).Match(anotherNamespace, perfectMatch)).HasSameValueAs(expectedMatch);
        }
    }
}