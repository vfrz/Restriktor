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
    }
}