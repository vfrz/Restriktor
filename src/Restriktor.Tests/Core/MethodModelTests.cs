using NFluent;
using Restriktor.Core;
using Xunit;

namespace Restriktor.Tests.Core
{
    public class MethodModelTests
    {
        [Theory]
        [InlineData("Console.WriteLine()", "<global namespace>.Console.WriteLine()")]
        [InlineData("System.Console.WriteLine()", "System.Console.WriteLine()")]
        [InlineData("System.Console.WriteLine(*)", "System.Console.WriteLine(*)")]
        [InlineData("System.Console.WriteLine(System.Object)", "System.Console.WriteLine(System.Object)")]
        [InlineData("System.Console.WriteLine(System.Object,System.String)", "System.Console.WriteLine(System.Object,System.String)")]
        [InlineData("System.Console.WriteLine(System.Object, System.String )", "System.Console.WriteLine(System.Object,System.String)")]
        public void Parse_ToString(string method, string expected)
        {
            Check.That(MethodModel.Parse(method).ToString()).HasSameValueAs(expected);
        }

        [Theory]
        [InlineData("System.Console.WriteLine()", "System.Console.WriteLine()", true, true)]
        [InlineData("System.Console.WriteLine()", "System.Console.WriteLine()", false, true)]
        [InlineData("System.Console.WriteLine()", "System.Console.WriteLine(System.String)", true, false)]
        [InlineData("System.Console.WriteLine()", "System.Console.WriteLine(System.String)", false, false)]
        [InlineData("System.Console.WriteLine(System.Int32)", "System.Console.WriteLine(System.String)", true, false)]
        [InlineData("System.Console.WriteLine(System.Int32)", "System.Console.WriteLine(System.String)", false, false)]
        [InlineData("System.Console.WriteLine(System.string)", "System.Console.WriteLine(System.String)", true, false)]
        [InlineData("System.Console.WriteLine(System.string)", "System.Console.WriteLine(System.String)", false, false)]
        [InlineData("System.Console.WriteLine(*)", "System.Console.WriteLine(System.String)", true, false)]
        [InlineData("System.Console.WriteLine(*)", "System.Console.WriteLine(System.String)", false, true)]
        [InlineData("System.Console.WriteLine(*)", "System.Console.WriteLine(System.String, System.Int32)", true, false)]
        [InlineData("System.Console.WriteLine(*)", "System.Console.WriteLine(System.String, System.Int32)", false, true)]
        [InlineData("System.Console.WriteLine(System.String)", "System.Console.WriteLine(System.String)", true, true)]
        [InlineData("System.Console.WriteLine(System.String)", "System.Console.WriteLine(System.String)", false, true)]
        [InlineData("System.Console.WriteLine(System.String)", "System.Console.WriteLine()", true, false)]
        [InlineData("System.Console.WriteLine(System.String)", "System.Console.WriteLine()", false, false)]
        [InlineData("System.Console.WriteLine(System.String, System.Int32)", "System.Console.WriteLine()", true, false)]
        [InlineData("System.Console.WriteLine(System.String, System.Int32)", "System.Console.WriteLine()", false, false)]
        [InlineData("System.Console.WriteLine(System.String, System.Int32)", "System.Console.WriteLine(System.String, System.Int32)", true, true)]
        [InlineData("System.Console.WriteLine(System.String, System.Int32)", "System.Console.WriteLine(System.String, System.Int32)", false, true)]
        public void Match_Nominal(string method, string another, bool perfectMatch, bool shouldMatch)
        {
            Check.That(MethodModel.Parse(method).Match(MethodModel.Parse(another), perfectMatch)).HasSameValueAs(shouldMatch);
        }
    }
}