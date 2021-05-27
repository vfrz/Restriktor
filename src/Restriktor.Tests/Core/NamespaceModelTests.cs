using NFluent;
using Restriktor.Core;
using Xunit;

namespace Restriktor.Tests.Core
{
    public class NamespaceModelTests
    {
        [Theory]
        [InlineData("System", "System", true, true)]
        [InlineData("System.Console", "System.Console", true, true)]
        [InlineData("System", "System.Console", true, false)]
        [InlineData("System.Console", "System", true, false)]
        [InlineData("System", "Restriktor", true, false)]
        [InlineData(null, null, true, true)]
        [InlineData(null, "System", true, false)]
        [InlineData("System", null, true, false)]
        [InlineData("System", "System", false, true)]
        [InlineData("System.Console", "System.Console", false, true)]
        [InlineData("System", "System.Console", false, false)]
        [InlineData("System.Console", "System", false, true)]
        [InlineData(null, null, false, true)]
        [InlineData("System", null, false, true)]
        [InlineData(null, "System", false, false)]
        public void Match_Nominal(string baseNamespace, string anotherNamespace, bool perfectMatch, bool expectedMatch)
        {
            Check.That(NamespaceModel.Parse(baseNamespace).Match(anotherNamespace, perfectMatch)).HasSameValueAs(expectedMatch);
        }
        
        [Theory]
        [InlineData("System", null)]
        [InlineData("System.Console", "System")]
        [InlineData("System.Console.Debug", "System.Console")]
        public void Parent_Nominal(string ns, string expectedParent)
        {
            var parent = NamespaceModel.Parse(ns).Parent();
            Check.That(parent.Match(expectedParent, true)).IsTrue();
        }

        [Theory]
        [InlineData(null, "<global namespace>")]
        [InlineData("System", "System")]
        [InlineData("System.Console", "System.Console")]
        public void Parse_ToString(string ns, string expectedValue)
        {
            Check.That(NamespaceModel.Parse(ns).ToString()).HasSameValueAs(expectedValue);
        }
    }
}