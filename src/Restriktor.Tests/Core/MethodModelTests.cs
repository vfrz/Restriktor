using NFluent;
using Restriktor.Core;
using Xunit;

namespace Restriktor.Tests.Core
{
    public class MethodModelTests
    {
        [Theory]
        [InlineData("Console.WriteLine()", "Console.WriteLine()")]
        [InlineData("System.Console.WriteLine()", "System.Console.WriteLine()")]
        [InlineData("Console.WriteLine(*)", "Console.WriteLine(*)")]
        [InlineData("Console.WriteLine(System.Object)", "Console.WriteLine(System.Object)")]
        [InlineData("Console.WriteLine(System.Object,System.String)", "Console.WriteLine(System.Object,System.String)")]
        [InlineData("Console.WriteLine(System.Object, System.String )", "Console.WriteLine(System.Object,System.String)")]
        public void StringConstructor_ToString(string method, string expected)
        {
            Check.That(new MethodModel(method).ToString()).HasSameValueAs(expected);
        }
    }
}